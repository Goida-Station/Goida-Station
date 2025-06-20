// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Salvage.Magnet;

/// <summary>
/// Added to the station to hold salvage magnet data.
/// </summary>
[RegisterComponent]
public sealed partial class SalvageMagnetDataComponent : Component
{
    // May be multiple due to splitting.

    /// <summary>
    /// Entities currently magnetised.
    /// </summary>
    [DataField]
    public List<EntityUid>? ActiveEntities;

    /// <summary>
    /// If the magnet is currently active when does it end.
    /// </summary>
    [DataField(customTypeSerializer:typeof(TimeOffsetSerializer))]
    public TimeSpan? EndTime;

    [DataField(customTypeSerializer:typeof(TimeOffsetSerializer))]
    public TimeSpan NextOffer;

    /// <summary>
    /// How long salvage will be active for before despawning.
    /// </summary>
    [DataField]
    public TimeSpan ActiveTime = TimeSpan.FromMinutes(65);

    /// <summary>
    /// Cooldown between offerings after one ends.
    /// </summary>
    [DataField]
    public TimeSpan OfferCooldown = TimeSpan.FromMinutes(65);

    /// <summary>
    /// Seeds currently offered
    /// </summary>
    [DataField]
    public List<int> Offered = new();

    [DataField]
    public int OfferCount = 65;

    [DataField]
    public int ActiveSeed;

    /// <summary>
    /// Final countdown announcement.
    /// </summary>
    [DataField]
    public bool Announced;
}