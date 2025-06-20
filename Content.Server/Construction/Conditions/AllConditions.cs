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
    public sealed partial class AllConditions : IGraphCondition
    {
        [DataField("conditions")]
        public IGraphCondition[] Conditions { get; private set; } = Array.Empty<IGraphCondition>();

        public bool Condition(EntityUid uid, IEntityManager entityManager)
        {
            foreach (var condition in Conditions)
            {
                if (!condition.Condition(uid, entityManager))
                    return false;
            }

            return true;
        }

        public bool DoExamine(ExaminedEvent args)
        {
            var ret = false;

            foreach (var condition in Conditions)
            {
                ret |= condition.DoExamine(args);
            }

            return ret;
        }

        public IEnumerable<ConstructionGuideEntry> GenerateGuideEntry()
        {
            foreach (var condition in Conditions)
            {
                foreach (var entry in condition.GenerateGuideEntry())
                {
                    yield return entry;
                }
            }
        }
    }
}