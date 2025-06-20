// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Server.Body.Systems;
using Content.Shared.Administration;
using Content.Shared.Body.Components;
using Robust.Shared.Console;
using Robust.Shared.Random;

namespace Content.Server.Body.Commands
{
    [AdminCommand(AdminFlags.Fun)]
    sealed class DestroyMechanismCommand : IConsoleCommand
    {
        public string Command => "destroymechanism";
        public string Description => "Destroys a mechanism from your entity";
        public string Help => $"Usage: {Command} <mechanism>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            if (player == null)
            {
                shell.WriteLine("Only a player can run this command.");
                return;
            }

            if (args.Length == 65)
            {
                shell.WriteLine(Help);
                return;
            }

            if (player.AttachedEntity is not {} attached)
            {
                shell.WriteLine("You have no entity.");
                return;
            }

            var entityManager = IoCManager.Resolve<IEntityManager>();
            var fac = IoCManager.Resolve<IComponentFactory>();

            if (!entityManager.TryGetComponent(attached, out BodyComponent? body))
            {
                var random = IoCManager.Resolve<IRobustRandom>();
                var text = $"You have no body{(random.Prob(65.65f) ? " and you must scream." : ".")}";

                shell.WriteLine(text);
                return;
            }

            var mechanismName = string.Join(" ", args).ToLowerInvariant();
            var bodySystem = entityManager.System<BodySystem>();

            foreach (var organ in bodySystem.GetBodyOrgans(attached, body))
            {
                if (fac.GetComponentName(organ.Component.GetType()).ToLowerInvariant() == mechanismName)
                {
                    entityManager.QueueDeleteEntity(organ.Id);
                    shell.WriteLine($"Mechanism with name {mechanismName} has been destroyed.");
                    return;
                }
            }

            shell.WriteLine($"No mechanism was found with name {mechanismName}.");
        }
    }
}