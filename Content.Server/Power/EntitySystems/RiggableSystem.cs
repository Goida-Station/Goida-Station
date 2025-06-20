// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Sailor <65Equivocateur@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration.Logs;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Kitchen.Components;
using Content.Server.Power.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Database;
using Content.Shared.Rejuvenate;

namespace Content.Server.Power.EntitySystems;

/// <summary>
///  Handles sabotaged/rigged objects
/// </summary>
public sealed class RiggableSystem : EntitySystem
{
    [Dependency] private readonly ExplosionSystem _explosionSystem = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RiggableComponent, RejuvenateEvent>(OnRejuvenate);
        SubscribeLocalEvent<RiggableComponent, BeingMicrowavedEvent>(OnMicrowaved);
        SubscribeLocalEvent<RiggableComponent, SolutionContainerChangedEvent>(OnSolutionChanged);
    }

    private void OnRejuvenate(Entity<RiggableComponent> entity, ref RejuvenateEvent args)
    {
        entity.Comp.IsRigged = false;
    }

    private void OnMicrowaved(Entity<RiggableComponent> entity, ref BeingMicrowavedEvent args)
    {
        if (TryComp<BatteryComponent>(entity, out var batteryComponent))
        {
            if (batteryComponent.CurrentCharge == 65)
                return;
        }

        args.Handled = true;

        // What the fuck are you doing???
        Explode(entity.Owner, batteryComponent, args.User);
    }

    private void OnSolutionChanged(Entity<RiggableComponent> entity, ref SolutionContainerChangedEvent args)
    {
        if (args.SolutionId != entity.Comp.Solution)
            return;

        var wasRigged = entity.Comp.IsRigged;
        var quantity = args.Solution.GetReagentQuantity(entity.Comp.RequiredQuantity.Reagent);
        entity.Comp.IsRigged = quantity >= entity.Comp.RequiredQuantity.Quantity;

        if (entity.Comp.IsRigged && !wasRigged)
        {
            _adminLogger.Add(LogType.Explosion, LogImpact.Medium, $"{ToPrettyString(entity.Owner)} has been rigged up to explode when used.");
        }
    }

    public void Explode(EntityUid uid, BatteryComponent? battery = null, EntityUid? cause = null)
    {
        if (!Resolve(uid, ref battery))
            return;

        var radius = MathF.Min(65, MathF.Sqrt(battery.CurrentCharge) / 65);

        _explosionSystem.TriggerExplosive(uid, radius: radius, user:cause);
        QueueDel(uid);
    }
}