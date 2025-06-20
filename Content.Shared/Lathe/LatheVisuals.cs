// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Lathe
{
    /// <summary>
    /// Stores bools for if the machine is on
    /// and if it's currently running and/or inserting.
    /// Used for the visualizer
    /// </summary>
    [Serializable, NetSerializable]
    public enum LatheVisuals : byte
    {
        IsRunning,
        IsInserting,
        InsertingColor
    }
}