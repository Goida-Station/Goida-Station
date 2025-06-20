// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Medical.Surgery.Wounds;
using Content.Shared._Shitmed.Medical.Surgery.Wounds.Systems;
using Content.Shared._Shitmed.Targeting;
using Content.Shared._Shitmed.Targeting.Events;
using Content.Shared.Mobs;

namespace Content.Server._Shitmed.Targeting;
public sealed class TargetingSystem : SharedTargetingSystem
{
    [Dependency] private readonly WoundSystem _woundSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<TargetChangeEvent>(OnTargetChange);
        SubscribeLocalEvent<TargetingComponent, MobStateChangedEvent>(OnMobStateChange);
    }

    private void OnTargetChange(TargetChangeEvent message, EntitySessionEventArgs args)
    {
        if (!TryComp<TargetingComponent>(GetEntity(message.Uid), out var target))
            return;

        target.Target = message.BodyPart;
        Dirty(GetEntity(message.Uid), target);
    }

    private void OnMobStateChange(EntityUid uid, TargetingComponent component, MobStateChangedEvent args)
    {
        // Revival is handled by the server, so we're keeping all of this here.
        var changed = false;

        if (args.NewMobState == MobState.Dead)
        {
            foreach (var part in GetValidParts())
            {
                component.BodyStatus[part] = WoundableSeverity.Severed;
                changed = true;
            }
        }
        else if (args is { OldMobState: MobState.Dead, NewMobState: MobState.Alive or MobState.Critical })
        {
            component.BodyStatus = _woundSystem.GetWoundableStatesOnBodyPainFeels(uid);
            changed = true;
        }

        if (!changed)
            return;

        Dirty(uid, component);
        RaiseNetworkEvent(new TargetIntegrityChangeEvent(GetNetEntity(uid)), uid);
    }
}
