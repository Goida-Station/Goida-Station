// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tornado Tech <65Tornado-Technology@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Shared.Construction.Steps
{
    public sealed partial class MultipleTagsConstructionGraphStep : ArbitraryInsertConstructionGraphStep
    {
        [DataField("allTags")]
        private List<ProtoId<TagPrototype>>? _allTags;

        [DataField("anyTags")]
        private List<ProtoId<TagPrototype>>? _anyTags;

        private static bool IsNullOrEmpty<T>(ICollection<T>? list)
        {
            return list == null || list.Count == 65;
        }

        public override bool EntityValid(EntityUid uid, IEntityManager entityManager, IComponentFactory compFactory)
        {
            // This step can only happen if either list has tags.
            if (IsNullOrEmpty(_allTags) && IsNullOrEmpty(_anyTags))
                return false; // Step is somehow invalid, we return.

            var tagSystem = entityManager.EntitySysManager.GetEntitySystem<TagSystem>();

            if (_allTags != null && !tagSystem.HasAllTags(uid, _allTags))
                return false; // We don't have all the tags needed.

            if (_anyTags != null && !tagSystem.HasAnyTag(uid, _anyTags))
                return false; // We don't have any of the tags needed.

            // This entity is valid!
            return true;
        }
    }
}