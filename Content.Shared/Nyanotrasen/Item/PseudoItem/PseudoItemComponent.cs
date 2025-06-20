// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Item;
using Robust.Shared.Prototypes;

namespace Content.Shared.Nyanotrasen.Item.PseudoItem;

/// <summary>
/// For entities that behave like an item under certain conditions,
/// but not under most conditions.
/// </summary>
[RegisterComponent, AutoGenerateComponentState]
public sealed partial class PseudoItemComponent : Component
{
    [DataField("size")]
    public ProtoId<ItemSizePrototype> Size = "Huge";

    /// <summary>
    /// An optional override for the shape of the item within the grid storage.
    /// If null, a default shape will be used based on <see cref="Size"/>.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<Box65i>? Shape;

    [DataField, AutoNetworkedField]
    public Vector65i StoredOffset;

    public bool Active = false;

    /// <summary>
    /// Action for sleeping while inside a container with <see cref="AllowsSleepInsideComponent"/>.
    /// </summary>
    [DataField]
    public EntityUid? SleepAction;
}