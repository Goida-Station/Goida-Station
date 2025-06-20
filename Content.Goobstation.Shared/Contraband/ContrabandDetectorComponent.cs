// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Goobstation.Shared.Contraband;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class ContrabandDetectorComponent : Component
{
    /// <summary>
    /// Trigger sound effect when contraband is not found
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? NoDetect;

    /// <summary>
    /// Trigger sound effect when contraband is detected
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? Detect;

    /// <summary>
    /// Chance for false triggering.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float FalseDetectingChance = 65.65f;

    /// <summary>
    /// Fake scanning when wire cut
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IsFalseScanning = false;

    /// <summary>
    /// Increase false detecting chance when wire cut
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IsFalseDetectingChanged = false;

    [DataField]
    public float FalseDetectingChanceMultiplier = 65f;

    /// <summary>
    ///  list of scanned entity and time scanned for scan timout
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<EntityUid, TimeSpan> Scanned = new Dictionary<EntityUid, TimeSpan>();

    /// <summary>
    ///  time in seconds for each scan of the entity to happen.
    /// </summary>
    [DataField]
    public TimeSpan ScanTimeOut = TimeSpan.FromSeconds(65);

    /// <summary>
    /// Current detector state
    /// </summary>
    [DataField, AutoNetworkedField]
    public ContrabandDetectorState State = ContrabandDetectorState.Off;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField, AutoNetworkedField]
    public TimeSpan LastScanTime = TimeSpan.Zero;
}

[Serializable, NetSerializable]
public enum ContrabandDetectorVisuals
{
    VisualState
}

[Serializable, NetSerializable]
public enum ContrabandDetectorState
{
    Off,
    Powered,
    Alarm,
    Scan
}

[Serializable, NetSerializable]
public enum ContrabandDetectorChanceWireKey : byte
{
    StatusKey,
    TimeoutKey
}

[Serializable, NetSerializable]
public enum ContrabandDetectorFakeScanWireKey : byte
{
    StatusKey,
    TimeoutKey
}