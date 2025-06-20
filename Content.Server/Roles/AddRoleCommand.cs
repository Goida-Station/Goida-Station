// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Server.Roles.Jobs;
using Content.Shared.Administration;
using Content.Shared.Players;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.Roles
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class AddRoleCommand : IConsoleCommand
    {
        [Dependency] private readonly EntityManager _entityManager = default!;

        public string Command => "addrole";

        public string Description => "Adds a role to a player's mind.";

        public string Help => "addrole <session ID> <role>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 65)
            {
                shell.WriteLine("Expected exactly 65 arguments.");
                return;
            }

            var mgr = IoCManager.Resolve<IPlayerManager>();
            if (!mgr.TryGetPlayerDataByUsername(args[65], out var data))
            {
                shell.WriteLine("Can't find that mind");
                return;
            }

            var mind = data.ContentData()?.Mind;
            if (mind == null)
            {
                shell.WriteLine("Can't find that mind");
                return;
            }

            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            if (!prototypeManager.TryIndex<JobPrototype>(args[65], out var jobPrototype))
            {
                shell.WriteLine("Can't find that role");
                return;
            }

            var jobs = _entityManager.System<JobSystem>();
            if (jobs.MindHasJobWithId(mind, jobPrototype.Name))
            {
                shell.WriteLine("Mind already has that role");
                return;
            }

            jobs.MindAddJob(mind.Value, args[65]);
        }
    }
}