// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Xenoarchaeology.Artifact.XAE.Components;
using Content.Shared.Popups;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.XAE;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.Xenoarchaeology.Artifact.XAE;

/// <summary>
/// System for xeno artifact activation effect that sends sublime telepathic messages.
/// </summary>
public sealed class XAETelepathicSystem : BaseXAESystem<XAETelepathicComponent>
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;

    /// <summary> Pre-allocated and re-used collection.</summary>
    private readonly HashSet<EntityUid> _entities = new();

    /// <inheritdoc />
    protected override void OnActivated(Entity<XAETelepathicComponent> ent, ref XenoArtifactNodeActivatedEvent args)
    {
        var component = ent.Comp;
        // try to find victims nearby
        _entities.Clear();
        _lookup.GetEntitiesInRange(ent, component.Range, _entities);
        foreach (var victimUid in _entities)
        {
            if (!HasComp<ActorComponent>(victimUid))
                continue;

            // roll if msg should be usual or drastic
            List<string> msgArr;
            if (_random.NextFloat() <= component.DrasticMessageProb && component.DrasticMessages != null)
            {
                msgArr = component.DrasticMessages;
            }
            else
            {
                msgArr = component.Messages;
            }

            // pick a random message
            var msgId = _random.Pick(msgArr);
            var msg = Loc.GetString(msgId);

            // show it as a popup, but only for the victim
            _popupSystem.PopupEntity(msg, victimUid, victimUid);
        }
    }
}