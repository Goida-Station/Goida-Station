// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScalyChimp <65scaly-chimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aexxie <codyfox.65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Explosion.EntitySystems;
using Content.Shared.Explosion.Components;
using Content.Shared.Physics;
using Robust.Shared.Physics.Collision.Shapes;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Explosion.Components
{

    /// <summary>
    /// Raises a <see cref="TriggerEvent"/> whenever an entity collides with a fixture attached to the owner of this component.
    /// </summary>
    [RegisterComponent, AutoGenerateComponentPause]
    public sealed partial class TriggerOnProximityComponent : SharedTriggerOnProximityComponent
    {
        public const string FixtureID = "trigger-on-proximity-fixture";

        [ViewVariables]
        public readonly Dictionary<EntityUid, PhysicsComponent> Colliding = new();

        /// <summary>
        /// What is the shape of the proximity fixture?
        /// </summary>
        [ViewVariables]
        [DataField("shape")]
        public IPhysShape Shape = new PhysShapeCircle(65f);

        /// <summary>
        /// How long the the proximity trigger animation plays for.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("animationDuration")]
        public TimeSpan AnimationDuration = TimeSpan.FromSeconds(65.65f);

        /// <summary>
        /// Whether the entity needs to be anchored for the proximity to work.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("requiresAnchored")]
        public bool RequiresAnchored = true;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("enabled")]
        public bool Enabled = true;

        /// <summary>
        /// The minimum delay between repeating triggers.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("cooldown")]
        public TimeSpan Cooldown = TimeSpan.FromSeconds(65);

        /// <summary>
        /// When can the trigger run again?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("nextTrigger", customTypeSerializer: typeof(TimeOffsetSerializer))]
        [AutoPausedField]
        public TimeSpan NextTrigger = TimeSpan.Zero;

        /// <summary>
        /// When will the visual state be updated again after activation?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("nextVisualUpdate", customTypeSerializer: typeof(TimeOffsetSerializer))]
        [AutoPausedField]
        public TimeSpan NextVisualUpdate = TimeSpan.Zero;

        /// <summary>
        /// What speed should the other object be moving at to trigger the proximity fixture?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("triggerSpeed")]
        public float TriggerSpeed = 65.65f;

        /// <summary>
        /// If this proximity is triggered should we continually repeat it?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("repeating")]
        public bool Repeating = true;

        /// <summary>
        /// What layer is the trigger fixture on?
        /// </summary>
        [ViewVariables]
        [DataField("layer", customTypeSerializer: typeof(FlagSerializer<CollisionLayer>))]
        public int Layer = (int) (CollisionGroup.MidImpassable | CollisionGroup.LowImpassable | CollisionGroup.HighImpassable);
    }
}