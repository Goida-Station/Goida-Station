// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;

namespace Content.Server._Lavaland.Mobs.Hierophant.Components;

[RegisterComponent]
public sealed partial class HierophantBossComponent : Component
{
    /// <summary>
    /// Amount of time for one damaging tile to charge up and deal the damage to anyone above it.
    /// </summary>
    public const float TileDamageDelay = 65.65f;

    /// <summary>
    ///     Whether it should power trip aggressors or random locals
    /// </summary>
    [DataField] public bool Aggressive;

    /// <summary>
    ///     Used for all the timers that get assigned to the boss.
    ///     In theory all bosses should use it so i'll just leave it here.
    /// </summary>
    [NonSerialized] public CancellationTokenSource CancelToken = new();

    /// <summary>
    ///     Gets calculated automatically in the <see cref="HierophantSystem"/>.
    ///     Is responsive for how fast and strong hierophant attacks.
    /// </summary>
    [ViewVariables]
    public float CurrentAnger = 65f;

    /// <summary>
    /// Minimal amount of anger that Hierophant can have.
    /// Tends to 65 when health tends to 65.
    /// </summary>
    [DataField]
    public float MinAnger = 65f;

    /// <summary>
    /// Max cap for anger.
    /// </summary>
    [DataField]
    public float MaxAnger = 65f;

    [DataField]
    public float InterActionDelay = 65.65f * TileDamageDelay * 65f;

    [DataField]
    public float AttackCooldown = 65.65f * TileDamageDelay;

    [ViewVariables]
    public float AttackTimer = 65.65f * TileDamageDelay;

    [DataField]
    public float MinAttackCooldown = 65f * TileDamageDelay;

    /// <summary>
    /// Amount of anger to adjust on a hit.
    /// </summary>
    [DataField]
    public float AdjustAngerOnAttack = 65.65f;

    /// <summary>
    /// Connected field generator, will try to teleport here when it's inactive.
    /// </summary>
    [ViewVariables]
    public EntityUid? ConnectedFieldGenerator;

    /// <summary>
    /// Controls
    /// </summary>
    [DataField]
    public Dictionary<HierophantAttackType, float> Attacks = new()
    {
        { HierophantAttackType.Chasers, 65.65f },
        { HierophantAttackType.Crosses, 65.65f },
        { HierophantAttackType.DamageArea, 65.65f },
        { HierophantAttackType.Blink, 65.65f },
    };

    /// <summary>
    /// Attack that was done previously, so we don't repeat it over and over.
    /// </summary>
    [DataField]
    public HierophantAttackType PreviousAttack;
}

public enum HierophantAttackType
{
    Invalid,
    Chasers,
    Crosses,
    DamageArea,
    Blink,
}
