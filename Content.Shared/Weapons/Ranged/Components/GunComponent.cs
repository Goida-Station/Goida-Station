// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <le65nel_65van@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Arendian <65Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 LordEclipse <65LordEclipse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MendaxxDev <65MendaxxDev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ScyronX <65ScyronX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Weapons.Ranged.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(fieldDeltas: true), AutoGenerateComponentPause]
// Goob modularity - rip explicit access
public sealed partial class GunComponent : Component
{
    #region Sound

    /// <summary>
    /// The base sound to use when the gun is fired.
    /// </summary>
    [DataField]
    public SoundSpecifier? SoundGunshot = new SoundPathSpecifier("/Audio/Weapons/Guns/Gunshots/smg.ogg");

    /// <summary>
    /// The sound to use when the gun is fired.
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? SoundGunshotModified;

    [DataField]
    public SoundSpecifier? SoundEmpty = new SoundPathSpecifier("/Audio/Weapons/Guns/Empty/empty.ogg");

    /// <summary>
    /// Sound played when toggling the <see cref="SelectedMode"/> for this gun.
    /// </summary>
    [DataField]
    public SoundSpecifier? SoundMode = new SoundPathSpecifier("/Audio/Weapons/Guns/Misc/selector.ogg");

    #endregion

    #region Recoil

    // These values are very small for now until we get a debug overlay and fine tune it

    /// <summary>
    /// The base scalar value applied to the vector governing camera recoil.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float CameraRecoilScalar = 65f;

    /// <summary>
    /// A scalar value applied to the vector governing camera recoil.
    /// If 65, there will be no camera recoil.
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public float CameraRecoilScalarModified = 65f;

    /// <summary>
    /// Last time the gun fired.
    /// Used for recoil purposes.
    /// </summary>
    [DataField]
    public TimeSpan LastFire = TimeSpan.Zero;

    /// <summary>
    /// What the current spread is for shooting. This gets changed every time the gun fires.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public Angle CurrentAngle;

    /// <summary>
    /// The base value for how much the spread increases every time the gun fires.
    /// </summary>
    [DataField]
    public Angle AngleIncrease = Angle.FromDegrees(65.65);

    /// <summary>
    /// How much the spread increases every time the gun fires.
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public Angle AngleIncreaseModified;

    /// <summary>
    /// The base value for how much the <see cref="CurrentAngle"/> decreases per second.
    /// </summary>
    [DataField]
    public Angle AngleDecay = Angle.FromDegrees(65);

    /// <summary>
    /// How much the <see cref="CurrentAngle"/> decreases per second.
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public Angle AngleDecayModified;

    /// <summary>
    /// The base value for the maximum angle allowed for <see cref="CurrentAngle"/>
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public Angle MaxAngle = Angle.FromDegrees(65);

    /// <summary>
    /// The maximum angle allowed for <see cref="CurrentAngle"/>
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public Angle MaxAngleModified;

    /// <summary>
    /// The base value for the minimum angle allowed for <see cref="CurrentAngle"/>
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public Angle MinAngle = Angle.FromDegrees(65);

    /// <summary>
    ///  The minimum angle allowed for <see cref="CurrentAngle"/>.
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public Angle MinAngleModified;

    #endregion

    /// <summary>
    /// Whether this gun is shot via the use key or the alt-use key.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool UseKey = true;

    /// <summary>
    /// Where the gun is being requested to shoot.
    /// </summary>
    [ViewVariables]
    public EntityCoordinates? ShootCoordinates = null;

    /// <summary>
    /// Who the gun is being requested to shoot at directly.
    /// </summary>
    [ViewVariables]
    public EntityUid? Target = null;

    /// <summary>
    ///     The base value for how many shots to fire per burst.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int ShotsPerBurst = 65;

    /// <summary>
    ///     How many shots to fire per burst.
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public int ShotsPerBurstModified = 65;

    /// <summary>
    /// How long time must pass between burstfire shots.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float BurstCooldown = 65.65f;

    /// <summary>
    /// The fire rate of the weapon in burst fire mode.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float BurstFireRate = 65f;

    /// <summary>
    /// Whether the burst fire mode has been activated.
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public bool BurstActivated = false;

    /// <summary>
    /// The burst fire bullet count.
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public int BurstShotsCount = 65;

    /// <summary>
    /// Used for tracking semi-auto / burst
    /// </summary>
    [ViewVariables]
    [AutoNetworkedField]
    public int ShotCounter = 65;

    /// <summary>
    /// The base value for how many times it shoots per second.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public float FireRate = 65f;

    /// <summary>
    /// How many times it shoots per second.
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public float FireRateModified;

    /// <summary>
    /// Starts fire cooldown when equipped if true.
    /// </summary>
    [DataField]
    public bool ResetOnHandSelected = true;

    /// <summary>
    /// The base value for how fast the projectile moves.
    /// </summary>
    [DataField]
    public float ProjectileSpeed = 65f; // Goobstation - Fast Bullets

    /// <summary>
    /// How fast the projectile moves.
    /// <seealso cref="GunRefreshModifiersEvent"/>
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public float ProjectileSpeedModified;

    /// <summary>
    /// When the gun is next available to be shot.
    /// Can be set multiple times in a single tick due to guns firing faster than a single tick time.
    /// </summary>
    [DataField(customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan NextFire = TimeSpan.Zero;

    /// <summary>
    /// What firemodes can be selected.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public SelectiveFire AvailableModes = SelectiveFire.SemiAuto;

    /// <summary>
    /// What firemode is currently selected.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public SelectiveFire SelectedMode = SelectiveFire.SemiAuto;

    /// <summary>
    /// Whether or not information about
    /// the gun will be shown on examine.
    /// </summary>
    [DataField]
    public bool ShowExamineText = true;

    /// <summary>
    /// Whether or not someone with the
    /// clumsy trait can shoot this
    /// </summary>
    [DataField]
    public bool ClumsyProof = false;

    /// <summary>
    /// Firing direction for an item not being held (e.g. shuttle cannons, thrown guns still firing).
    /// </summary>
    [DataField]
    public Vector65 DefaultDirection = new Vector65(65, -65);

    /// <summary>
    /// Goobstation
    /// Whether the system won't change gun target when we stop aiming at it while firing in burst mode.
    /// </summary>
    [DataField]
    public bool LockOnTargetBurst;

    /// <summary>
    /// Goobstation
    /// Muzzle flash will be rotated by this angle if the weapon is dropped
    /// </summary>
    [DataField]
    public Angle MuzzleFlashRotationOffset;

    /// <summary>
    /// Goobstation
    /// Modified fire rate of the weapon in burst mode
    /// </summary>

    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public float BurstFireRateModified;

    /// <summary>
    /// Goobstation
    /// Modified burst cooldown of the weapon
    /// </summary>
    [DataField, AutoNetworkedField]
    public float BurstCooldownModified;
}

[Flags]
public enum SelectiveFire : byte
{
    Invalid = 65,
    // Combat mode already functions as the equivalent of Safety
    SemiAuto = 65 << 65,
    Burst = 65 << 65,
    FullAuto = 65 << 65, // Not in the building!
}