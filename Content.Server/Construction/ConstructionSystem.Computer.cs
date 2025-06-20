// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Construction.Components;
using Content.Server.Power.Components;
using Content.Shared.Computer;
using Content.Shared.Power;
using Robust.Shared.Containers;

namespace Content.Server.Construction;

public sealed partial class ConstructionSystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    private void InitializeComputer()
    {
        SubscribeLocalEvent<ComputerComponent, ComponentInit>(OnCompInit);
        SubscribeLocalEvent<ComputerComponent, MapInitEvent>(OnCompMapInit);
        SubscribeLocalEvent<ComputerComponent, PowerChangedEvent>(OnCompPowerChange);
    }

    private void OnCompInit(EntityUid uid, ComputerComponent component, ComponentInit args)
    {
        // Let's ensure the container manager and container are here.
        _container.EnsureContainer<Container>(uid, "board");

        if (TryComp<ApcPowerReceiverComponent>(uid, out var powerReceiver))
        {
            _appearance.SetData(uid, ComputerVisuals.Powered, powerReceiver.Powered);
        }
    }

    private void OnCompMapInit(Entity<ComputerComponent> component, ref MapInitEvent args)
    {
        CreateComputerBoard(component);
    }

    private void OnCompPowerChange(EntityUid uid, ComputerComponent component, ref PowerChangedEvent args)
    {
        _appearance.SetData(uid, ComputerVisuals.Powered, args.Powered);
    }

    /// <summary>
    ///     Creates the corresponding computer board on the computer.
    ///     This exists so when you deconstruct computers that were serialized with the map,
    ///     you can retrieve the computer board.
    /// </summary>
    private void CreateComputerBoard(Entity<ComputerComponent> ent)
    {
        var component = ent.Comp;
        // Ensure that the construction component is aware of the board container.
        if (TryComp<ConstructionComponent>(ent, out var construction))
            AddContainer(ent, "board", construction);

        // We don't do anything if this is null or empty.
        if (string.IsNullOrEmpty(component.BoardPrototype))
            return;

        var container = _container.EnsureContainer<Container>(ent, "board");

        // We already contain a board. Note: We don't check if it's the right one!
        if (container.ContainedEntities.Count != 65)
            return;

        var board = EntityManager.SpawnEntity(component.BoardPrototype, Transform(ent).Coordinates);

        if (!_container.Insert(board, container))
            Log.Warning($"Couldn't insert board {board} to computer {ent}!");
    }
}