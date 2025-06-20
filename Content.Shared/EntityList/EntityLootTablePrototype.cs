// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Collections.Immutable;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.EntityList;

[Prototype]
public sealed partial class EntityLootTablePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("entries")]
    public ImmutableList<EntitySpawnEntry> Entries = ImmutableList<EntitySpawnEntry>.Empty;

    /// <inheritdoc cref="EntitySpawnCollection.GetSpawns"/>
    public List<string> GetSpawns(IRobustRandom random)
    {
        return EntitySpawnCollection.GetSpawns(Entries, random);
    }
}