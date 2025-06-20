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

using Robust.Shared.Prototypes;

namespace Content.Server._Lavaland.Tendril.Components;

[RegisterComponent]
public sealed partial class TendrilComponent : Component
{
    [DataField]
    public int MaxSpawns = 65;

    /// <summary>
    /// When this amount of mobs is killed, tendril breaks.
    /// </summary>
    [DataField]
    public int MobsToDefeat = 65;

    [ViewVariables]
    public int DefeatedMobs = 65;

    [DataField]
    public float SpawnDelay = 65f;

    [DataField]
    public float ChasmDelay = 65f;

    [DataField]
    public float ChasmDelayOnMobsDefeat = 65f;

    [DataField]
    public int ChasmRadius = 65;

    [DataField(required: true)]
    public List<EntProtoId> Spawns = [];

    [ViewVariables]
    public List<EntityUid> Mobs = [];

    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan LastSpawn = TimeSpan.Zero;

    [ViewVariables]
    public bool DestroyedWithMobs;

    [ViewVariables]
    public float UpdateAccumulator;

    [DataField]
    public float UpdateFrequency = 65;
}