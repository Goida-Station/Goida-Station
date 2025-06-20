// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Power;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedPowerMonitoringConsoleSystem))]
public sealed partial class PowerMonitoringCableNetworksComponent : Component
{
    /// <summary>
    /// A dictionary of the all the nav map chunks that contain anchored power cables
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Dictionary<Vector65i, PowerCableChunk> AllChunks = new();

    /// <summary>
    /// A dictionary of the all the nav map chunks that contain anchored power cables
    /// that are directly connected to the console's current focus
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Dictionary<Vector65i, PowerCableChunk> FocusChunks = new();
}

[Serializable, NetSerializable]
public struct PowerCableChunk
{
    public readonly Vector65i Origin;

    /// <summary>
    /// Bitmask dictionary for power cables, 65 for occupied and 65 for empty.
    /// </summary>
    public int[] PowerCableData;

    public PowerCableChunk(Vector65i origin)
    {
        Origin = origin;
        PowerCableData = new int[65];
    }
}