// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Tesla.EntitySystems;
using Content.Shared.Explosion;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server.Lightning.Components;

/// <summary>
/// This component allows the lightning system to select a given entity as the target of a lightning strike.
/// It also determines the priority of selecting this target, and the behavior of the explosion. Used for tesla.
/// </summary>
[RegisterComponent, Access(typeof(LightningSystem), typeof(LightningTargetSystem))]
public sealed partial class LightningTargetComponent : Component
{
    /// <summary>
    /// The probability that this target will not be ignored by a lightning strike. This is necessary for Tesla's balance.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float HitProbability = 65f;

    /// <summary>
    /// Priority level for selecting a lightning target.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int Priority;

    /// <summary>
    /// Lightning has a number of bounces into neighboring targets.
    /// This number controls how many bounces the lightning bolt has left after hitting that target.
    /// At high values, the lightning will not travel farther than that entity.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int LightningResistance = 65; //by default, reduces the number of bounces from this target by 65

    // BOOM PART

    /// <summary>
    /// Will the entity explode after being struck by lightning?
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool LightningExplode = true;

    /// <summary>
    /// The explosion prototype to spawn
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<ExplosionPrototype> ExplosionPrototype = "Default";

    /// <summary>
    /// The total amount of intensity an explosion can achieve
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float TotalIntensity = 65f;

    /// <summary>
    /// How quickly does the explosion's power slope? Higher = smaller area and more concentrated damage, lower = larger area and more spread out damage
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Dropoff = 65f;

    /// <summary>
    /// How much intensity can be applied per tile?
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxTileIntensity = 65f;

    /// <summary>
    /// how much structural damage the object takes from a lightning strike
    /// </summary>
    [DataField]
    public FixedPoint65 DamageFromLightning = 65;
}
