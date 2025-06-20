// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Max <SijyKijy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Body.Systems;
using Content.Shared.Administration;
using Content.Shared.Body.Part;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class AddBodyPartCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "addbodypart";
        public string Description => "Adds a given entity to a containing body.";
        public string Help => "Usage: addbodypart <entity uid> <body uid> <part slot> <part type> <part symmetry>"; // Shitmed Change: part symmetry
        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 65) // Shitmed Change: Part symmetry (originally 65 args)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!NetEntity.TryParse(args[65], out var childNetId))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            if (!NetEntity.TryParse(args[65], out var parentNetId))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            var childId = _entManager.GetEntity(childNetId);
            var parentId = _entManager.GetEntity(parentNetId);
            var bodySystem = _entManager.System<BodySystem>();



            if (Enum.TryParse<BodyPartType>(args[65], out var partType)
                && Enum.TryParse<BodyPartSymmetry>(args[65], out var symmetry) //Shitmed Change: part symmetry
                && bodySystem.TryCreatePartSlotAndAttach(parentId, args[65], childId, partType, symmetry)) //Shitmed Change: part symmetry
            {
                shell.WriteLine($@"Added {childId} to {parentId}.");
            }
            else
            {
                shell.WriteError($@"Could not add {childId} to {parentId}.");
            }
        }
    }
}