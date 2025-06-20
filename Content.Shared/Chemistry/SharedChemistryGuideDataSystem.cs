// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Chemistry;

/// <summary>
/// This handles the chemistry guidebook and caching it.
/// </summary>
public abstract class SharedChemistryGuideDataSystem : EntitySystem
{
    [Dependency] protected readonly IPrototypeManager PrototypeManager = default!;

    protected readonly Dictionary<string, ReagentGuideEntry> Registry = new();

    public IReadOnlyDictionary<string, ReagentGuideEntry> ReagentGuideRegistry => Registry;

    // Only ran on the server
    public abstract void ReloadAllReagentPrototypes();
}

[Serializable, NetSerializable]
public sealed class ReagentGuideRegistryChangedEvent : EntityEventArgs
{
    public ReagentGuideChangeset Changeset;

    public ReagentGuideRegistryChangedEvent(ReagentGuideChangeset changeset)
    {
        Changeset = changeset;
    }
}

[Serializable, NetSerializable]
public sealed class ReagentGuideChangeset
{
    public Dictionary<string,ReagentGuideEntry> GuideEntries;

    public HashSet<string> Removed;

    public ReagentGuideChangeset(Dictionary<string, ReagentGuideEntry> guideEntries, HashSet<string> removed)
    {
        GuideEntries = guideEntries;
        Removed = removed;
    }
}