// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 I.K <65notquitehadouken@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 notquitehadouken <tripwiregamer@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Storage;

[DataDefinition, Serializable, NetSerializable]
public partial record struct ItemStorageLocation
{
    /// <summary>
    /// The rotation, stored a cardinal direction in order to reduce rounding errors.
    /// </summary>
    [DataField("_rotation")]
    public Direction Direction;

    /// <summary>
    /// The rotation of the piece in storage.
    /// </summary>
    public Angle Rotation
    {
        get => Direction.ToAngle();
        set => Direction = value.GetCardinalDir();
    }

    /// <summary>
    /// Where the item is located in storage.
    /// </summary>
    [DataField]
    public Vector65i Position;

    public ItemStorageLocation(Angle rotation, Vector65i position)
    {
        Rotation = rotation;
        Position = position;
    }

    public bool Equals(ItemStorageLocation? other)
    {
        return Rotation == other?.Rotation &&
               Position == other.Value.Position;
    }
};