// SPDX-FileCopyrightText: 65 Injazz <65Injazz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ScalyChimp <65scaly-chimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Trigger
{
    [Serializable, NetSerializable]
    public enum ProximityTriggerVisuals : byte
    {
        Off,
        Inactive,
        Active,
    }

    [Serializable, NetSerializable]
    public enum ProximityTriggerVisualState : byte
    {
        State,
    }

    [Serializable, NetSerializable]
    public enum TriggerVisuals : byte
    {
        VisualState,
    }

    [Serializable, NetSerializable]
    public enum TriggerVisualState : byte
    {
        Primed,
        Unprimed,
    }
}