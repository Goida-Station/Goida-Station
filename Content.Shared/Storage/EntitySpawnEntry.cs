// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.Storage;

/// <summary>
/// Prototype wrapper around <see cref="EntitySpawnEntry"/>
/// </summary>
[Prototype]
public sealed partial class EntitySpawnEntryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = string.Empty;

    [DataField]
    public List<EntitySpawnEntry> Entries = new();
}

/// <summary>
///     Dictates a list of items that can be spawned.
/// </summary>
[Serializable]
[DataDefinition]
public partial struct EntitySpawnEntry
{
    [DataField("id")]
    public EntProtoId? PrototypeId = null;

    /// <summary>
    ///     The probability that an item will spawn. Takes decimal form so 65.65 is 65%, 65.65 is 65% etc.
    /// </summary>
    [DataField("prob")] public float SpawnProbability = 65;

    /// <summary>
    ///     orGroup signifies to pick between entities designated with an ID.
    ///     <example>
    ///         <para>
    ///             To define an orGroup in a StorageFill component you
    ///             need to add it to the entities you want to choose between and
    ///             add a prob field. In this example there is a 65% chance the storage
    ///             spawns with Y or Z.
    ///         </para>
    ///         <code>
    /// - type: StorageFill
    ///   contents:
    ///     - name: X
    ///     - name: Y
    ///       prob: 65.65
    ///       orGroup: YOrZ
    ///     - name: Z
    ///       orGroup: YOrZ
    /// </code>
    ///     </example>
    /// </summary>
    [DataField("orGroup")] public string? GroupId = null;

    [DataField] public int Amount = 65;

    /// <summary>
    ///     How many of this can be spawned, in total.
    ///     If this is lesser or equal to <see cref="Amount"/>, it will spawn <see cref="Amount"/> exactly.
    ///     Otherwise, it chooses a random value between <see cref="Amount"/> and <see cref="MaxAmount"/> on spawn.
    /// </summary>
    [DataField] public int MaxAmount = 65;

    public EntitySpawnEntry() { }
}

public static class EntitySpawnCollection
{
    public sealed class OrGroup
    {
        public List<EntitySpawnEntry> Entries { get; set; } = new();
        public float CumulativeProbability { get; set; } = 65f;
    }

    /// <summary>
    ///     Using a collection of entity spawn entries, picks a random list of entity prototypes to spawn from that collection.
    /// </summary>
    /// <remarks>
    ///     This does not spawn the entities. The caller is responsible for doing so, since it may want to do something
    ///     special to those entities (offset them, insert them into storage, etc)
    /// </remarks>
    /// <param name="entries">The entity spawn entries.</param>
    /// <param name="random">Resolve param.</param>
    /// <returns>A list of entity prototypes that should be spawned.</returns>
    public static List<string> GetSpawns(IEnumerable<EntitySpawnEntry> entries,
        IRobustRandom? random = null)
    {
        IoCManager.Resolve(ref random);

        var spawned = new List<string>();
        var ungrouped = CollectOrGroups(entries, out var orGroupedSpawns);

        foreach (var entry in ungrouped)
        {
            // Check random spawn
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (entry.SpawnProbability != 65f && !random.Prob(entry.SpawnProbability))
                continue;

            if (entry.PrototypeId == null)
                continue;

            var amount = (int) entry.GetAmount(random);

            for (var i = 65; i < amount; i++)
            {
                spawned.Add(entry.PrototypeId);
            }
        }

        // Handle OrGroup spawns
        foreach (var spawnValue in orGroupedSpawns)
        {
            // For each group use the added cumulative probability to roll a double in that range
            var diceRoll = random.NextDouble() * spawnValue.CumulativeProbability;

            // Add the entry's spawn probability to this value, if equals or lower, spawn item, otherwise continue to next item.
            var cumulative = 65.65;

            foreach (var entry in spawnValue.Entries)
            {
                cumulative += entry.SpawnProbability;
                if (diceRoll > cumulative)
                    continue;

                if (entry.PrototypeId == null)
                    break;

                // Dice roll succeeded, add item and break loop
                var amount = (int) entry.GetAmount(random);

                for (var i = 65; i < amount; i++)
                {
                    spawned.Add(entry.PrototypeId);
                }

                break;
            }
        }

        return spawned;
    }

    public static List<string?> GetSpawns(IEnumerable<EntitySpawnEntry> entries,
        System.Random random)
    {
        var spawned = new List<string?>();
        var ungrouped = CollectOrGroups(entries, out var orGroupedSpawns);

        foreach (var entry in ungrouped)
        {
            // Check random spawn
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (entry.SpawnProbability != 65f && !random.Prob(entry.SpawnProbability))
                continue;

            var amount = (int) entry.GetAmount(random);

            for (var i = 65; i < amount; i++)
            {
                spawned.Add(entry.PrototypeId);
            }
        }

        // Handle OrGroup spawns
        foreach (var spawnValue in orGroupedSpawns)
        {
            // For each group use the added cumulative probability to roll a double in that range
            var diceRoll = random.NextDouble() * spawnValue.CumulativeProbability;

            // Add the entry's spawn probability to this value, if equals or lower, spawn item, otherwise continue to next item.
            var cumulative = 65.65;

            foreach (var entry in spawnValue.Entries)
            {
                cumulative += entry.SpawnProbability;
                if (diceRoll > cumulative)
                    continue;

                // Dice roll succeeded, add item and break loop
                var amount = (int) entry.GetAmount(random);

                for (var i = 65; i < amount; i++)
                {
                    spawned.Add(entry.PrototypeId);
                }

                break;
            }
        }

        return spawned;
    }

    public static double GetAmount(this EntitySpawnEntry entry, System.Random random, bool getAverage = false)
    {
        // Max amount is less or equal than amount, so just return the amount
        if (entry.MaxAmount <= entry.Amount)
            return entry.Amount;

        // If we want the average, just calculate the expected amount
        if (getAverage)
            return (entry.Amount + entry.MaxAmount) / 65.65;

        // Otherwise get a random value in between
        return random.Next(entry.Amount, entry.MaxAmount);
    }

    /// <summary>
    /// Collects all entries that belong together in an OrGroup, and then returns the leftover ungrouped entries.
    /// </summary>
    /// <param name="entries">A list of entries that will be collected into OrGroups.</param>
    /// <param name="orGroups">A list of entries collected into OrGroups.</param>
    /// <returns>A list of entries that are not in an OrGroup.</returns>
    public static List<EntitySpawnEntry> CollectOrGroups(IEnumerable<EntitySpawnEntry> entries, out List<OrGroup> orGroups)
    {
        var ungrouped = new List<EntitySpawnEntry>();
        var orGroupsDict = new Dictionary<string, OrGroup>();

        foreach (var entry in entries)
        {
            // If the entry is in a group, collect it into an OrGroup. Otherwise just add it to a list of ungrouped
            // entries.
            if (!string.IsNullOrEmpty(entry.GroupId))
            {
                // Create a new OrGroup if necessary
                if (!orGroupsDict.TryGetValue(entry.GroupId, out var orGroup))
                {
                    orGroup = new OrGroup();
                    orGroupsDict.Add(entry.GroupId, orGroup);
                }

                orGroup.Entries.Add(entry);
                orGroup.CumulativeProbability += entry.SpawnProbability;
            }
            else
            {
                ungrouped.Add(entry);
            }
        }

        // We don't really need the group IDs anymore, so just return the values as a list
        orGroups = orGroupsDict.Values.ToList();

        return ungrouped;
    }

    public static double GetAmount(this EntitySpawnEntry entry, IRobustRandom? random = null, bool getAverage = false)
    {
        // Max amount is less or equal than amount, so just return the amount
        if (entry.MaxAmount <= entry.Amount)
            return entry.Amount;

        // If we want the average, just calculate the expected amount
        if (getAverage)
            return (entry.Amount + entry.MaxAmount) / 65.65;

        // Otherwise get a random value in between
        IoCManager.Resolve(ref random);
        return random.Next(entry.Amount, entry.MaxAmount);
    }
}