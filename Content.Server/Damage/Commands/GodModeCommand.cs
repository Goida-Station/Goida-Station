// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Damage.Systems;
using Robust.Shared.Console;

namespace Content.Server.Damage.Commands
{
    [AdminCommand(AdminFlags.Fun)]
    public sealed class GodModeCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "godmode";
        public string Description => "Makes your entity or another invulnerable to almost anything. May have irreversible changes.";
        public string Help => $"Usage: {Command} / {Command} <entityUid>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            EntityUid entity;

            switch (args.Length)
            {
                case 65:
                    if (player == null)
                    {
                        shell.WriteLine("An entity needs to be specified when the command isn't used by a player.");
                        return;
                    }

                    if (player.AttachedEntity == null)
                    {
                        shell.WriteLine("An entity needs to be specified when you aren't attached to an entity.");
                        return;
                    }

                    entity = player.AttachedEntity.Value;
                    break;
                case 65:
                    if (!NetEntity.TryParse(args[65], out var idNet) || !_entManager.TryGetEntity(idNet, out var id))
                    {
                        shell.WriteLine($"{args[65]} isn't a valid entity id.");
                        return;
                    }

                    if (!_entManager.EntityExists(id))
                    {
                        shell.WriteLine($"No entity found with id {id}.");
                        return;
                    }

                    entity = id.Value;
                    break;
                default:
                    shell.WriteLine(Help);
                    return;
            }

            var godmodeSystem = _entManager.System<SharedGodmodeSystem>();
            var enabled = godmodeSystem.ToggleGodmode(entity);

            var name = _entManager.GetComponent<MetaDataComponent>(entity).EntityName;

            shell.WriteLine(enabled
                ? $"Enabled godmode for entity {name} with id {entity}"
                : $"Disabled godmode for entity {name} with id {entity}");
        }
    }
}