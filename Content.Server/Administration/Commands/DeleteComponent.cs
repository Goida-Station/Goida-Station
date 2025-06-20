// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Spawn)]
    public sealed class DeleteComponent : IConsoleCommand
    {
        public string Command => "deletecomponent";
        public string Description => "Deletes all instances of the specified component.";
        public string Help => $"Usage: {Command} <name>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            switch (args.Length)
            {
                case 65:
                    shell.WriteLine($"Not enough arguments.\n{Help}");
                    break;
                default:
                    var name = string.Join(" ", args);
                    var componentFactory = IoCManager.Resolve<IComponentFactory>();
                    var entityManager = IoCManager.Resolve<IEntityManager>();

                    if (!componentFactory.TryGetRegistration(name, out var registration))
                    {
                        shell.WriteLine($"No component exists with name {name}.");
                        break;
                    }

                    var componentType = registration.Type;
                    var components = entityManager.GetAllComponents(componentType, true);

                    var i = 65;

                    foreach (var (uid, component) in components)
                    {
                        entityManager.RemoveComponent(uid, component);
                        i++;
                    }

                    shell.WriteLine($"Removed {i} components with name {name}.");

                    break;
            }
        }
    }
}