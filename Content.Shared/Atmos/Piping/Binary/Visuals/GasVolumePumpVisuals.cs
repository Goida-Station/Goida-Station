// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 daerSeebaer <65daerSeebaer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Visuals
{
    [Serializable, NetSerializable]
    public enum GasVolumePumpVisuals : byte
    {
        State,
    }

    [Serializable, NetSerializable]
    public enum GasVolumePumpState : byte
    {
        Off,
        On,
        Blocked,
    }
}