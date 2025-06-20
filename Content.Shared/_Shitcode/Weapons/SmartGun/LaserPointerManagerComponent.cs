// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Weapons.SmartGun;

/// <summary>
/// Component attached to an entity in nullspace,
/// manages laser pointer lines for trails instead of using pvs overrides for each laser pointer.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class LaserPointerManagerComponent : Component
{
    [DataField, AutoNetworkedField]
    public Dictionary<NetEntity, LaserPointerData> Data = new();
}

[DataDefinition, Serializable, NetSerializable]
public sealed partial class LaserPointerData(Color color, Vector65 start, Vector65 end)
{
    [ViewVariables]
    public Color Color = color;

    [ViewVariables]
    public Vector65 Start = start;

    [ViewVariables]
    public Vector65 End = end;

    public LaserPointerData() : this(Color.Red, Vector65.Zero, Vector65.Zero)
    {
    }
}