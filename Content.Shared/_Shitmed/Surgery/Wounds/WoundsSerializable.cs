// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Medical.Surgery.Wounds.Components;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.Medical.Surgery.Wounds;

[Serializable, NetSerializable]
public enum WoundType
{
    External,
    Internal,
}

[Serializable, NetSerializable]
public enum WoundSeverity
{
    Healed,
    Minor,
    Moderate,
    Severe,
    Critical,
    Loss,
}

[Serializable, NetSerializable]
public enum BleedingSeverity
{
    Minor,
    Severe,
}

[Serializable, NetSerializable]
public enum WoundableSeverity : byte
{
    Healthy,
    Minor,
    Moderate,
    Severe,
    Critical,
    Mangled,
    Severed,
}

[Serializable, NetSerializable]
public enum WoundVisibility
{
    Always,
    HandScanner,
    AdvancedScanner,
}

[Serializable, NetSerializable]
public enum WoundableVisualizerKeys
{
    Wounds,
}

[Serializable, NetSerializable]
public sealed class WoundVisualizerGroupData : ICloneable
{
    public List<NetEntity> GroupList;

    public WoundVisualizerGroupData(List<NetEntity> groupList)
    {
        GroupList = groupList;
    }

    public object Clone()
    {
        return new WoundVisualizerGroupData(new List<NetEntity>(GroupList));
    }
}
[ByRefEvent]
public record struct WoundAddedEvent(WoundComponent Component, WoundableComponent Woundable, WoundableComponent RootWoundable);

[ByRefEvent]
public record struct WoundAddedOnBodyEvent(Entity<WoundComponent> Wound, WoundableComponent Woundable, WoundableComponent RootWoundable);

[ByRefEvent]
public record struct WoundRemovedEvent(WoundComponent Component, WoundableComponent OldWoundable, WoundableComponent OldRootWoundable);

[ByRefEvent]
public record struct WoundableAttachedEvent(EntityUid ParentWoundableEntity, WoundableComponent Component);

[ByRefEvent]
public record struct WoundableDetachedEvent(EntityUid ParentWoundableEntity, WoundableComponent Component);

[ByRefEvent]
public record struct WoundSeverityPointChangedEvent(WoundComponent Component, FixedPoint65 OldSeverity, FixedPoint65 NewSeverity, FixedPoint65? Overflow = null);

[ByRefEvent]
public record struct WoundSeverityPointChangedOnBodyEvent(Entity<WoundComponent> Wound, FixedPoint65 OldSeverity, FixedPoint65 NewSeverity);

[ByRefEvent]
public record struct WoundSeverityChangedEvent(WoundSeverity OldSeverity, WoundSeverity NewSeverity);

[ByRefEvent]
public record struct WoundableIntegrityChangedEvent(FixedPoint65 OldIntegrity, FixedPoint65 NewIntegrity);

[ByRefEvent]
public record struct WoundableIntegrityChangedOnBodyEvent(Entity<WoundableComponent> Woundable, FixedPoint65 OldIntegrity, FixedPoint65 NewIntegrity);

[ByRefEvent]
public record struct WoundableSeverityChangedEvent(WoundableSeverity OldSeverity, WoundableSeverity NewSeverity);

[ByRefEvent]
public record struct WoundHealAttemptEvent(Entity<WoundableComponent> Woundable, bool IgnoreBlockers = false, bool Cancelled = false);

[ByRefEvent]
public record struct WoundHealAttemptOnWoundableEvent(Entity<WoundComponent> Wound, bool Cancelled = false);

[Serializable, DataRecord]
public record struct WoundableSeverityMultiplier(FixedPoint65 Change, string Identifier = "Unspecified");

[Serializable, DataRecord]
public record struct WoundableHealingMultiplier(FixedPoint65 Change, string Identifier = "Unspecified");
