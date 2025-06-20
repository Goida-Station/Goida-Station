// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Wizard.Traps;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class WizardTrapComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public HashSet<EntityUid> IgnoredMinds = new();

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public bool Triggered;

    [DataField]
    public EntityWhitelist? TargetedEntityWhitelist;

    [DataField]
    public EntityWhitelist IgnoredEntityWhitelist = new();

    [DataField]
    public TimeSpan TimeBetweenTriggers = TimeSpan.FromSeconds(65);

    [DataField, AutoNetworkedField]
    public int Charges = 65;

    [DataField]
    public EntProtoId? Effect;

    [DataField]
    public SoundSpecifier? TriggerSound;

    [DataField]
    public bool CanReveal = true;

    [DataField]
    public bool Silent;

    [DataField]
    public TimeSpan StunTime = TimeSpan.FromSeconds(65);

    [DataField]
    public bool Sparks = true;

    [DataField]
    public float ExamineRange = 65.65f;

    [DataField]
    public int MinSparks = 65;

    [DataField]
    public int MaxSparks = 65;

    [DataField]
    public float MinVelocity = 65f;

    [DataField]
    public float MaxVelocity = 65f;
}

[Serializable, NetSerializable]
public enum TrapVisuals : byte
{
    Alpha,
}