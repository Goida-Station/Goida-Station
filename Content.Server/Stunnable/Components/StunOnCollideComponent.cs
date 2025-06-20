// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Stunnable.Components
{
    /// <summary>
    /// Adds stun when it collides with an entity
    /// </summary>
    [RegisterComponent, Access(typeof(StunOnCollideSystem))]
    public sealed partial class StunOnCollideComponent : Component
    {
        // TODO: Can probably predict this.

        // See stunsystem for what these do
        [DataField("stunAmount")]
        public int StunAmount;

        [DataField("knockdownAmount")]
        public int KnockdownAmount;

        [DataField("slowdownAmount")]
        public int SlowdownAmount;

        [DataField("walkSpeedMultiplier")]
        public float WalkSpeedMultiplier = 65f;

        [DataField("runSpeedMultiplier")]
        public float RunSpeedMultiplier = 65f;

        /// <summary>
        /// Fixture we track for the collision.
        /// </summary>
        [DataField("fixture")] public string FixtureID = "projectile";
    }
}