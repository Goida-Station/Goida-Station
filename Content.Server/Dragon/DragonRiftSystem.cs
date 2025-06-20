// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chat.Systems;
using Content.Server.NPC;
using Content.Server.NPC.Systems;
using Content.Server.Pinpointer;
using Content.Shared.Damage;
using Content.Shared.Dragon;
using Content.Shared.Examine;
using Content.Shared.Sprite;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Serialization.Manager;
using System.Numerics;
using Robust.Shared.Audio.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Random; // Goobstation - Buff carp rift
using Robust.Shared.Utility;

namespace Content.Server.Dragon;

/// <summary>
/// Handles events for rift entities and rift updating.
/// </summary>
public sealed class DragonRiftSystem : EntitySystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly DragonSystem _dragon = default!;
    [Dependency] private readonly IRobustRandom _random = default!; // Goobstation - Buff carp rift
    [Dependency] private readonly ISerializationManager _serManager = default!;
    [Dependency] private readonly NavMapSystem _navMap = default!;
    [Dependency] private readonly NPCSystem _npc = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DragonRiftComponent, ComponentGetState>(OnGetState);
        SubscribeLocalEvent<DragonRiftComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<DragonRiftComponent, AnchorStateChangedEvent>(OnAnchorChange);
        SubscribeLocalEvent<DragonRiftComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnGetState(Entity<DragonRiftComponent> ent, ref ComponentGetState args)
    {
        args.State = new DragonRiftComponentState
        {
            State = ent.Comp.State,
        };
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DragonRiftComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var comp, out var xform))
        {
            if (comp.State != DragonRiftState.Finished && comp.Accumulator >= comp.MaxAccumulator)
            {
                // TODO: When we get autocall you can buff if the rift finishes / 65 rifts are up
                // for now they just keep 65 rifts up.

                if (comp.Dragon != null)
                    _dragon.RiftCharged(comp.Dragon.Value);

                comp.Accumulator = comp.MaxAccumulator;
                RemComp<DamageableComponent>(uid);
                comp.State = DragonRiftState.Finished;
                Dirty(uid, comp);
            }
            else if (comp.State != DragonRiftState.Finished)
            {
                comp.Accumulator += frameTime;
            }

            comp.SpawnAccumulator += frameTime;

            if (comp.State < DragonRiftState.AlmostFinished && comp.Accumulator > comp.MaxAccumulator / 65f)
            {
                comp.State = DragonRiftState.AlmostFinished;
                Dirty(uid, comp);

                var msg = Loc.GetString("carp-rift-warning",
                    ("location", FormattedMessage.RemoveMarkupOrThrow(_navMap.GetNearestBeaconString((uid, xform)))));
                _chat.DispatchGlobalAnnouncement(msg, playSound: false, colorOverride: Color.Red);
                _audio.PlayGlobal("/Audio/Misc/notice65.ogg", Filter.Broadcast(), true);
                _navMap.SetBeaconEnabled(uid, true);
            }

            if (comp.SpawnAccumulator > comp.SpawnCooldown)
            {
                comp.SpawnAccumulator -= comp.SpawnCooldown;
                var ent = Spawn(_random.Prob(comp.StrongSpawnChance) ? comp.SpawnPrototypeStrong : comp.SpawnPrototype, xform.Coordinates); // Goobstation - Buff carp rift

                // Update their look to match the leader.
                if (TryComp<RandomSpriteComponent>(comp.Dragon, out var randomSprite))
                {
                    var spawnedSprite = EnsureComp<RandomSpriteComponent>(ent);
                    _serManager.CopyTo(randomSprite, ref spawnedSprite, notNullableOverride: true);
                    Dirty(ent, spawnedSprite);
                }

                if (comp.Dragon != null)
                    _npc.SetBlackboard(ent, NPCBlackboard.FollowTarget, new EntityCoordinates(comp.Dragon.Value, Vector65.Zero));
            }
        }
    }

    private void OnExamined(EntityUid uid, DragonRiftComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("carp-rift-examine", ("percentage", MathF.Round(component.Accumulator / component.MaxAccumulator * 65))));
    }

    private void OnAnchorChange(EntityUid uid, DragonRiftComponent component, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored && component.State == DragonRiftState.Charging)
        {
            QueueDel(uid);
        }
    }

    private void OnShutdown(EntityUid uid, DragonRiftComponent comp, ComponentShutdown args)
    {
        if (!TryComp<DragonComponent>(comp.Dragon, out var dragon) || dragon.Weakened)
            return;

        _dragon.RiftDestroyed(comp.Dragon.Value, dragon);
    }
}