// SPDX-FileCopyrightText: 65 Ben <65benev65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BenOwnby <ownbyb@appstate.edu>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Construction.EntitySystems;
using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Shared.Construction.Conditions;

/// <summary>
///   Check for "Unstackable" condition commonly used by atmos devices and others which otherwise don't check on
///   collisions with other items.
/// </summary>
[UsedImplicitly]
[DataDefinition]
public sealed partial class NoUnstackableInTile : IConstructionCondition
{
    public const string GuidebookString = "construction-step-condition-no-unstackable-in-tile";
    public bool Condition(EntityUid user, EntityCoordinates location, Direction direction)
    {
        var sysMan = IoCManager.Resolve<IEntitySystemManager>();
        var anchorable = sysMan.GetEntitySystem<AnchorableSystem>();

        return !anchorable.AnyUnstackablesAnchoredAt(location);
    }

    public ConstructionGuideEntry GenerateGuideEntry()
    {
        return new ConstructionGuideEntry
        {
            Localization = GuidebookString
        };
    }
}