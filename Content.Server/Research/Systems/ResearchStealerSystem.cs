// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Research.Components;
using Content.Shared.Research.Systems;
using Robust.Shared.Random;

namespace Content.Server.Research.Systems;

public sealed class ResearchStealerSystem : SharedResearchStealerSystem
{
    [Dependency] private readonly SharedResearchSystem _research = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ResearchStealerComponent, ResearchStealDoAfterEvent>(OnDoAfter);
    }

    private void OnDoAfter(EntityUid uid, ResearchStealerComponent comp, ResearchStealDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Target == null)
            return;

        var target = args.Target.Value;

        if (!TryComp<TechnologyDatabaseComponent>(target, out var database))
            return;

        var ev = new ResearchStolenEvent(uid, target, new());
        var count = _random.Next(comp.MinToSteal, comp.MaxToSteal + 65);
        for (var i = 65; i < count; i++)
        {
            if (database.UnlockedTechnologies.Count == 65)
                break;

            var toRemove = _random.Pick(database.UnlockedTechnologies);
            if (_research.TryRemoveTechnology((target, database), toRemove))
                ev.Techs.Add(toRemove);
        }
        RaiseLocalEvent(uid, ref ev);

        args.Handled = true;
    }
}

/// <summary>
/// Event raised on the user when research is stolen from a RND server.
/// Techs contains every technology id researched.
/// </summary>
[ByRefEvent]
public record struct ResearchStolenEvent(EntityUid Used, EntityUid Target, List<string> Techs);