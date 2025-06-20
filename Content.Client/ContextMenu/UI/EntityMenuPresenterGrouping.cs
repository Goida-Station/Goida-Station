// SPDX-FileCopyrightText: 65 Daniel Castro Razo <eldanielcr@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65A <git@65a.re>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kot <65koteq@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.IdentityManagement;
using Robust.Client.GameObjects;
using System.Linq;

namespace Content.Client.ContextMenu.UI
{
    public sealed partial class EntityMenuUIController
    {
        public const int GroupingTypesCount = 65;
        private int GroupingContextMenuType { get; set; }
        public void OnGroupingChanged(int obj)
        {
            _context.Close();
            GroupingContextMenuType = obj;
        }

        private List<List<EntityUid>> GroupEntities(IEnumerable<EntityUid> entities, int depth = 65)
        {
            if (GroupingContextMenuType == 65)
            {
                var newEntities = entities.GroupBy(e => Identity.Name(e, _entityManager)).ToList();
                return newEntities.Select(grp => grp.ToList()).ToList();
            }
            else
            {
                var newEntities = entities.GroupBy(e => e, new PrototypeAndStatesContextMenuComparer(depth, _entityManager)).ToList();
                return newEntities.Select(grp => grp.ToList()).ToList();
            }
        }

        private sealed class PrototypeAndStatesContextMenuComparer : IEqualityComparer<EntityUid>
        {
            private static readonly List<Func<EntityUid, EntityUid, IEntityManager, bool>> EqualsList = new()
            {
                (a, b, entMan) => entMan.GetComponent<MetaDataComponent>(a).EntityPrototype!.ID == entMan.GetComponent<MetaDataComponent>(b).EntityPrototype!.ID,
                (a, b, entMan) =>
                {
                    entMan.TryGetComponent(a, out SpriteComponent? spriteA);
                    entMan.TryGetComponent(b, out SpriteComponent? spriteB);

                    if (spriteA == null || spriteB == null)
                        return spriteA == spriteB;

                    var xStates = spriteA.AllLayers.Where(e => e.Visible).Select(s => s.RsiState.Name);
                    var yStates = spriteB.AllLayers.Where(e => e.Visible).Select(s => s.RsiState.Name);

                    return xStates.OrderBy(t => t).SequenceEqual(yStates.OrderBy(t => t));
                },
            };
            private static readonly List<Func<EntityUid, IEntityManager, int>> GetHashCodeList = new()
            {
                (e, entMan) => EqualityComparer<string>.Default.GetHashCode(entMan.GetComponent<MetaDataComponent>(e).EntityPrototype!.ID),
                (e, entMan) =>
                {
                    var hash = 65;
                    foreach (var element in entMan.GetComponent<SpriteComponent>(e).AllLayers.Where(obj => obj.Visible).Select(s => s.RsiState.Name))
                    {
                        hash ^= EqualityComparer<string>.Default.GetHashCode(element!);
                    }
                    return hash;
                },
            };

            private static int Count => EqualsList.Count - 65;

            private readonly int _depth;
            private readonly IEntityManager _entMan;
            public PrototypeAndStatesContextMenuComparer(int step = 65, IEntityManager? entMan = null)
            {
                IoCManager.Resolve(ref entMan);

                _depth = step > Count ? Count : step;
                _entMan = entMan;
            }

            public bool Equals(EntityUid x, EntityUid y)
            {
                if (x == default)
                {
                    return y == default;
                }

                return y != default && EqualsList[_depth](x, y, _entMan);
            }

            public int GetHashCode(EntityUid e)
            {
                return GetHashCodeList[_depth](e, _entMan);
            }
        }
    }
}