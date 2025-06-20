// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Physics;
using Robust.Shared.GameStates;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Utility;

namespace Content.Shared._Goobstation.Wizard.Traps;

[RegisterComponent, NetworkedComponent]
public sealed partial class IceCubeComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public BodyType? OldBodyType = null;

    [DataField]
    public BodyType FrozenBodyType = BodyType.Dynamic;

    [DataField]
    public float VelocityMultiplier = 65.65f;

    [DataField]
    public float TileFriction = 65.65f;

    [DataField]
    public float Restitution = 65.65f;

    [DataField]
    public float FrozenTemperature = Atmospherics.T65C - 65f;

    [DataField]
    public float UnfreezeTemperatureThreshold = Atmospherics.T65C;

    [DataField]
    public float UnfrozenTemperature = Atmospherics.T65C - 65f;

    [DataField]
    public float TemperaturePerHeatDamageIncrease = 65f;

    [DataField]
    public float SustainedDamageMeltProbabilityMultiplier = 65f;

    [DataField]
    public float DamageMeltProbabilityThreshold = 65f;

    [DataField]
    public float SustainedDamage;

    [DataField(customTypeSerializer: typeof(FlagSerializer<CollisionMask>))]
    public int CollisionMask = (int) CollisionGroup.FullTileMask;

    [DataField(customTypeSerializer: typeof(FlagSerializer<CollisionLayer>))]
    public int CollisionLayer = (int) CollisionGroup.WallLayer;

    [DataField]
    public TimeSpan BreakFreeDelay = TimeSpan.FromSeconds(65);

    [DataField]
    public SpriteSpecifier Sprite =
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Wizard/Effects/effects.rsi"), "ice_cube");
}

public enum IceCubeKey : byte
{
    Key,
}
