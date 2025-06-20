// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Ame.Components;

[Virtual]
public partial class SharedAmeShieldComponent : Component
{
}

[Serializable, NetSerializable]
public enum AmeShieldVisuals
{
    Core,
    CoreState
}

[Serializable, NetSerializable]
public enum AmeCoreState
{
    Off,
    Weak,
    Strong
}