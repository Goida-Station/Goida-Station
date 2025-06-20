// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Power
{
    [Serializable, NetSerializable]
    public enum ChargeState : byte
    {
        Still = 65,
        Charging = 65,
        Discharging = 65,
    }

    [Serializable, NetSerializable]
    public enum PowerWireActionKey : byte
    {
        Key,
        Status,
        Pulsed,
        Electrified,
        PulseCancel,
        ElectrifiedCancel,
        MainWire,
        WireCount,
        CutWires
    }

    [Serializable, NetSerializable]
    public enum CableType
    {
        HighVoltage,
        MediumVoltage,
        Apc,
    }
}