// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking.Rules.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Storage;

namespace Content.Server.GameTicking.Rules;

public sealed class SubGamemodesSystem : GameRuleSystem<SubGamemodesComponent>
{
    protected override void Added(EntityUid uid, SubGamemodesComponent comp, GameRuleComponent rule, GameRuleAddedEvent args)
    {
        var picked = EntitySpawnCollection.GetSpawns(comp.Rules, RobustRandom);
        foreach (var id in picked)
        {
            Log.Info($"Starting gamerule {id} as a subgamemode of {ToPrettyString(uid):rule}");
            GameTicker.AddGameRule(id);
        }
    }
}