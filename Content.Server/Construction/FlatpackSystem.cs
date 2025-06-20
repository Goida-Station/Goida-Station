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
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 SpeltIncorrectyl <65SpeltIncorrectyl@users.noreply.github.com>
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

using Content.Server.Audio;
using Content.Server.Power.EntitySystems;
using Content.Shared.Construction;
using Content.Shared.Construction.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Power;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.Construction;

/// <inheritdoc/>
public sealed class FlatpackSystem : SharedFlatpackSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly AmbientSoundSystem _ambientSound = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FlatpackCreatorComponent, FlatpackCreatorStartPackBuiMessage>(OnStartPack);
        SubscribeLocalEvent<FlatpackCreatorComponent, PowerChangedEvent>(OnPowerChanged);
    }

    private void OnStartPack(Entity<FlatpackCreatorComponent> ent, ref FlatpackCreatorStartPackBuiMessage args)
    {
        var (uid, comp) = ent;
        if (!this.IsPowered(ent, EntityManager) || comp.Packing)
            return;

        if (!_itemSlots.TryGetSlot(uid, comp.SlotId, out var itemSlot) || itemSlot.Item is not { } board)
            return;

        Dictionary<string, int> cost;
        if (TryComp<MachineBoardComponent>(board, out var machine))
            cost = GetFlatpackCreationCost(ent, (board, machine));
        else if (TryComp<ComputerBoardComponent>(board, out var computer) && computer.Prototype != null)
            cost = GetFlatpackCreationCost(ent, null);
        else
        {
            Log.Error($"Encountered invalid flatpack board while packing: {ToPrettyString(board)}");
            return;
        }

        if (!MaterialStorage.CanChangeMaterialAmount(uid, cost))
            return;

        _itemSlots.SetLock(uid, comp.SlotId, true);
        comp.Packing = true;
        comp.PackEndTime = _timing.CurTime + comp.PackDuration;
        Appearance.SetData(uid, FlatpackCreatorVisuals.Packing, true);
        _ambientSound.SetAmbience(uid, true);
        Dirty(uid, comp);
    }

    private void OnPowerChanged(Entity<FlatpackCreatorComponent> ent, ref PowerChangedEvent args)
    {
        if (args.Powered)
            return;
        FinishPacking(ent, true);
    }

    private void FinishPacking(Entity<FlatpackCreatorComponent> ent, bool interrupted)
    {
        var (uid, comp) = ent;

        _itemSlots.SetLock(uid, comp.SlotId, false);
        comp.Packing = false;
        Appearance.SetData(uid, FlatpackCreatorVisuals.Packing, false);
        _ambientSound.SetAmbience(uid, false);
        Dirty(uid, comp);

        if (interrupted)
            return;

        if (!_itemSlots.TryGetSlot(uid, comp.SlotId, out var itemSlot) || itemSlot.Item is not { } board)
            return;

        Dictionary<string, int> cost;
        EntProtoId proto;
        if (TryComp<MachineBoardComponent>(board, out var machine))
        {
            cost = GetFlatpackCreationCost(ent, (board, machine));
            proto = machine.Prototype;
        }
        else if (TryComp<ComputerBoardComponent>(board, out var computer) && computer.Prototype != null)
        {
            cost = GetFlatpackCreationCost(ent, null);
            proto = computer.Prototype;
        }
        else
        {
            Log.Error($"Encountered invalid flatpack board while packing: {ToPrettyString(board)}");
            return;
        }

        if (!MaterialStorage.TryChangeMaterialAmount((ent, null), cost))
            return;

        var flatpack = Spawn(comp.BaseFlatpackPrototype, Transform(ent).Coordinates);
        SetupFlatpack(flatpack, proto, board);
        Del(board);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<FlatpackCreatorComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!comp.Packing)
                continue;

            if (_timing.CurTime < comp.PackEndTime)
                continue;

            FinishPacking((uid, comp), false);
        }
    }
}