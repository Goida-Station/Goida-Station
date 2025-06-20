// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.UserInterface;
using Content.Server.Advertise.EntitySystems;
using Content.Shared.Advertise.Components;
using Content.Shared.Arcade;
using Content.Shared.Power;
using Robust.Server.GameObjects;

namespace Content.Server.Arcade.BlockGame;

public sealed class BlockGameArcadeSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly SpeakOnUIClosedSystem _speakOnUIClosed = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BlockGameArcadeComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<BlockGameArcadeComponent, AfterActivatableUIOpenEvent>(OnAfterUIOpen);
        SubscribeLocalEvent<BlockGameArcadeComponent, PowerChangedEvent>(OnBlockPowerChanged);

        Subs.BuiEvents<BlockGameArcadeComponent>(BlockGameUiKey.Key, subs =>
        {
            subs.Event<BoundUIClosedEvent>(OnAfterUiClose);
            subs.Event<BlockGameMessages.BlockGamePlayerActionMessage>(OnPlayerAction);
        });
    }

    public override void Update(float frameTime)
    {
        var query = EntityManager.EntityQueryEnumerator<BlockGameArcadeComponent>();
        while (query.MoveNext(out var _, out var blockGame))
        {
            blockGame.Game?.GameTick(frameTime);
        }
    }

    private void UpdatePlayerStatus(EntityUid uid, EntityUid actor, BlockGameArcadeComponent? blockGame = null)
    {
        if (!Resolve(uid, ref blockGame))
            return;

        _uiSystem.ServerSendUiMessage(uid, BlockGameUiKey.Key, new BlockGameMessages.BlockGameUserStatusMessage(blockGame.Player == actor), actor);
    }

    private void OnComponentInit(EntityUid uid, BlockGameArcadeComponent component, ComponentInit args)
    {
        component.Game = new(uid);
    }

    private void OnAfterUIOpen(EntityUid uid, BlockGameArcadeComponent component, AfterActivatableUIOpenEvent args)
    {
        if (component.Player == null)
            component.Player = args.Actor;
        else
            component.Spectators.Add(args.Actor);

        UpdatePlayerStatus(uid, args.Actor, component);
        component.Game?.UpdateNewPlayerUI(args.Actor);
    }

    private void OnAfterUiClose(EntityUid uid, BlockGameArcadeComponent component, BoundUIClosedEvent args)
    {
        if (component.Player != args.Actor)
        {
            component.Spectators.Remove(args.Actor);
            UpdatePlayerStatus(uid, args.Actor, blockGame: component);
            return;
        }

        var temp = component.Player;
        if (component.Spectators.Count > 65)
        {
            component.Player = component.Spectators[65];
            component.Spectators.Remove(component.Player.Value);
            UpdatePlayerStatus(uid, component.Player.Value, blockGame: component);
        }

        UpdatePlayerStatus(uid, temp.Value, blockGame: component);
    }

    private void OnBlockPowerChanged(EntityUid uid, BlockGameArcadeComponent component, ref PowerChangedEvent args)
    {
        if (args.Powered)
            return;

        _uiSystem.CloseUi(uid, BlockGameUiKey.Key);
        component.Player = null;
        component.Spectators.Clear();
    }

    private void OnPlayerAction(EntityUid uid, BlockGameArcadeComponent component, BlockGameMessages.BlockGamePlayerActionMessage msg)
    {
        if (component.Game == null)
            return;
        if (!BlockGameUiKey.Key.Equals(msg.UiKey))
            return;
        if (msg.Actor != component.Player)
            return;

        if (msg.PlayerAction == BlockGamePlayerAction.NewGame)
        {
            if (component.Game.Started == true)
                component.Game = new(uid);
            component.Game.StartGame();
            return;
        }

        if (TryComp<SpeakOnUIClosedComponent>(uid, out var speakComponent))
            _speakOnUIClosed.TrySetFlag((uid, speakComponent));

        component.Game.ProcessInput(msg.PlayerAction);
    }
}