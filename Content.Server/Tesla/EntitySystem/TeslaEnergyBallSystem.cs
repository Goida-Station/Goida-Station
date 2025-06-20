// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Singularity.Components;
using Content.Server.Tesla.Components;
using Robust.Server.Audio;
using Content.Server.Singularity.Events;

namespace Content.Server.Tesla.EntitySystems;

/// <summary>
/// A component that tracks an entity's saturation level from absorbing other creatures by touch, and spawns new entities when the saturation limit is reached.
/// </summary>
public sealed class TeslaEnergyBallSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audio = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TeslaEnergyBallComponent, EntityConsumedByEventHorizonEvent>(OnConsumed);
    }

    private void OnConsumed(Entity<TeslaEnergyBallComponent> tesla, ref EntityConsumedByEventHorizonEvent args)
    {
        Spawn(tesla.Comp.ConsumeEffectProto, Transform(args.Entity).Coordinates);
        if (TryComp<SinguloFoodComponent>(args.Entity, out var singuloFood))
        {
            AdjustEnergy(tesla, tesla.Comp, singuloFood.Energy);
        } else
        {
            AdjustEnergy(tesla, tesla.Comp, tesla.Comp.ConsumeStuffEnergy);
        }
    }

    public void AdjustEnergy(EntityUid uid, TeslaEnergyBallComponent component, float delta)
    {
        component.Energy += delta;

        if (component.Energy > component.NeedEnergyToSpawn)
        {
            component.Energy -= component.NeedEnergyToSpawn;
            Spawn(component.SpawnProto, Transform(uid).Coordinates);
        }
        if (component.Energy < component.EnergyToDespawn)
        {
            _audio.PlayPvs(component.SoundCollapse, uid);
            QueueDel(uid);
        }
    }
}