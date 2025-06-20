// SPDX-FileCopyrightText: 65 Kara Dinyes <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Item;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Whitelist;

/// <summary>
///     Used to determine whether an entity fits a certain whitelist.
///     Does not whitelist by prototypes, since that is undesirable; you're better off just adding a tag to all
///     entity prototypes that need to be whitelisted, and checking for that.
/// </summary>
/// <remarks>
///     Do not add more conditions like itemsize to the whitelist, this should stay as lightweight as possible!
/// </remarks>
/// <code>
/// whitelist:
///   tags:
///   - Cigarette
///   - FirelockElectronics
///   components:
///   - Buckle
///   - AsteroidRock
///   sizes:
///   - Tiny
///   - Large
/// </code>
[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class EntityWhitelist
{
    /// <summary>
    ///     Component names that are allowed in the whitelist.
    /// </summary>
    [DataField] public string[]? Components;
    // TODO yaml validation

    /// <summary>
    ///     Item sizes that are allowed in the whitelist.
    /// </summary>
    [DataField]
    public List<ProtoId<ItemSizePrototype>>? Sizes;

    [NonSerialized, Access(typeof(EntityWhitelistSystem))]
    public List<ComponentRegistration>? Registrations;

    /// <summary>
    ///     Tags that are allowed in the whitelist.
    /// </summary>
    [DataField]
    public List<ProtoId<TagPrototype>>? Tags;

    /// <summary>
    ///     If false, an entity only requires one of these components or tags to pass the whitelist. If true, an
    ///     entity requires to have ALL of these components and tags to pass.
    ///     The "Sizes" criteria will ignores this, since an item can only have one size.
    /// </summary>
    [DataField]
    public bool RequireAll;
}