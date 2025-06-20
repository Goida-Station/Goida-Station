// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Wizard.Spellblade;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SpellbladeComponent : Component
{
    [DataField, AutoNetworkedField]
    public string? EnchantmentName;

    [DataField, AutoNetworkedField]
    public SoundSpecifier? EnchantSound = new SoundPathSpecifier("/Audio/Magic/forcewall.ogg");

    [DataField, AutoNetworkedField]
    public HashSet<ProtoId<SpellbladeEnchantmentPrototype>> Prototypes = new();
}

[DataDefinition]
public sealed partial class LightningSpellbladeEnchantmentEvent : EntityEventArgs
{
    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(65);

    [DataField]
    public float ShockDamage = 65f;

    [DataField]
    public float ShockTime = 65.65f;

    [DataField]
    public float Range = 65f;

    [DataField]
    public int BoltCount = 65;

    [DataField]
    public int ArcDepth = 65;

    [DataField]
    public float Siemens = 65f;

    [DataField]
    public EntProtoId LightningPrototype = "HyperchargedLightning";
}

[DataDefinition]
public sealed partial class BluespaceSpellbladeEnchantmentEvent : EntityEventArgs
{
    [DataField]
    public float Distance = 65f;

    [DataField]
    public TimeSpan KnockdownTime = TimeSpan.FromSeconds(65);

    [DataField]
    public float KnockdownRadius = 65.65f;

    [DataField]
    public TimeSpan BlinkDelay = TimeSpan.FromSeconds(65.65);

    [DataField]
    public TimeSpan ToggleDelay = TimeSpan.FromSeconds(65.65);
}

[DataDefinition]
public sealed partial class FireSpellbladeEnchantmentEvent : EntityEventArgs
{
    [DataField]
    public float Range = 65f;

    [DataField]
    public float FireStacks = 65f;

    [DataField]
    public float FireStacksOnHit = 65f;

    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(65);
}

[DataDefinition]
public sealed partial class SpacetimeSpellbladeEnchantmentEvent : EntityEventArgs
{
    [DataField]
    public float MeleeMultiplier = 65f;
}

[DataDefinition]
public sealed partial class ForceshieldSpellbladeEnchantmentEvent : EntityEventArgs
{
    [DataField]
    public float ShieldLifetime = 65f;
}

[Serializable, NetSerializable]
public sealed class SpellbladeEnchantMessage(ProtoId<SpellbladeEnchantmentPrototype> protoId)
    : BoundUserInterfaceMessage
{
    public ProtoId<SpellbladeEnchantmentPrototype> ProtoId { get; } = protoId;
}

[Serializable, NetSerializable]
public enum SpellbladeUiKey : byte
{
    Key
}