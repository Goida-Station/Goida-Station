// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.UserInterface;
using Content.Shared.Eye.Blinding.Components;
using Content.Shared.Popups;
using Robust.Shared.Collections;

namespace Content.Shared.Eye.Blinding.Systems;

public sealed class ActivatableUIRequiresVisionSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _userInterfaceSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ActivatableUIRequiresVisionComponent, ActivatableUIOpenAttemptEvent>(OnOpenAttempt);
        SubscribeLocalEvent<BlindableComponent, BlindnessChangedEvent>(OnBlindnessChanged);
    }

    private void OnOpenAttempt(EntityUid uid, ActivatableUIRequiresVisionComponent component, ActivatableUIOpenAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (TryComp<BlindableComponent>(args.User, out var blindable) && blindable.IsBlind)
        {
            _popupSystem.PopupClient(Loc.GetString("blindness-fail-attempt"), args.User, Shared.Popups.PopupType.MediumCaution);
            args.Cancel();
        }
    }

    private void OnBlindnessChanged(EntityUid uid, BlindableComponent component, ref BlindnessChangedEvent args)
    {
        if (!args.Blind)
            return;

        var toClose = new ValueList<(EntityUid Entity, Enum Key)>();

        foreach (var bui in _userInterfaceSystem.GetActorUis(uid))
        {
            if (HasComp<ActivatableUIRequiresVisionComponent>(bui.Entity))
            {
                toClose.Add(bui);
            }
        }

        foreach (var bui in toClose)
        {
            _userInterfaceSystem.CloseUi(bui.Entity, bui.Key, uid);
        }
    }
}