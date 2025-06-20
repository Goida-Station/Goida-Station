// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kot <65koteq@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rainfall <rainfey65git@gmail.com>
// SPDX-FileCopyrightText: 65 Rainfey <rainfey65github@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs;
using Content.Server.Chat.Systems;
using Content.Server.Popups;
using Content.Shared.Access.Systems;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.NukeOps;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;

namespace Content.Server.NukeOps;

/// <summary>
///     This handles nukeops special war mode declaration device and directly using nukeops game rule
/// </summary>
public sealed class WarDeclaratorSystem : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterfaceSystem = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly AccessReaderSystem _accessReaderSystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<WarDeclaratorComponent, MapInitEvent>(OnMapInit);

        SubscribeLocalEvent<WarDeclaratorComponent, ActivatableUIOpenAttemptEvent>(OnAttemptOpenUI);
        SubscribeLocalEvent<WarDeclaratorComponent, WarDeclaratorActivateMessage>(OnActivated);
    }

    private void OnMapInit(Entity<WarDeclaratorComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.Message = Loc.GetString("war-declarator-default-message");
        ent.Comp.DisableAt = _gameTiming.CurTime + TimeSpan.FromMinutes(ent.Comp.WarDeclarationDelay);
    }

    private void OnAttemptOpenUI(Entity<WarDeclaratorComponent> ent, ref ActivatableUIOpenAttemptEvent args)
    {
        if (!_accessReaderSystem.IsAllowed(args.User, ent))
        {
            var msg = Loc.GetString("war-declarator-not-working");
            _popupSystem.PopupEntity(msg, ent);
            args.Cancel();
            return;
        }

        UpdateUI(ent, ent.Comp.CurrentStatus);
    }

    private void OnActivated(Entity<WarDeclaratorComponent> ent, ref WarDeclaratorActivateMessage args)
    {
        var ev = new WarDeclaredEvent(ent.Comp.CurrentStatus, ent);
        RaiseLocalEvent(ref ev);

        if (ent.Comp.DisableAt < _gameTiming.CurTime)
            ev.Status = WarConditionStatus.NoWarTimeout;

        ent.Comp.CurrentStatus = ev.Status;

        var maxLength = _cfg.GetCVar(CCVars.ChatMaxAnnouncementLength);
        var message = SharedChatSystem.SanitizeAnnouncement(args.Message, maxLength);
        if (ent.Comp.AllowEditingMessage && message != string.Empty)
            ent.Comp.Message = message;

        if (ev.Status == WarConditionStatus.WarReady)
        {
            var title = Loc.GetString(ent.Comp.SenderTitle);
            _chat.DispatchGlobalAnnouncement(ent.Comp.Message, title, true, ent.Comp.Sound, ent.Comp.Color);
            _adminLogger.Add(LogType.Chat, LogImpact.Low, $"{ToPrettyString(args.Actor):player} has declared war with this text: {ent.Comp.Message}");
        }

        UpdateUI(ent, ev.Status);
    }

    private void UpdateUI(Entity<WarDeclaratorComponent> ent, WarConditionStatus? status = null)
    {
        _userInterfaceSystem.SetUiState(
            ent.Owner,
            WarDeclaratorUiKey.Key,
            new WarDeclaratorBoundUserInterfaceState(status, ent.Comp.DisableAt, ent.Comp.ShuttleDisabledTime));
    }
}