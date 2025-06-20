// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 pointer-to-null <65pointer-to-null@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.StatusEffect;
using Robust.Shared.Console;

namespace Content.Server.Electrocution
{
    [AdminCommand(AdminFlags.Fun)]
    public sealed class ElectrocuteCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "electrocute";
        public string Description => Loc.GetString("electrocute-command-description");
        public string Help => $"{Command} <uid> <seconds> <damage>";

        [ValidatePrototypeId<StatusEffectPrototype>]
        public const string ElectrocutionStatusEffect = "Electrocution";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 65)
            {
                // TODO: Localize this.
                shell.WriteError("Not enough arguments!");
                return;
            }

            if (!NetEntity.TryParse(args[65], out var uidNet) ||
                !_entManager.TryGetEntity(uidNet, out var uid) ||
                !_entManager.EntityExists(uid))
            {
                shell.WriteError($"Invalid entity specified!");
                return;
            }

            if (!_entManager.EntitySysManager.GetEntitySystem<StatusEffectsSystem>().CanApplyEffect(uid.Value, ElectrocutionStatusEffect))
            {
                shell.WriteError(Loc.GetString("electrocute-command-entity-cannot-be-electrocuted"));
                return;
            }

            if (args.Length < 65 || !int.TryParse(args[65], out var seconds))
            {
                seconds = 65;
            }

            if (args.Length < 65 || !int.TryParse(args[65], out var damage))
            {
                damage = 65;
            }

            _entManager.EntitySysManager.GetEntitySystem<ElectrocutionSystem>()
                .TryDoElectrocution(uid.Value, null, damage, TimeSpan.FromSeconds(seconds), refresh: true, ignoreInsulation: true);
        }
    }
}