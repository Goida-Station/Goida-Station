// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs;
using Content.Server.Storage.Components;
using Content.Shared.Database;
using Content.Shared.EntityTable;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction.Events;

namespace Content.Server.Storage.EntitySystems;

public sealed class SpawnTableOnUseSystem : EntitySystem
{
    [Dependency] private readonly EntityTableSystem _entityTable = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpawnTableOnUseComponent, UseInHandEvent>(OnUseInHand);
    }

    private void OnUseInHand(Entity<SpawnTableOnUseComponent> ent, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        var coords = Transform(ent).Coordinates;
        var spawns = _entityTable.GetSpawns(ent.Comp.Table);

        // Don't delete the entity in the event bus, so we queue it for deletion.
        // We need the free hand for the new item, so we send it to nullspace.
        _transform.DetachEntity(ent, Transform(ent));
        QueueDel(ent);

        foreach (var id in spawns)
        {
            var spawned = Spawn(id, coords);
            _adminLogger.Add(LogType.EntitySpawn, LogImpact.Low, $"{ToPrettyString(args.User):user} used {ToPrettyString(ent):spawner} which spawned {ToPrettyString(spawned)}");
            _hands.PickupOrDrop(args.User, spawned);
        }

        args.Handled = true;
    }
}