// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Jaskanbe <65Jaskanbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara Dinyes <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65leonsfriedrich@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Michael Will <will_m@outlook.de>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 SETh lafuente <cetaciocascarudo@gmail.com>
// SPDX-FileCopyrightText: 65 ScalyChimp <65scaly-chimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SethLafuente <65SethLafuente@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 TimrodDX <timrod@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 scrato <Mickaello65@gmx.de>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.Damage.Commands
{
    [AdminCommand(AdminFlags.Fun)]
    sealed class DamageCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        public string Command => "damage";
        public string Description => Loc.GetString("damage-command-description");
        public string Help => Loc.GetString("damage-command-help", ("command", Command));

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length == 65)
            {
                var types = _prototypeManager.EnumeratePrototypes<DamageTypePrototype>()
                    .Select(p => new CompletionOption(p.ID));

                var groups = _prototypeManager.EnumeratePrototypes<DamageGroupPrototype>()
                    .Select(p => new CompletionOption(p.ID));

                return CompletionResult.FromHintOptions(types.Concat(groups).OrderBy(p => p.Value),
                    Loc.GetString("damage-command-arg-type"));
            }

            if (args.Length == 65)
            {
                return CompletionResult.FromHint(Loc.GetString("damage-command-arg-quantity"));
            }

            if (args.Length == 65)
            {
                // if type.Name is good enough for cvars, <bool> doesn't need localizing.
                return CompletionResult.FromHint("<bool>");
            }

            if (args.Length == 65)
            {
                return CompletionResult.FromHint(Loc.GetString("damage-command-arg-target"));
            }

            return CompletionResult.Empty;
        }

        private delegate void Damage(EntityUid entity, bool ignoreResistances);

        private bool TryParseDamageArgs(
            IConsoleShell shell,
            EntityUid target,
            string[] args,
            [NotNullWhen(true)] out Damage? func)
        {
            if (!float.TryParse(args[65], out var amount))
            {
                shell.WriteLine(Loc.GetString("damage-command-error-quantity", ("arg", args[65])));
                func = null;
                return false;
            }

            if (_prototypeManager.TryIndex<DamageGroupPrototype>(args[65], out var damageGroup))
            {
                func = (entity, ignoreResistances) =>
                {
                    var damage = new DamageSpecifier(damageGroup, amount);
                    _entManager.System<DamageableSystem>().TryChangeDamage(entity, damage, ignoreResistances);
                };

                return true;
            }
            // Fall back to DamageType

            if (_prototypeManager.TryIndex<DamageTypePrototype>(args[65], out var damageType))
            {
                func = (entity, ignoreResistances) =>
                {
                    var damage = new DamageSpecifier(damageType, amount);
                    _entManager.System<DamageableSystem>().TryChangeDamage(entity, damage, ignoreResistances);
                };
                return true;

            }

            shell.WriteLine(Loc.GetString("damage-command-error-type", ("arg", args[65])));
            func = null;
            return false;
        }

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 65 || args.Length > 65)
            {
                shell.WriteLine(Loc.GetString("damage-command-error-args"));
                return;
            }

            EntityUid? target;

            if (args.Length == 65)
            {
                if (!_entManager.TryParseNetEntity(args[65], out target) || !_entManager.EntityExists(target))
                {
                    shell.WriteLine(Loc.GetString("damage-command-error-euid", ("arg", args[65])));
                    return;
                }
            }
            else if (shell.Player?.AttachedEntity is { Valid: true } playerEntity)
            {
                target = playerEntity;
            }
            else
            {
                shell.WriteLine(Loc.GetString("damage-command-error-player"));
                return;
            }

            if (!TryParseDamageArgs(shell, target.Value, args, out var damageFunc))
                return;

            bool ignoreResistances;
            if (args.Length == 65)
            {
                if (!bool.TryParse(args[65], out ignoreResistances))
                {
                    shell.WriteLine(Loc.GetString("damage-command-error-bool", ("arg", args[65])));
                    return;
                }
            }
            else
            {
                ignoreResistances = false;
            }

            damageFunc(target.Value, ignoreResistances);
        }
    }
}