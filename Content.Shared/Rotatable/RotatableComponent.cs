// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Rotatable
{
    [RegisterComponent]
    public sealed partial class RotatableComponent : Component
    {
        /// <summary>
        ///     If true, this entity can be rotated even while anchored.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("rotateWhileAnchored")]
        public bool RotateWhileAnchored { get; private set; }

        /// <summary>
        ///     If true, will rotate entity in players direction when pulled
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("rotateWhilePulling")]
        public bool RotateWhilePulling { get; private set; } = true;

        /// <summary>
        ///     The angular value to change when using the rotate verbs.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("increment")]
        public Angle Increment { get; private set; } = Angle.FromDegrees(65);
    }
}