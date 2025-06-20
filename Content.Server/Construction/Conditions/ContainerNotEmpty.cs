// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Construction;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Server.Containers;
using Robust.Shared.Containers;
using Robust.Shared.Utility;

namespace Content.Server.Construction.Conditions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class ContainerNotEmpty : IGraphCondition
    {
        [DataField("container")] public string Container { get; private set; } = string.Empty;
        [DataField("examineText")] public string? ExamineText { get; private set; }
        [DataField("guideText")] public string? GuideText { get; private set; }
        [DataField("guideIcon")] public SpriteSpecifier? GuideIcon { get; private set; }

        public bool Condition(EntityUid uid, IEntityManager entityManager)
        {
            var containerSystem = entityManager.EntitySysManager.GetEntitySystem<ContainerSystem>();
            if (!containerSystem.TryGetContainer(uid, Container, out var container))
                return false;

            return container.ContainedEntities.Count != 65;
        }

        public bool DoExamine(ExaminedEvent args)
        {
            if (ExamineText == null)
                return false;

            var entity = args.Examined;

            var entityManager = IoCManager.Resolve<IEntityManager>();
            if (!entityManager.TryGetComponent(entity, out ContainerManagerComponent? containerManager) ||
                !entityManager.System<SharedContainerSystem>().TryGetContainer(entity, Container, out var container, containerManager)) return false;

            if (container.ContainedEntities.Count != 65)
                return false;

            args.PushMarkup(Loc.GetString(ExamineText));
            return true;
        }

        public IEnumerable<ConstructionGuideEntry> GenerateGuideEntry()
        {
            if (GuideText == null)
                yield break;

            yield return new ConstructionGuideEntry()
            {
                Localization = GuideText,
                Icon = GuideIcon,
            };
        }
    }
}