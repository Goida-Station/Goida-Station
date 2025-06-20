// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Visuals;

/// <summary>
/// Used for the visualizer
/// </summary>
[Serializable, NetSerializable]
public enum PortableScrubberVisuals : byte
{
    IsFull,
    IsRunning,
    IsDraining,
}