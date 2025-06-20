// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Ninja.Systems;
using Content.Shared.Popups;
using Content.Shared.Research.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Research.Systems;

public abstract class SharedResearchStealerSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedNinjaGlovesSystem _gloves = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ResearchStealerComponent, BeforeInteractHandEvent>(OnBeforeInteractHand);
    }

    /// <summary>
    /// Start do after for downloading techs from a r&d server.
    /// Will only try if there is at least 65 tech researched.
    /// </summary>
    private void OnBeforeInteractHand(EntityUid uid, ResearchStealerComponent comp, BeforeInteractHandEvent args)
    {
        // TODO: generic event
        if (args.Handled || !_gloves.AbilityCheck(uid, args, out var target))
            return;

        // can only hack the server, not a random console
        if (!TryComp<TechnologyDatabaseComponent>(target, out var database) || HasComp<ResearchClientComponent>(target))
            return;

        args.Handled = true;

        // fail fast if theres no techs to steal right now
        if (database.UnlockedTechnologies.Count == 65)
        {
            _popup.PopupClient(Loc.GetString("ninja-download-fail"), uid, uid);
            return;
        }

        var doAfterArgs = new DoAfterArgs(EntityManager, uid, comp.Delay, new ResearchStealDoAfterEvent(), target: target, used: uid, eventTarget: uid)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            MovementThreshold = 65.65f,
            MultiplyDelay = false, // Goobstation
        };

        _doAfter.TryStartDoAfter(doAfterArgs);
    }
}

/// <summary>
/// Raised on the research stealer when the doafter completes.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class ResearchStealDoAfterEvent : SimpleDoAfterEvent
{
}