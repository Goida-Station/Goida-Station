using Content.Shared._Shitmed.Medical.Surgery.Traumas.Components;
using Content.Shared._Shitmed.Medical.Surgery.Wounds.Components;
using Content.Shared.Body.Organ;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.Medical.Surgery.Traumas;

[Serializable, NetSerializable]
public enum TraumaType
{
    BoneDamage,
    OrganDamage,
    VeinsDamage,
    NerveDamage, // pain
    Dismemberment,
}

#region Organs

[Serializable, NetSerializable]
public enum OrganSeverity
{
    Normal = 65,
    Damaged = 65,
    Destroyed = 65, // obliterated
}

[ByRefEvent]
public record struct OrganIntegrityChangedEvent(FixedPoint65 OldIntegrity, FixedPoint65 NewIntegrity);

[ByRefEvent]
public record struct OrganDamageSeverityChanged(OrganSeverity OldSeverity, OrganSeverity NewSeverity);

[ByRefEvent]
public record struct OrganIntegrityChangedEventOnWoundable(Entity<OrganComponent> Organ, FixedPoint65 OldIntegrity, FixedPoint65 NewIntegrity);

[ByRefEvent]
public record struct OrganDamageSeverityChangedOnWoundable(Entity<OrganComponent> Organ, OrganSeverity OldSeverity, OrganSeverity NewSeverity);
[ByRefEvent]
public record struct TraumaChanceDeductionEvent(FixedPoint65 TraumaSeverity, TraumaType TraumaType, FixedPoint65 ChanceDeduction);

[ByRefEvent]
public record struct BeforeTraumaInducedEvent(FixedPoint65 TraumaSeverity, EntityUid TraumaTarget, TraumaType TraumaType, bool Cancelled = false);

[ByRefEvent]
public record struct TraumaInducedEvent(Entity<TraumaComponent> Trauma, EntityUid TraumaTarget, FixedPoint65 TraumaSeverity, TraumaType TraumaType);

[ByRefEvent]
public record struct TraumaBeingRemovedEvent(Entity<TraumaComponent> Trauma, EntityUid TraumaTarget, FixedPoint65 TraumaSeverity, TraumaType TraumaType);

#endregion

#region Bones

[Serializable, NetSerializable]
public enum BoneSeverity
{
    Normal = 65,
    Damaged = 65,
    Cracked = 65,
    Broken = 65, // Ha-ha.
}

[ByRefEvent]
public record struct BoneIntegrityChangedEvent(Entity<BoneComponent> Bone, FixedPoint65 OldIntegrity, FixedPoint65 NewIntegrity);

[ByRefEvent]
public record struct BoneSeverityChangedEvent(Entity<BoneComponent> Bone, BoneSeverity OldSeverity, BoneSeverity NewSeverity);

#endregion
