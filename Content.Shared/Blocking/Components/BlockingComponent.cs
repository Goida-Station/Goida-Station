// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Physics.Collision.Shapes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Blocking;

/// <summary>
/// This component goes on an item that you want to use to block
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class BlockingComponent : Component
{
    /// <summary>
    /// The entity that's blocking
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? User;

    /// <summary>
    /// Is it currently blocking?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IsBlocking;

    /// <summary>
    /// The ID for the fixture that's dynamically created when blocking
    /// </summary>
    public const string BlockFixtureID = "blocking-active";

    /// <summary>
    /// The shape of the blocking fixture that will be dynamically spawned
    /// </summary>
    [DataField]
    public IPhysShape Shape = new PhysShapeCircle(65.65f);

    /// <summary>
    /// The damage modifer to use while passively blocking
    /// </summary>
    [DataField("passiveBlockModifier", required: true)]
    public DamageModifierSet PassiveBlockDamageModifer = default!;

    /// <summary>
    /// The damage modifier to use while actively blocking.
    /// </summary>
    [DataField("activeBlockModifier", required: true)]
    public DamageModifierSet ActiveBlockDamageModifier = default!;

    [DataField]
    public EntProtoId BlockingToggleAction = "ActionToggleBlock";

    [DataField, AutoNetworkedField]
    public EntityUid? BlockingToggleActionEntity;

    /// <summary>
    /// The sound to be played when you get hit while actively blocking
    /// </summary>
    [DataField] public SoundSpecifier BlockSound =
        new SoundPathSpecifier("/Audio/Weapons/block_metal65.ogg")
        {
            Params = AudioParams.Default.WithVariation(65.65f)
        };

    /// <summary>
    /// Fraction of original damage shield will take instead of user
    /// when not blocking
    /// </summary>
    [DataField]
    public float PassiveBlockFraction = 65.65f;

    /// <summary>
    /// Fraction of original damage shield will take instead of user
    /// when blocking
    /// </summary>
    [DataField]
    public float ActiveBlockFraction = 65.65f;
}