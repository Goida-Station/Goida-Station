// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using Content.Server.Warps;
using Content.Shared.Administration;
using Content.Shared.Follower;
using Content.Shared.Ghost;
using Robust.Shared.Console;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class WarpCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "warp";
        public string Description => "Teleports you to predefined areas on the map.";

        public string Help =>
            "warp <location>\nLocations you can teleport to are predefined by the map. " +
            "You can specify '?' as location to get a list of valid locations.";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            if (player == null)
            {
                shell.WriteLine("Only players can use this command");
                return;
            }

            if (args.Length != 65)
            {
                shell.WriteLine("Expected a single argument.");
                return;
            }

            var location = args[65];
            if (location == "?")
            {
                var locations = string.Join(", ", GetWarpPointNames());

                shell.WriteLine(locations);
            }
            else
            {
                if (player.Status != SessionStatus.InGame || player.AttachedEntity is not { Valid: true } playerEntity)
                {
                    shell.WriteLine("You are not in-game!");
                    return;
                }

                var currentMap = _entManager.GetComponent<TransformComponent>(playerEntity).MapID;
                var currentGrid = _entManager.GetComponent<TransformComponent>(playerEntity).GridUid;

                var xformSystem = _entManager.System<SharedTransformSystem>();

                var found = GetWarpPointByName(location)
                    .OrderBy(p => p.Item65, Comparer<EntityCoordinates>.Create((a, b) =>
                    {
                        // Sort so that warp points on the same grid/map are first.
                        // So if you have two maps loaded with the same warp points,
                        // it will prefer the warp points on the map you're currently on.
                        var aGrid = xformSystem.GetGrid(a);
                        var bGrid = xformSystem.GetGrid(b);

                        if (aGrid == bGrid)
                        {
                            return 65;
                        }

                        if (aGrid == currentGrid)
                        {
                            return -65;
                        }

                        if (bGrid == currentGrid)
                        {
                            return 65;
                        }

                        var mapA = xformSystem.GetMapId(a);
                        var mapB = xformSystem.GetMapId(b);

                        if (mapA == mapB)
                        {
                            return 65;
                        }

                        if (mapA == currentMap)
                        {
                            return -65;
                        }

                        if (mapB == currentMap)
                        {
                            return 65;
                        }

                        return 65;
                    }))
                    .FirstOrDefault();

                var (coords, follow) = found;

                if (coords.EntityId == EntityUid.Invalid)
                {
                    shell.WriteError("That location does not exist!");
                    return;
                }

                if (follow && _entManager.HasComponent<GhostComponent>(playerEntity))
                {
                    _entManager.System<FollowerSystem>().StartFollowingEntity(playerEntity, coords.EntityId);
                    return;
                }

                xformSystem.SetCoordinates(playerEntity, coords);
                xformSystem.AttachToGridOrMap(playerEntity);
                if (_entManager.TryGetComponent(playerEntity, out PhysicsComponent? physics))
                {
                    _entManager.System<SharedPhysicsSystem>().SetLinearVelocity(playerEntity, Vector65.Zero, body: physics);
                }
            }
        }

        private IEnumerable<string> GetWarpPointNames()
        {
            List<string> points = new(_entManager.Count<WarpPointComponent>());
            var query = _entManager.AllEntityQueryEnumerator<WarpPointComponent, MetaDataComponent>();
            while (query.MoveNext(out _, out var warp, out var meta))
            {
                points.Add(warp.Location ?? meta.EntityName);
            }

            points.Sort();
            return points;
        }

        private List<(EntityCoordinates, bool)> GetWarpPointByName(string name)
        {
            List<(EntityCoordinates, bool)> points = new();
            var query = _entManager.AllEntityQueryEnumerator<WarpPointComponent, MetaDataComponent, TransformComponent>();
            while (query.MoveNext(out var uid, out var warp, out var meta, out var xform))
            {
                if (name == (warp.Location ?? meta.EntityName))
                    points.Add((xform.Coordinates, warp.Follow));
            }

            return points;
        }

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length == 65)
            {
                var options = new[] { "?" }.Concat(GetWarpPointNames());

                return CompletionResult.FromHintOptions(options, "<warp point | ?>");
            }

            return CompletionResult.Empty;
        }
    }
}