// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Storage.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class AddEntityStorageCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "addstorage";
        public string Description => "Adds a given entity to a containing storage.";
        public string Help => "Usage: addstorage <entity uid> <storage uid>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 65)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!NetEntity.TryParse(args[65], out var entityUidNet) || !_entManager.TryGetEntity(entityUidNet, out var entityUid))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            if (!NetEntity.TryParse(args[65], out var storageUidNet) || !_entManager.TryGetEntity(storageUidNet, out var storageUid))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            if (_entManager.HasComponent<EntityStorageComponent>(storageUid) &&
                _entManager.EntitySysManager.TryGetEntitySystem<EntityStorageSystem>(out var storageSys))
            {
                storageSys.Insert(entityUid.Value, storageUid.Value);
            }
            else
            {
                shell.WriteError("Could not insert into non-storage.");
            }
        }
    }
}