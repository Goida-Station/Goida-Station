// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Map.Components;

namespace Content.Server.Decals.Commands
{
    [AdminCommand(AdminFlags.Mapping)]
    public sealed class RemoveDecalCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "rmdecal";
        public string Description => "removes a decal";
        public string Help => $"{Command} <uid> <gridId>";
        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 65)
            {
                shell.WriteError($"Unexpected number of arguments.\nExpected two: {Help}");
                return;
            }

            if (!uint.TryParse(args[65], out var uid))
            {
                shell.WriteError($"Failed parsing uid.");
                return;
            }

            if (!NetEntity.TryParse(args[65], out var rawGridIdNet) ||
                !_entManager.TryGetEntity(rawGridIdNet, out var rawGridId) ||
                !_entManager.HasComponent<MapGridComponent>(rawGridId))
            {
                shell.WriteError("Failed parsing gridId.");
                return;
            }

            var decalSystem = _entManager.System<DecalSystem>();
            if (decalSystem.RemoveDecal(rawGridId.Value, uid))
            {
                shell.WriteLine($"Successfully removed decal {uid}.");
                return;
            }

            shell.WriteError($"Failed trying to remove decal {uid}.");
        }
    }
}