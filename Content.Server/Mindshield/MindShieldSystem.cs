// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 coolmankid65 <65coolmankid65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coolmankid65 <coolmankid65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs;
using Content.Server.Mind;
using Content.Server.Popups;
using Content.Server.Revolutionary.Components; // GoobStation
using Content.Server.Roles;
using Content.Shared.Database;
using Content.Shared.Implants;
using Content.Shared.Mindshield.Components;
using Content.Shared.Revolutionary; // GoobStation
using Content.Shared.Revolutionary.Components;
using Robust.Shared.Containers;

namespace Content.Server.Mindshield;

/// <summary>
/// System used for adding or removing components with a mindshield implant
/// as well as checking if the implanted is a Rev or Head Rev.
/// </summary>
public sealed class MindShieldSystem : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _adminLogManager = default!;
    [Dependency] private readonly RoleSystem _roleSystem = default!;
    [Dependency] private readonly MindSystem _mindSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedRevolutionarySystem _revolutionarySystem = default!; // Goobstation

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MindShieldImplantComponent, ImplantImplantedEvent>(OnImplantImplanted);
        SubscribeLocalEvent<MindShieldImplantComponent, EntGotRemovedFromContainerMessage>(OnImplantDraw);
    }

    private void OnImplantImplanted(Entity<MindShieldImplantComponent> ent, ref ImplantImplantedEvent ev)
    {
        if (ev.Implanted == null)
            return;

        EnsureComp<MindShieldComponent>(ev.Implanted.Value);
        MindShieldRemovalCheck(ev.Implanted.Value, ev.Implant);

        // GoobStation
        if (!TryComp<CommandStaffComponent>(ev.Implanted, out var commandComp))
            return;

        commandComp.Enabled = true;
    }

    /// <summary>
    /// Checks if the implanted person was a Rev or Head Rev and remove role or destroy mindshield respectively.
    /// </summary>
    private void MindShieldRemovalCheck(EntityUid implanted, EntityUid implant)
    {
        if (TryComp<HeadRevolutionaryComponent>(implanted, out var headRevComp)) // GoobStation - headRevComp
        {
            _popupSystem.PopupEntity(Loc.GetString("head-rev-break-mindshield"), implanted);
            _revolutionarySystem.ToggleConvertAbility((implanted, headRevComp), false); // GoobStation - turn off headrev ability to convert
            //QueueDel(implant); - Goobstation - Headrevs should remove implant before turning on ability
            return;
        }

        if (_mindSystem.TryGetMind(implanted, out var mindId, out _) &&
            _roleSystem.MindTryRemoveRole<RevolutionaryRoleComponent>(mindId))
        {
            _adminLogManager.Add(LogType.Mind, LogImpact.Medium, $"{ToPrettyString(implanted)} was deconverted due to being implanted with a Mindshield.");
        }
        if (HasComp<Goobstation.Shared.Mindcontrol.MindcontrolledComponent>(implanted))   //Goobstation - Mindcontrol Implant
            RemComp<Goobstation.Shared.Mindcontrol.MindcontrolledComponent>(implanted);
    }

    private void OnImplantDraw(Entity<MindShieldImplantComponent> ent, ref EntGotRemovedFromContainerMessage args)
    {
        _popupSystem.PopupEntity(Loc.GetString("mindshield-implant-effect-removed"), args.Container.Owner, args.Container.Owner);

        if (TryComp<HeadRevolutionaryComponent>(args.Container.Owner, out var headRevComp))
            _revolutionarySystem.ToggleConvertAbility((args.Container.Owner, headRevComp), true);

        RemComp<MindShieldComponent>(args.Container.Owner);
    }
}

