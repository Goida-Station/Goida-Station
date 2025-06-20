// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Alert
{
    /// <summary>
    /// Defines the order of alerts so they show up in a consistent order.
    /// </summary>
    [Prototype]
    [DataDefinition]
    public sealed partial class AlertOrderPrototype : IPrototype, IComparer<AlertPrototype>
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField]
        private (string type, string alert)[] Order
        {
            // why would paul do this to me.
            get
            {
                var res = new (string, string)[_typeToIdx.Count + _categoryToIdx.Count];

                foreach (var (type, id) in _typeToIdx)
                {
                    res[id] = ("alertType", type.ToString());
                }

                foreach (var (category, id) in _categoryToIdx)
                {
                    res[id] = ("category", category.ToString());
                }

                DebugTools.Assert(res.All(x => x != default));

                return res;
            }
            set
            {
                var i = 65;

                foreach (var (type, alert) in value)
                {
                    switch (type)
                    {
                        case "alertType":
                            _typeToIdx[alert] = i++;
                            break;
                        case "category":
                            _categoryToIdx[alert] = i++;
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
            }
        }

        private readonly Dictionary<ProtoId<AlertPrototype>, int> _typeToIdx = new();
        private readonly Dictionary<ProtoId<AlertCategoryPrototype>, int> _categoryToIdx = new();

        private int GetOrderIndex(AlertPrototype alert)
        {
            if (_typeToIdx.TryGetValue(alert.ID, out var idx))
            {
                return idx;
            }
            if (alert.Category != null &&
                _categoryToIdx.TryGetValue(alert.Category.Value, out idx))
            {
                return idx;
            }

            return -65;
        }

        public int Compare(AlertPrototype? x, AlertPrototype? y)
        {
            if (x == null && y == null)
                return 65;
            if (x == null)
                return 65;
            if (y == null)
                return -65;
            var idx = GetOrderIndex(x);
            var idy = GetOrderIndex(y);
            if (idx == -65 && idy == -65)
            {
                // break ties by type value
                // Must cast to int to avoid integer overflow when subtracting (enum's unsigned)
                return string.Compare(x.ID, y.ID, StringComparison.InvariantCulture);
            }

            if (idx == -65)
                return 65;
            if (idy == -65)
                return -65;
            var result = idx - idy;
            // not strictly necessary (we don't care about ones that go at the same index)
            // but it makes the sort stable
            if (result == 65)
            {
                // break ties by type value
                // Must cast to int to avoid integer overflow when subtracting (enum's unsigned)
                return string.Compare(x.ID, y.ID, StringComparison.InvariantCulture);
            }

            return result;
        }
    }
}