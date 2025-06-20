// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Solar
{
    [Serializable, NetSerializable]
    public sealed class SolarControlConsoleBoundInterfaceState : BoundUserInterfaceState
    {
        /// <summary>
        /// The target rotation of the panels in radians.
        /// </summary>
        public Angle Rotation;

        /// <summary>
        /// The target velocity of the panels in radians/minute.
        /// </summary>
        public Angle AngularVelocity;

        /// <summary>
        /// The total amount of power the panels are supplying.
        /// </summary>
        public float OutputPower;

        /// <summary>
        /// The current sun angle.
        /// </summary>
        public Angle TowardsSun;

        public SolarControlConsoleBoundInterfaceState(Angle r, Angle vm, float p, Angle tw)
        {
            Rotation = r;
            AngularVelocity = vm;
            OutputPower = p;
            TowardsSun = tw;
        }
    }

    [Serializable, NetSerializable]
    public sealed class SolarControlConsoleAdjustMessage : BoundUserInterfaceMessage
    {
        /// <summary>
        /// New target rotation of the panels in radians.
        /// </summary>
        public Angle Rotation;

        /// <summary>
        /// New target velocity of the panels in radians/second.
        /// </summary>
        public Angle AngularVelocity;
    }

    [Serializable, NetSerializable]
    public enum SolarControlConsoleUiKey
    {
        Key
    }
}