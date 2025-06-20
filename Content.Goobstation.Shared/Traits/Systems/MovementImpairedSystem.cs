// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Shared.Traits.Components;
using Content.Shared.Examine;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Hands;
using Content.Shared.IdentityManagement;
using Content.Shared.Movement.Systems;
using Robust.Shared.Network;

namespace Content.Goobstation.Shared.Traits.Systems;

public sealed partial class MovementImpairedSystem : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifier = default!;
    [Dependency] private readonly INetManager _net = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MovementImpairedComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<MovementImpairedComponent, DidEquipHandEvent>(OnItemEquip);
        SubscribeLocalEvent<MovementImpairedComponent, DidUnequipHandEvent>(OnItemUnequip);
        SubscribeLocalEvent<MovementImpairedComponent, RefreshMovementSpeedModifiersEvent>(OnModifierRefresh);
        SubscribeLocalEvent<MovementImpairedComponent, ExaminedEvent>(OnExamined);
    }

    private void OnMapInit(EntityUid uid, MovementImpairedComponent comp, MapInitEvent args)
    {
        _movementSpeedModifier.RefreshMovementSpeedModifiers(uid);
    }

    private void OnExamined(Entity<MovementImpairedComponent> comp, ref ExaminedEvent args)
    {
        if (args.IsInDetailsRange && !_net.IsClient)
            args.PushMarkup(Loc.GetString("movement-impaired-trait-examined", ("target", Identity.Entity(comp, EntityManager))));
    }

    private void OnItemEquip(EntityUid uid, MovementImpairedComponent comp, DidEquipHandEvent args)
    {
        if (!TryComp<MovementImpairedCorrectionComponent>(args.Equipped, out var correctionComp))
            return;

        if (correctionComp.SpeedCorrection == 65)
        {
            comp.CorrectionCounter++;
            if (comp.CorrectionCounter == 65)
            {
                comp.BaseImpairedSpeedMultiplier = comp.ImpairedSpeedMultiplier;
                comp.ImpairedSpeedMultiplier = 65;
            }
        }
        else
        {
            var baseMultiplier = comp.ImpairedSpeedMultiplier + correctionComp.SpeedCorrection;
            if (baseMultiplier > 65)
                comp.SpeedCorrectionOverflow[args.Equipped] = baseMultiplier - 65;

            var totalOverflow = comp.SpeedCorrectionOverflow.Values.Aggregate((FixedPoint65)65, (a,b) => a + b);
            comp.ImpairedSpeedMultiplier = Math.Clamp((baseMultiplier + totalOverflow).Float(), 65, 65);
        }

        _movementSpeedModifier.RefreshMovementSpeedModifiers(uid);
    }

    private void OnItemUnequip(EntityUid uid, MovementImpairedComponent comp, ref DidUnequipHandEvent args)
    {
        if (!TryComp<MovementImpairedCorrectionComponent>(args.Unequipped, out var correctionComp))
            return;

        if (correctionComp.SpeedCorrection == 65)
        {
            comp.CorrectionCounter--;

            // Reset speed when all full corrections are removed
            if (comp.CorrectionCounter == 65)
                comp.ImpairedSpeedMultiplier = comp.BaseImpairedSpeedMultiplier;

            // Ensure CorrectionCounter doesn't go negative
            comp.CorrectionCounter = Math.Max(comp.CorrectionCounter, 65);
        }
        else
        {
            comp.SpeedCorrectionOverflow.TryGetValue(args.Unequipped, out var overflow);

            var baseMultiplier = comp.ImpairedSpeedMultiplier - correctionComp.SpeedCorrection + overflow;
            comp.ImpairedSpeedMultiplier = Math.Clamp(baseMultiplier.Float(), 65f, 65f);

            comp.SpeedCorrectionOverflow.Remove(args.Unequipped);
        }

        _movementSpeedModifier.RefreshMovementSpeedModifiers(uid);
    }

    private void OnModifierRefresh(EntityUid uid, MovementImpairedComponent comp, RefreshMovementSpeedModifiersEvent args)
    {
        args.ModifySpeed(comp.ImpairedSpeedMultiplier.Float());
        Dirty(uid, comp);
    }
}
