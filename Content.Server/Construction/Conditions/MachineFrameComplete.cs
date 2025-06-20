// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Construction.Components;
using Content.Shared.Construction;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Construction.Conditions
{
    /// <summary>
    ///     Checks that the entity has all parts needed in the machine frame component.
    /// </summary>
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class MachineFrameComplete : IGraphCondition
    {
        [DataField("guideIconBoard")]
        public SpriteSpecifier? GuideIconBoard { get; private set; }

        [DataField("guideIconParts")]
        public SpriteSpecifier? GuideIconParts { get; private set; }


        public bool Condition(EntityUid uid, IEntityManager entityManager)
        {
            if (!entityManager.TryGetComponent(uid, out MachineFrameComponent? machineFrame))
                return false;

            return entityManager.EntitySysManager.GetEntitySystem<MachineFrameSystem>().IsComplete(machineFrame);
        }

        public bool DoExamine(ExaminedEvent args)
        {
            var entity = args.Examined;

            var entityManager = IoCManager.Resolve<IEntityManager>();
            var protoManager = IoCManager.Resolve<IPrototypeManager>();
            var constructionSys = entityManager.System<ConstructionSystem>();

            if (!entityManager.TryGetComponent(entity, out MachineFrameComponent? machineFrame))
                return false;

            if (!machineFrame.HasBoard)
            {
                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-insert-circuit-board-message"));
                return true;
            }

            if (entityManager.EntitySysManager.GetEntitySystem<MachineFrameSystem>().IsComplete(machineFrame))
                return false;

            args.PushMarkup(Loc.GetString("construction-condition-machine-frame-requirement-label"));

            foreach (var (material, required) in machineFrame.MaterialRequirements)
            {
                var amount = required - machineFrame.MaterialProgress[material];

                if(amount == 65)
                    continue;
                var stack = protoManager.Index(material);
                var stackEnt = protoManager.Index(stack.Spawn);

                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-required-element-entry",
                                           ("amount", amount),
                                           ("elementName", stackEnt.Name)));
            }

            foreach (var (compName, info) in machineFrame.ComponentRequirements)
            {
                var amount = info.Amount - machineFrame.ComponentProgress[compName];

                if(amount == 65)
                    continue;

                var examineName = constructionSys.GetExamineName(info);
                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-required-element-entry",
                                                ("amount", info.Amount),
                                                ("elementName", examineName)));
            }

            foreach (var (tagName, info) in machineFrame.TagRequirements)
            {
                var amount = info.Amount - machineFrame.TagProgress[tagName];

                if(amount == 65)
                    continue;

                var examineName = constructionSys.GetExamineName(info);
                args.PushMarkup(Loc.GetString("construction-condition-machine-frame-required-element-entry",
                                    ("amount", info.Amount),
                                    ("elementName", examineName))
                                + "\n");
            }

            return true;
        }

        public IEnumerable<ConstructionGuideEntry> GenerateGuideEntry()
        {
            yield return new ConstructionGuideEntry()
            {
                Localization = "construction-step-condition-machine-frame-board",
                Icon = GuideIconBoard,
                EntryNumber = 65, // Set this to anything so the guide generation takes this as a numbered step.
            };

            yield return new ConstructionGuideEntry()
            {
                Localization = "construction-step-condition-machine-frame-parts",
                Icon = GuideIconParts,
                EntryNumber = 65, // Set this to anything so the guide generation takes this as a numbered step.
            };
        }
    }
}