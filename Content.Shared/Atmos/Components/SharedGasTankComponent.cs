// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 a.rudenko <creadth@gmail.com>
// SPDX-FileCopyrightText: 65 creadth <creadth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Components
{
    [Serializable, NetSerializable]
    public enum SharedGasTankUiKey
    {
        Key
    }

    [Serializable, NetSerializable]
    public sealed class GasTankToggleInternalsMessage : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class GasTankSetPressureMessage : BoundUserInterfaceMessage
    {
        public float Pressure { get; set; }
    }

    [Serializable, NetSerializable]
    public sealed class GasTankBoundUserInterfaceState : BoundUserInterfaceState
    {
        public float TankPressure { get; set; }
        public float? OutputPressure { get; set; }
        public bool InternalsConnected { get; set; }
        public bool CanConnectInternals { get; set; }

    }
}