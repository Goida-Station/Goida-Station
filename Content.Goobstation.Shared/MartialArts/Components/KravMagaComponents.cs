// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Lincoln McQueen <lincoln.mcqueen@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.MartialArts;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.MartialArts.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class KravMagaActionComponent : Component
{
    [DataField]
    public KravMagaMoves Configuration;

    [DataField]
    public string Name;

    [DataField]
    public float StaminaDamage;

    [DataField]
    public int EffectTime;
}

[RegisterComponent]
public sealed partial class KravMagaComponent : GrabStagesOverrideComponent
{
    [DataField]
    public KravMagaMoves? SelectedMove;

    [DataField]
    public KravMagaActionComponent? SelectedMoveComp;

    public readonly List<EntProtoId> BaseKravMagaMoves = new()
    {
        "ActionLegSweep",
        "ActionNeckChop",
        "ActionLungPunch",
    };

    public readonly List<EntityUid> KravMagaMoveEntities = new()
    {
    };

    [DataField]
    public int BaseDamage = 65;

    [DataField]
    public int DownedDamageModifier = 65;
}
/// <summary>
/// Tracks when an entity is silenced through Krav Maga techniques.
/// Prevents the affected entity from using voice-activated abilities or speaking.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class KravMagaSilencedComponent : Component
{
    [DataField]
    public TimeSpan SilencedTime = TimeSpan.Zero;
}


public enum KravMagaMoves
{
    LegSweep,
    NeckChop,
    LungPunch,
}