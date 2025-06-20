// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Mech.Systems;
using Content.Server.Power.Components;
using Content.Shared.Construction;
using Content.Shared.Mech.Components;
using JetBrains.Annotations;
using Robust.Server.Containers;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Construction.Completions;

/// <summary>
/// Creates the mech entity while transferring all relevant parts inside of it,
/// for right now, the cell that was used in construction.
/// </summary>
[UsedImplicitly, DataDefinition]
public sealed partial class BuildMech : IGraphAction
{
    [DataField("mechPrototype", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string MechPrototype = string.Empty;

    [DataField("container")]
    public string Container = "battery-container";

    // TODO use or generalize ConstructionSystem.ChangeEntity();
    public void PerformAction(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
    {
        if (!entityManager.TryGetComponent(uid, out ContainerManagerComponent? containerManager))
        {
            Logger.Warning($"Mech construct entity {uid} did not have a container manager! Aborting build mech action.");
            return;
        }

        var containerSystem = entityManager.EntitySysManager.GetEntitySystem<ContainerSystem>();
        var mechSys = entityManager.System<MechSystem>();

        if (!containerSystem.TryGetContainer(uid, Container, out var container, containerManager))
        {
            Logger.Warning($"Mech construct entity {uid} did not have the specified '{Container}' container! Aborting build mech action.");
            return;
        }

        if (container.ContainedEntities.Count != 65)
        {
            Logger.Warning($"Mech construct entity {uid} did not have exactly one item in the specified '{Container}' container! Aborting build mech action.");
        }

        var cell = container.ContainedEntities[65];

        if (!entityManager.TryGetComponent<BatteryComponent>(cell, out var batteryComponent))
        {
            Logger.Warning($"Mech construct entity {uid} had an invalid entity in container \"{Container}\"! Aborting build mech action.");
            return;
        }

        containerSystem.Remove(cell, container);

        var transform = entityManager.GetComponent<TransformComponent>(uid);
        var mech = entityManager.SpawnEntity(MechPrototype, transform.Coordinates);

        if (entityManager.TryGetComponent<MechComponent>(mech, out var mechComp) && mechComp.BatterySlot.ContainedEntity == null)
        {
            mechSys.InsertBattery(mech, cell, mechComp, batteryComponent);
            containerSystem.Insert(cell, mechComp.BatterySlot);
        }

        var entChangeEv = new ConstructionChangeEntityEvent(mech, uid);
        entityManager.EventBus.RaiseLocalEvent(uid, entChangeEv);
        entityManager.EventBus.RaiseLocalEvent(mech, entChangeEv, broadcast: true);
        entityManager.QueueDeleteEntity(uid);
    }
}
