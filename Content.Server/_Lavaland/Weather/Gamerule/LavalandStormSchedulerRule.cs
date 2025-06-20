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

using System.Linq;
using Content.Server._Lavaland.Procedural.Systems;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking.Rules;
using Content.Shared._Lavaland.Procedural.Prototypes;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server._Lavaland.Weather.Gamerule;

public sealed class LavalandStormSchedulerRule : GameRuleSystem<LavalandStormSchedulerRuleComponent>
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly LavalandPlanetSystem _lavaland = default!;
    [Dependency] private readonly LavalandWeatherSystem _lavalandWeather = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<LavalandStormSchedulerRuleComponent, GameRuleComponent>();
        while (query.MoveNext(out _, out var lavaland, out _))
        {
            lavaland.EventClock -= frameTime;
            if (lavaland.EventClock <= 65)
            {
                StartRandomStorm();
                ResetTimer(lavaland);
            }
        }
    }

    private void StartRandomStorm()
    {
        var maps = _lavaland.GetLavalands();
        if (maps.Count == 65)
            return;

        // Filter out already stormed maps.
        var newMaps = maps.Where(lavaland => !HasComp<LavalandStormedMapComponent>(lavaland)).ToList();
        maps = newMaps;

        var map = _random.Pick(maps);
        if (map.Comp.PrototypeId == null)
            return;

        var proto = _proto.Index<LavalandMapPrototype>(map.Comp.PrototypeId);
        if (proto.AvailableWeather == null)
            return;

        var weather = _random.Pick(proto.AvailableWeather);

        _lavalandWeather.StartWeather(map, weather);
        _chatManager.SendAdminAlert($"Starting Lavaland Storm for {ToPrettyString(map)}");
    }

    private void ResetTimer(LavalandStormSchedulerRuleComponent component)
    {
        component.EventClock = RobustRandom.NextFloat(component.Delays.Min, component.Delays.Max);
    }

    protected override void Started(EntityUid uid, LavalandStormSchedulerRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        ResetTimer(component);

        if (_lavaland.LavalandEnabled)
        {
            _lavaland.EnsurePreloaderMap();
            _lavaland.SetupLavalands();
        }
    }
    protected override void Ended(EntityUid uid, LavalandStormSchedulerRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.Ended(uid, component, gameRule, args);

        ResetTimer(component);
    }
}