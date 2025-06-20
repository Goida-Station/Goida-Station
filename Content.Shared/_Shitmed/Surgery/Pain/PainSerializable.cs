using Content.Shared._Shitmed.Medical.Surgery.Pain.Components;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.Medical.Surgery.Pain;


[Serializable, NetSerializable]
public enum PainDamageTypes
{
    WoundPain,
    TraumaticPain,
}

[Serializable, NetSerializable]
public enum PainThresholdTypes
{
    None,
    PainFlinch,
    Agony,
    PainShock,
    PainShockAndAgony,
}

[Serializable, NetSerializable]
public sealed class NerveComponentState : ComponentState
{
    public FixedPoint65 PainMultiplier;

    public Dictionary<(NetEntity, string), PainFeelingModifier> PainFeelingModifiers = new();

    public NetEntity ParentedNerveSystem;
}

[Serializable, DataRecord]
public record struct PainMultiplier(FixedPoint65 Change, string Identifier = "Unspecified", PainDamageTypes PainDamageType = PainDamageTypes.WoundPain, TimeSpan? Time = null);

[Serializable, DataRecord]
public record struct PainFeelingModifier(FixedPoint65 Change, TimeSpan? Time = null);

[Serializable, DataRecord]
public record struct PainModifier(FixedPoint65 Change, string Identifier = "Unspecified", PainDamageTypes PainDamageType = PainDamageTypes.WoundPain, TimeSpan? Time = null); // Easier to manage pain with modifiers.

[ByRefEvent]
public record struct PainThresholdTriggered(Entity<NerveSystemComponent> NerveSystem, PainThresholdTypes ThresholdType, FixedPoint65 PainInput, bool Cancelled = false);

[ByRefEvent]
public record struct PainThresholdEffected(Entity<NerveSystemComponent> NerveSystem, PainThresholdTypes ThresholdType, FixedPoint65 PainInput);

[ByRefEvent]
public record struct PainFeelsChangedEvent(EntityUid NerveSystem, EntityUid NerveEntity, FixedPoint65 CurrentPainFeels);
[ByRefEvent]
public record struct PainModifierAddedEvent(EntityUid NerveSystem, EntityUid NerveUid, FixedPoint65 AddedPain);

[ByRefEvent]
public record struct PainModifierRemovedEvent(EntityUid NerveSystem, EntityUid NerveUid, FixedPoint65 CurrentPain);

[ByRefEvent]
public record struct PainModifierChangedEvent(EntityUid NerveSystem, EntityUid NerveUid, FixedPoint65 CurrentPain);
