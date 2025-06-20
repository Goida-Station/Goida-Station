// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared._Goobstation.Wizard.Traps;

namespace Content.Server._Goobstation.Wizard.Systems;

public sealed class WizardTrapsSystem : SharedWizardTrapsSystem
{
    [Dependency] private readonly FlammableSystem _flammable = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FlameTrapComponent, TrapTriggeredEvent>(OnFlameTriggered);
    }

    private void OnFlameTriggered(Entity<FlameTrapComponent> ent, ref TrapTriggeredEvent args)
    {
        var (_, comp) = ent;
        var victim = args.Victim;

        if (TryComp(victim, out FlammableComponent? flammable))
            _flammable.AdjustFireStacks(victim, comp.FireStacks, flammable, true);
    }
}