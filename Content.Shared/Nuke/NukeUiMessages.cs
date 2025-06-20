// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Nuke
{
    public abstract partial class SharedNukeComponent : Component
    {
        public const string NukeDiskSlotId = "Nuke";
    }

    [Serializable, NetSerializable]
    public sealed class NukeAnchorMessage : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class NukeKeypadMessage : BoundUserInterfaceMessage
    {
        public int Value;

        public NukeKeypadMessage(int value)
        {
            Value = value;
        }
    }

    [Serializable, NetSerializable]
    public sealed class NukeKeypadClearMessage : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class NukeKeypadEnterMessage : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class NukeArmedMessage : BoundUserInterfaceMessage
    {
    }
}