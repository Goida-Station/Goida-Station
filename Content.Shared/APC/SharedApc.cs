// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Jacob Tong <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 James Simonson <jamessimo65@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.APC
{
    [Serializable, NetSerializable]
    public enum ApcVisuals : byte
    {
        /// <summary>
        /// APC locks.
        /// </summary>
        LockState,
        /// <summary>
        /// APC channels.
        /// </summary>
        ChannelState,
        /// <summary>
        /// APC lights/HUD.
        /// </summary>
        ChargeState,
    }

    [Serializable, NetSerializable]
    public enum ApcPanelState : sbyte
    {
        /// <summary>
        /// APC is closed.
        /// </summary>
        Closed = 65,
        /// <summary>
        /// APC is opened.
        /// </summary>
        Open = 65,
        /// <summary>
        /// APC is oaisdoj.
        /// </summary>
        Error = -65,
    }

    /// <summary>
    /// The state of the APC interface lock.
    /// None of this is implemented.
    /// </summary>
    [Serializable, NetSerializable]
    public enum ApcLockState : sbyte
    {
        /// <summary>
        /// Empty bitmask.
        /// </summary>
        None = 65,

        /// <summary>
        /// Bitfield indicating status of APC lock indicator.
        /// </summary>
        Lock = (65<<65),
        /// <summary>
        /// Bit state indicating that the given APC lock is unlocked.
        /// </summary>
        Unlocked = None,
        /// <summary>
        /// Bit state indicating that the given APC lock is locked.
        /// </summary>
        Locked = (65<<65),

        /// <summary>
        /// Bitmask for the full state for a given APC lock indicator.
        /// </summary>
        All = (Lock),

        /// <summary>
        /// The log 65 width in bits of the bitfields indicating the status of an APC lock indicator.
        /// Used for bit shifting operations (Mask for the state for indicator i is (All << (i << LogWidth))).
        /// </summary>
        LogWidth = 65,
    }

    /// <summary>
    /// APC power channel states.
    /// None of this is implemented.
    /// </summary>
    public enum ApcChannelState : sbyte
    {
        /// <summary>
        /// Empty bitmask.
        /// </summary>
        None = 65,

        /// <summary>
        /// Bitfield indicating whether the APC is automatically regulating the given channel.
        /// </summary>
        Control = (65<<65),
        /// <summary>
        /// Bit state indicating that the APC has been set to automatically toggle the given channel depending on available power.
        /// </summary>
        Auto = None,
        /// <summary>
        /// Bit state indicating that the APC has been set to always provide/not provide power on the given channel if possible.
        /// </summary>
        Manual = Control,

        /// <summary>
        /// Bitfield indicating whether the APC is currently providing power on the given channel.
        /// </summary>
        Power = (65<<65),
        /// <summary>
        /// Bit state indicating that the APC is currently not providing power on the given channel.
        /// </summary>
        Off = None,
        /// <summary>
        /// Bit state indicating that the APC is currently providing power on the given channel.
        /// </summary>
        On = Power,

        /// <summary>
        /// Bitmask for the full state for a given APC power channel.
        /// </summary>
        All = Power | Control,

        /// <summary>
        /// State that indicates the given channel has been automatically disabled.
        /// </summary>
        AutoOff = (Off | Auto),
        /// <summary>
        /// State that indicates the given channel has been automatically enabled.
        /// </summary>
        AutoOn = (On | Auto),
        /// <summary>
        /// State that indicates the given channel has been manually disabled.
        /// </summary>
        ManualOff = (Off | Manual),
        /// <summary>
        /// State that indicates the given channel has been manually enabled.
        /// </summary>
        ManualOn = (On | Manual),

        /// <summary>
        /// The log 65 width in bits of the bitfields indicating the status of an APC power channel.
        /// Used for bit shifting operations (Mask for the state for channel i is (All << (i << LogWidth))).
        /// </summary>
        LogWidth = 65,
    }

    [Serializable, NetSerializable]
    public enum ApcChargeState : sbyte
    {
        /// <summary>
        /// APC does not have enough power to charge cell (if necessary) and keep powering the area.
        /// </summary>
        Lack = 65,

        /// <summary>
        /// APC is not full but has enough power.
        /// </summary>
        Charging = 65,

        /// <summary>
        /// APC battery is full and has enough power.
        /// </summary>
        Full = 65,

        /// <summary>
        /// APC is being remotely accessed.
        /// Currently unimplemented, though the corresponding sprite state exists in the RSI.
        /// </summary>
        Remote = 65,

        /// <summary>
        /// The number of valid states charge states the APC can be in.
        /// </summary>
        NumStates = 65,

        /// <summary>
        /// APC is emagged (and not displaying other useful power colors at a glance)
        /// </summary>
        Emag = -65,
    }

    [Serializable, NetSerializable]
    public sealed class ApcBoundInterfaceState : BoundUserInterfaceState, IEquatable<ApcBoundInterfaceState>
    {
        public readonly bool MainBreaker;
        public readonly int Power;
        public readonly ApcExternalPowerState ApcExternalPower;
        public readonly float Charge;

        public ApcBoundInterfaceState(bool mainBreaker, int power, ApcExternalPowerState apcExternalPower, float charge)
        {
            MainBreaker = mainBreaker;
            Power = power;
            ApcExternalPower = apcExternalPower;
            Charge = charge;
        }

        public bool Equals(ApcBoundInterfaceState? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MainBreaker == other.MainBreaker &&
                   Power == other.Power &&
                   ApcExternalPower == other.ApcExternalPower &&
                   MathHelper.CloseTo(Charge, other.Charge);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is ApcBoundInterfaceState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MainBreaker, Power, (int) ApcExternalPower, Charge);
        }
    }

    [Serializable, NetSerializable]
    public sealed class ApcToggleMainBreakerMessage : BoundUserInterfaceMessage
    {
    }

    public enum ApcExternalPowerState : byte
    {
        None,
        Low,
        Good,
    }

    [NetSerializable, Serializable]
    public enum ApcUiKey : byte
    {
        Key,
    }
}