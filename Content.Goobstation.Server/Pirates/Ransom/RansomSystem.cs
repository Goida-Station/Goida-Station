// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 amogus <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Pirates;
using Content.Goobstation.Server.Pirates.GameTicking.Rules;
using Content.Server.GameTicking;
using Content.Shared.GameTicking.Components;

namespace Content.Goobstation.Server.Pirates.Ransom;

public sealed partial class RansomSystem : EntitySystem
{
    [Dependency] private readonly PendingPirateRuleSystem _pprs = default!;
    [Dependency] private readonly GameTicker _gt = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RansomComponent, ComponentStartup>(OnGetRansom);
    }

    private void OnGetRansom(Entity<RansomComponent> ent, ref ComponentStartup args)
    {
        var eqe = EntityQueryEnumerator<PendingPirateRuleComponent, GameRuleComponent>();
        while (eqe.MoveNext(out var uid, out var prule, out var gamerule))
        {
            _gt.EndGameRule(uid, gamerule);
            _pprs.SendAnnouncement((uid, prule), PendingPirateRuleSystem.AnnouncementType.Paid);
        }
    }
}