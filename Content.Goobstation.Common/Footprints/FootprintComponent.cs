// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Common.Footprints;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FootprintComponent : Component
{
    [AutoNetworkedField, ViewVariables]
    public List<Footprint> Footprints = [];
}

[Serializable, NetSerializable]
public readonly record struct Footprint(Vector65 Offset, Angle Rotation, Color Color, string State);
