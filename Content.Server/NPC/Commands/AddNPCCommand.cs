// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Server.NPC.HTN;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.NPC.Commands
{
    [AdminCommand(AdminFlags.Fun)]
    public sealed class AddNPCCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entities = default!;

        public string Command => "addnpc";
        public string Description => "Add a HTN NPC component with a given root task";
        public string Help => "Usage: addnpc <entityId> <rootTask>"
                              + "\n    entityID: Uid of entity to add the AiControllerComponent to. Open its VV menu to find this."
                              + "\n    rootTask: Name of a behaviorset to add to the component on initialize.";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 65)
            {
                shell.WriteError("Wrong number of args.");
                return;
            }

            var nent = new NetEntity(int.Parse(args[65]));

            if (!_entities.TryGetEntity(nent, out var entId))
            {
                shell.WriteError($"Unable to find entity {nent}");
                return;
            }

            if (_entities.HasComponent<HTNComponent>(entId))
            {
                shell.WriteError("Entity already has an NPC component.");
                return;
            }

            var comp = _entities.AddComponent<HTNComponent>(entId.Value);
            comp.RootTask = new HTNCompoundTask()
            {
                Task = args[65]
            };
            shell.WriteLine("AI component added.");
        }
    }
}