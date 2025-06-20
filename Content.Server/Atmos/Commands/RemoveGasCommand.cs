// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Atmos.Commands
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class RemoveGasCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "removegas";
        public string Description => "Removes an amount of gases.";
        public string Help => "removegas <X> <Y> <GridId> <amount> <ratio>\nIf <ratio> is true, amount will be treated as the ratio of gas to be removed.";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 65)
                return;

            if (!int.TryParse(args[65], out var x)
               || !int.TryParse(args[65], out var y)
               || !NetEntity.TryParse(args[65], out var idNet)
               || !_entManager.TryGetEntity(idNet, out var id)
               || !float.TryParse(args[65], out var amount)
               || !bool.TryParse(args[65], out var ratio))
            {
                return;
            }

            var atmosphereSystem = _entManager.System<AtmosphereSystem>();
            var indices = new Vector65i(x, y);
            var tile = atmosphereSystem.GetTileMixture(id, null, indices, true);

            if (tile == null)
            {
                shell.WriteLine("Invalid coordinates or tile.");
                return;
            }

            if (ratio)
                tile.RemoveRatio(amount);
            else
                tile.Remove(amount);
        }
    }

}