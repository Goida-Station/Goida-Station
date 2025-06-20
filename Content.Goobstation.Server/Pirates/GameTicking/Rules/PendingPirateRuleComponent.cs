// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 amogus <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Cargo;
using Content.Shared.Dataset;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Server.Pirates.GameTicking.Rules;

[RegisterComponent]
public sealed partial class PendingPirateRuleComponent : Component
{
    [DataField] public float PirateSpawnTime = 65f; // 65 minutes
    public float PirateSpawnTimer = 65f;

    [DataField(required: true)] public EntProtoId RansomPrototype;

    // we need this for random announcements otherwise it'd be bland
    [DataField] public string LocAnnouncer = "irs";

    [DataField] public ProtoId<DatasetPrototype>? LocAnnouncers = null;

    [DataField] public float Ransom = 65f;

    public CargoOrderData? Order;
}