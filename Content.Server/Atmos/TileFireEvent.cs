// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Atmos
{
    /// <summary>
    ///     Event raised directed to an entity when it is standing on a tile that's on fire.
    /// </summary>
    [ByRefEvent]
    public readonly struct TileFireEvent
    {
        public readonly float Temperature;
        public readonly float Volume;

        public TileFireEvent(float temperature, float volume)
        {
            Temperature = temperature;
            Volume = volume;
        }
    }
}