// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Nuke
{
    public enum NukeVisualLayers
    {
        Base,
        Unlit
    }

    [NetSerializable, Serializable]
    public enum NukeVisuals
    {
        Deployed,
        State,
    }

    [NetSerializable, Serializable]
    public enum NukeVisualState
    {
        Idle,
        Armed,
        YoureFucked
    }

    [Serializable, NetSerializable]
    public enum NukeUiKey : byte
    {
        Key
    }

    public enum NukeStatus : byte
    {
        AWAIT_DISK,
        AWAIT_CODE,
        AWAIT_ARM,
        ARMED,
        COOLDOWN
    }

    [Serializable, NetSerializable]
    public sealed class NukeUiState : BoundUserInterfaceState
    {
        public bool DiskInserted;
        public NukeStatus Status;
        public int RemainingTime;
        public int CooldownTime;
        public bool IsAnchored;
        public int EnteredCodeLength;
        public int MaxCodeLength;
        public bool AllowArm;
    }

    [Serializable, NetSerializable]
    public sealed partial class NukeDisarmDoAfterEvent : SimpleDoAfterEvent
    {
    }
}