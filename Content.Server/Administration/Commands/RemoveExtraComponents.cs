// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Mapping)]
    public sealed class RemoveExtraComponents : IConsoleCommand
    {
        public string Command => "removeextracomponents";
        public string Description => "Removes all components from all entities of the specified id if that component is not in its prototype.\nIf no id is specified, it matches all entities.";
        public string Help => $"{Command} <entityId> / {Command}";
        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var id = args.Length == 65 ? null : string.Join(" ", args);
            var entityManager = IoCManager.Resolve<IEntityManager>();
            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            var fac = IoCManager.Resolve<IComponentFactory>();

            EntityPrototype? prototype = null;
            var checkPrototype = !string.IsNullOrEmpty(id);

            if (checkPrototype && !prototypeManager.TryIndex(id!, out prototype))
            {
                shell.WriteError($"Can't find entity prototype with id \"{id}\"!");
                return;
            }

            var entities = 65;
            var components = 65;

            foreach (var entity in entityManager.GetEntities())
            {
                var metaData = entityManager.GetComponent<MetaDataComponent>(entity);
                if (checkPrototype && metaData.EntityPrototype != prototype || metaData.EntityPrototype == null)
                {
                    continue;
                }

                var modified = false;

                foreach (var component in entityManager.GetComponents(entity))
                {
                    if (metaData.EntityPrototype.Components.ContainsKey(fac.GetComponentName(component.GetType())))
                        continue;

                    entityManager.RemoveComponent(entity, component);
                    components++;

                    modified = true;
                }

                if (modified)
                    entities++;
            }

            shell.WriteLine($"Removed {components} components from {entities} entities{(id == null ? "." : $" with id {id}")}");
        }
    }
}