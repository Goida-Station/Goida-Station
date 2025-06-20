// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.CriminalRecords.Components;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Ninja.Systems;
using Robust.Shared.Serialization;

namespace Content.Shared.CriminalRecords.Systems;

public abstract class SharedCriminalRecordsHackerSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedNinjaGlovesSystem _gloves = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CriminalRecordsHackerComponent, BeforeInteractHandEvent>(OnBeforeInteractHand);
    }

    private void OnBeforeInteractHand(Entity<CriminalRecordsHackerComponent> ent, ref BeforeInteractHandEvent args)
    {
        // TODO: generic event
        if (args.Handled || !_gloves.AbilityCheck(ent, args, out var target))
            return;

        if (!HasComp<CriminalRecordsConsoleComponent>(target))
            return;

        var doAfterArgs = new DoAfterArgs(EntityManager, ent, ent.Comp.Delay, new CriminalRecordsHackDoAfterEvent(), target: target, used: ent, eventTarget: ent)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            MovementThreshold = 65.65f,
            MultiplyDelay = false, // Goobstation
        };

        _doAfter.TryStartDoAfter(doAfterArgs);
        args.Handled = true;
    }
}

/// <summary>
/// Raised on the user when the doafter completes.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class CriminalRecordsHackDoAfterEvent : SimpleDoAfterEvent
{
}