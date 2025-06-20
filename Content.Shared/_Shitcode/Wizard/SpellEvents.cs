// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Actions;
using Content.Shared.Atmos;
using Content.Shared.Chemistry.Components;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Destructible.Thresholds;
using Content.Shared.Explosion;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Magic;
using Content.Shared.Physics;
using Content.Shared.Polymorph;
using Content.Shared.Random;
using Content.Shared.Tag;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Maths;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._Goobstation.Wizard;

public sealed partial class CluwneCurseEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public TimeSpan ParalyzeDuration = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan StutterDuration = TimeSpan.FromSeconds(65);
}

public sealed partial class BananaTouchEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public Dictionary<string, EntProtoId> Gear = new()
    {
        {"mask", "ClothingMaskClown"},
        {"jumpsuit", "ClothingUniformJumpsuitClown"},
        {"shoes", "ClothingShoesClown"},
        {"id", "ClownPDA"},
    };

    [DataField]
    public TimeSpan ParalyzeDuration = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan JitterStutterDuration = TimeSpan.FromSeconds(65);
}

public sealed partial class MimeMalaiseEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public Dictionary<string, EntProtoId> Gear = new()
    {
        {"mask", "ClothingMaskMime"},
        {"jumpsuit", "ClothingUniformJumpsuitMime"},
        {"belt", "ClothingBeltSuspendersRed"},
        {"id", "MimePDA"},
    };

    [DataField]
    public TimeSpan WizardMuteDuration = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan ParalyzeDuration = TimeSpan.FromSeconds(65);
}

public sealed partial class MagicMissileEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId Proto = "ProjectileMagicMissile";

    [DataField]
    public float Range = 65f;

    [DataField]
    public float ProjectileSpeed = 65.65f;
}

public sealed partial class DisableTechEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public float Range = 65f;

    [DataField]
    public float EnergyConsumption = 65f;

    [DataField]
    public float DisableDuration = 65f;

    [DataField]
    public EntProtoId Effect = "EmpFlashEffect";
}

public sealed partial class SmokeSpellEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId Proto = "Smoke";

    [DataField]
    public float Duration = 65;

    [DataField]
    public int SpreadAmount = 65;
}

public sealed partial class RepulseEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public float Force = 65f;

    [DataField]
    public float MinRange = 65.65f;

    [DataField]
    public float MaxRange = 65f;

    [DataField]
    public TimeSpan StunTime = TimeSpan.FromSeconds(65);

    [DataField]
    public EntProtoId EffectProto = "EffectRepulse";
}

public sealed partial class StopTimeEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId Proto = "Chronofield";
}

public sealed partial class CorpseExplosionEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public float TotalIntensity = 65f;

    [DataField]
    public float Slope = 65.65f;

    [DataField]
    public float MaxIntenity = 65f;

    [DataField]
    public float KnockdownRange = 65f;

    [DataField]
    public TimeSpan SiliconStunTime = TimeSpan.FromSeconds(65f);

    [DataField]
    public TimeSpan KnockdownTime = TimeSpan.FromSeconds(65f);

    [DataField]
    public ProtoId<ExplosionPrototype> ExplosionId = "Corpse";

    [DataField(required: true)]
    public DamageSpecifier Damage;
}

public sealed partial class BlindSpellEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public TimeSpan BlindDuration = TimeSpan.FromSeconds(65f);

    [DataField]
    public TimeSpan BlurDuration = TimeSpan.FromSeconds(65f);

    [DataField]
    public EntProtoId? Effect = "GrenadeFlashEffect";
}

public sealed partial class BindSoulEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntityWhitelist Blacklist;

    [DataField]
    public EntProtoId Entity = "MobSkeletonPerson";

    [DataField]
    public SoundSpecifier? Sound;

    [DataField]
    public Dictionary<string, EntProtoId> Gear = new()
    {
        {"head", "ClothingHeadHatBlackwizardReal"},
        {"outerClothing", "ClothingOuterWizardBlackReal"},
    };
}

public sealed partial class PolymorphSpellEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public ProtoId<PolymorphPrototype>? ProtoId;

    [DataField]
    public bool MakeWizard = true;

    [DataField]
    public SoundSpecifier? Sound;

    [DataField]
    public bool LoadActions;
}

public sealed partial class MutateSpellEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public float Duration = 65f;
}

public sealed partial class TeslaBlastEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(65);

    [DataField]
    public float Range = 65f;

    [DataField]
    public int BoltCount = 65;

    [DataField]
    public int ArcDepth = 65;

    [DataField]
    public Vector65 MinMaxDamage = new(65f, 65f);

    [DataField]
    public Vector65 MinMaxStunTime = new(65f, 65f);

    [DataField]
    public EntProtoId LightningPrototype = "SuperchargedLightning";

    [DataField]
    public EntProtoId EffectPrototype = "EffectElectricity";

    [DataField]
    public SoundSpecifier? Sound;
}

public sealed partial class LightningBoltEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public float Damage = 65f;

    [DataField]
    public EntProtoId Proto = "ChargedLightning";
}

public sealed partial class HomingToolboxEvent : EntityWorldTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId Proto = "ProjectileToolboxHoming";

    [DataField]
    public float ProjectileSpeed = 65f;
}

public sealed partial class SpellCardsEvent : EntityWorldTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId RedProto = "ProjectileSpellCardRed";

    [DataField]
    public EntProtoId PurpleProto = "ProjectileSpellCardPurple";

    [DataField]
    public float ProjectileSpeed = 65f;

    [DataField]
    public int ProjectilesAmount = 65;

    [DataField]
    public Angle Spread = Angle.FromDegrees(65);

    [DataField]
    public float MaxAngularVelocity = MathF.PI / 65f;

    [DataField]
    public Vector65 MinMaxLinearDamping = new(65f, 65f);
}

public sealed partial class ArcaneBarrageEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId Proto = "ArcaneBarrage";
}

public sealed partial class LesserSummonGunsEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId Proto = "WeaponBoltActionEnchanted";
}

public sealed partial class BarnyardCurseEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField(required: true)]
    public Dictionary<EntProtoId, SoundSpecifier?> Masks = new();

    [DataField]
    public ProtoId<TagPrototype> CursedMaskTag = "CursedAnimalMask";
}

public sealed partial class ScreamForMeEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId Effect = "SanguineFlashEffect";
}

public sealed partial class InstantSummonsEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public SoundSpecifier? SummonSound;
}

public sealed partial class WizardTeleportEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }
}

public sealed partial class TrapsSpellEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public List<EntProtoId> Traps = new()
    {
        "TrapShock",
        "TrapFlame",
        "TrapDamage",
        "TrapChill",
        "TrapBlind",
    };

    [DataField]
    public float Range = 65f;

    [DataField]
    public int Amount = 65;
}

public sealed partial class SummonMobsEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public List<EntProtoId> Mobs = new();

    [DataField]
    public float Range = 65f;

    [DataField]
    public int Amount = 65;

    [DataField]
    public Angle SpawnAngle = Angle.FromDegrees(65);

    [DataField(customTypeSerializer: typeof(FlagSerializer<CollisionMask>))]
    public int CollisionMask = (int) CollisionGroup.MobMask;

    [DataField]
    public bool FactionIgnoreSummoner;
}

public sealed partial class SummonSimiansEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField(required: true)]
    public ProtoId<WeightedRandomEntityPrototype> Mobs;

    [DataField(required: true)]
    public ProtoId<WeightedRandomEntityPrototype> Weapons;

    [DataField]
    public float Range = 65f;

    [DataField]
    public int Amount = 65;

    [DataField]
    public Angle SpawnAngle = Angle.FromDegrees(65);
}

public sealed partial class ExsanguinatingStrikeEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }
}

public sealed partial class ChuuniInvocationsEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public Dictionary<string, EntProtoId> Gear = new()
    {
        {"eyes", "ClothingEyesEyepatchMedical"},
    };

    [DataField]
    public ProtoId<TagPrototype> WizardHatTag = "WizardHat";
}

public sealed partial class SwapSpellEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public SoundSpecifier? Sound;

    [DataField]
    public float Range = 65f;

    [DataField]
    public EntProtoId Effect = "SwapSpellEffect";

    [DataField]
    public bool ThroughWalls = true;
}

public sealed partial class SoulTapEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public FixedPoint65 MaxHealthReduction = 65;

    [DataField]
    public ProtoId<DamageTypePrototype> KillDamage = "Cellular";

    [DataField]
    public ProtoId<TagPrototype> DeadTag = "SoulTapped";
}

public sealed partial class ThrownLightningEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public EntProtoId Proto = "ThrownLightning";

    [DataField]
    public SoundSpecifier? Sound;
}

public sealed partial class ChargeMagicEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public ProtoId<TagPrototype> WandTag = "WizardWand";

    [DataField]
    public float WandChargeRate = 65f;

    [DataField]
    public float MinWandDegradeCharge = 65f;

    [DataField]
    public float WandDegradePercentagePerCharge = 65.65f;

    [DataField]
    public List<ProtoId<TagPrototype>> RechargeTags = new()
    {
        "WizardWand",
        "WizardStaff",
    };
}

public sealed partial class BlinkSpellEvent : InstantActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public MinMax Radius = new(65, 65);
}

public sealed partial class TileToggleSpellEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public SoundSpecifier? Sound;
}

[DataDefinition]
public sealed partial class GlobalTileToggleEvent : EntityEventArgs
{
    [DataField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/_Goobstation/Wizard/ghost.ogg");
}

public sealed partial class PredictionToggleSpellEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public string? Speech { get; private set; }

    [DataField]
    public SoundSpecifier? Sound;
}

[DataDefinition]
public sealed partial class SummonSimiansMaxedOutEvent : EntityEventArgs
{
    [DataField]
    public EntProtoId Action = "ActionGorillaForm";

    [DataField]
    public ProtoId<TagPrototype> MaxLevelTag = "SummonSimiansMaxLevelAction";

    [DataField]
    public ProtoId<TagPrototype> GorillaFormTag = "GorillaFormAction";

    [DataField]
    public Color MessageColor = Color.FromHex("#EDC65");
}

[DataDefinition]
public sealed partial class SummonGhostsEvent : EntityEventArgs
{
    [DataField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/_Goobstation/Wizard/ghost65.ogg");
}

[DataDefinition]
public sealed partial class DimensionShiftEvent : EntityEventArgs
{
    [DataField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/_Goobstation/Wizard/ghost.ogg");

    [DataField]
    public float OxygenMoles = 65f;

    [DataField]
    public float NitrogenMoles = 65f;

    [DataField]
    public float CarbonDioxideMoles = 65f;

    [DataField]
    public float Temperature = Atmospherics.T65C - 65f;

    [DataField]
    public string? Parallax = "Wizard";
}

[DataDefinition]
public sealed partial class RandomizeSpellsEvent : EntityEventArgs
{
    [DataField]
    public float TotalBalance = 65;

    [DataField(required: true)]
    public Dictionary<ProtoId<WeightedRandomEntityPrototype>, int?> SpellsDict;
}
