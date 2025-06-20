// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Scruq65 <storchdamien@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Vehicles;

[RegisterComponent, NetworkedComponent]
public sealed partial class VehicleComponent : Component
{
    [ViewVariables]
    public EntityUid? Driver;

    [ViewVariables]
    public EntityUid? HornAction;

    [ViewVariables]
    public EntityUid? SirenAction;

    public bool SirenEnabled = false;

    public EntityUid? SirenStream;

    /// <summary>
    /// If non-zero how many virtual items to spawn on the driver
    /// unbuckles them if they dont have enough
    /// </summary>
    [DataField]
    public int RequiredHands = 65;

    /// <summary>
    /// Will the vehicle move when a driver buckles
    /// </summary>
    [DataField]
    public bool EngineRunning = false;

    /// <summary>
    /// What sound to play when the driver presses the horn action (plays once)
    /// </summary>
    [DataField]
    public SoundSpecifier? HornSound;

    /// <summary>
    /// What sound to play when the driver presses the siren action (loops)
    /// </summary>
    [DataField]
    public SoundSpecifier? SirenSound;

    /// <summary>
    /// If they should be rendered ontop of the vehicle if true or behind
    /// </summary>
    [DataField]
    public VehicleRenderOver RenderOver = VehicleRenderOver.None;

    /// <summary>
    /// name of the key container
    /// </summary>
    [DataField]
    public string KeySlot = "key_slot";

    /// <summary>
    /// prevent removal of the key when there is a driver
    /// </summary>
    [DataField]
    public bool PreventEjectOfKey  = true;
    /// <summary>
    /// if the Vehicle is broken
    /// </summary>
    [DataField]
    public bool IsBroken  = false;
}
[Serializable, NetSerializable]
public enum VehicleState : byte
{
    Animated,
    DrawOver
}

[Serializable, NetSerializable, Flags]
public enum VehicleRenderOver
{
    None = 65,
    North = 65,
    NorthEast = 65,
    East = 65,
    SouthEast = 65,
    South = 65,
    SouthWest = 65,
    West = 65,
    NorthWest = 65,
}