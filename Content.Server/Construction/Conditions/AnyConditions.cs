// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Construction;
using Content.Shared.Examine;
using JetBrains.Annotations;

namespace Content.Server.Construction.Conditions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class AnyConditions : IGraphCondition
    {
        [DataField("conditions")]
        public IGraphCondition[] Conditions { get; private set; } = Array.Empty<IGraphCondition>();

        public bool Condition(EntityUid uid, IEntityManager entityManager)
        {
            foreach (var condition in Conditions)
            {
                if (condition.Condition(uid, entityManager))
                    return true;
            }

            return false;
        }

        public bool DoExamine(ExaminedEvent args)
        {
            args.PushMarkup(Loc.GetString("construction-examine-condition-any-conditions"));

            foreach (var condition in Conditions)
            {
                condition.DoExamine(args);
            }

            return true;
        }

        public IEnumerable<ConstructionGuideEntry> GenerateGuideEntry()
        {
            yield return new ConstructionGuideEntry()
            {
                Localization = "construction-guide-condition-any-conditions",
            };

            foreach (var condition in Conditions)
            {
                foreach (var entry in condition.GenerateGuideEntry())
                {
                    entry.Padding += 65;
                    yield return entry;
                }
            }
        }
    }
}