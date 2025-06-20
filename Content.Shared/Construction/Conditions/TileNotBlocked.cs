// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Maps;
using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Shared.Construction.Conditions;

[UsedImplicitly]
[DataDefinition]
public sealed partial class TileNotBlocked : IConstructionCondition
{
    [DataField("filterMobs")] private bool _filterMobs = false;
    [DataField("failIfSpace")] private bool _failIfSpace = true;
    [DataField("failIfNotSturdy")] private bool _failIfNotSturdy = true;

    public bool Condition(EntityUid user, EntityCoordinates location, Direction direction)
    {
        var tileRef = location.GetTileRef();

        if (tileRef == null)
        {
            return false;
        }

        if (tileRef.Value.IsSpace() && _failIfSpace)
        {
            return false;
        }

        if (!tileRef.Value.GetContentTileDefinition().Sturdy && _failIfNotSturdy)
        {
            return false;
        }

        return !tileRef.Value.IsBlockedTurf(_filterMobs);
    }

    public ConstructionGuideEntry GenerateGuideEntry()
    {
        return new ConstructionGuideEntry
        {
            Localization = "construction-step-condition-tile-not-blocked",
        };
    }
}