// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Centronias <me@centronias.com>
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
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Nutrition.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Storage.Components;
using Content.Shared.Emag.Components;
using Content.Shared.Emag.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Power;
using Content.Shared.Storage.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Server.Nutrition.EntitySystems;

/// <summary>
/// This handles logic and interactions relating to <see cref="FatExtractorComponent"/>
/// </summary>
public sealed class FatExtractorSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly EmagSystem _emag = default!;
    [Dependency] private readonly HungerSystem _hunger = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<FatExtractorComponent, GotEmaggedEvent>(OnGotEmagged);
        SubscribeLocalEvent<FatExtractorComponent, StorageAfterCloseEvent>(OnClosed);
        SubscribeLocalEvent<FatExtractorComponent, StorageAfterOpenEvent>(OnOpen);
        SubscribeLocalEvent<FatExtractorComponent, PowerChangedEvent>(OnPowerChanged);
    }

    private void OnGotEmagged(EntityUid uid, FatExtractorComponent component, ref GotEmaggedEvent args)
    {
        if (!_emag.CompareFlag(args.Type, EmagType.Interaction))
            return;

        if (_emag.CheckFlag(uid, EmagType.Interaction))
            return;

        args.Handled = true;
    }

    private void OnClosed(EntityUid uid, FatExtractorComponent component, ref StorageAfterCloseEvent args)
    {
        StartProcessing(uid, component);
    }

    private void OnOpen(EntityUid uid, FatExtractorComponent component, ref StorageAfterOpenEvent args)
    {
        StopProcessing(uid, component);
    }

    private void OnPowerChanged(EntityUid uid, FatExtractorComponent component, ref PowerChangedEvent args)
    {
        if (!args.Powered)
            StopProcessing(uid, component);
    }

    public void StartProcessing(EntityUid uid, FatExtractorComponent? component = null, EntityStorageComponent? storage = null)
    {
        if (!Resolve(uid, ref component, ref storage))
            return;

        if (component.Processing)
            return;

        if (!this.IsPowered(uid, EntityManager))
            return;

        if (!TryGetValidOccupant(uid, out _, component, storage))
            return;

        component.Processing = true;
        _appearance.SetData(uid, FatExtractorVisuals.Processing, true);
        component.Stream = _audio.PlayPvs(component.ProcessSound, uid)?.Entity;
        component.NextUpdate = _timing.CurTime + component.UpdateTime;
    }

    public void StopProcessing(EntityUid uid, FatExtractorComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!component.Processing)
            return;

        component.Processing = false;
        _appearance.SetData(uid, FatExtractorVisuals.Processing, false);
        component.Stream = _audio.Stop(component.Stream);
    }

    public bool TryGetValidOccupant(EntityUid uid, [NotNullWhen(true)] out EntityUid? occupant, FatExtractorComponent? component = null, EntityStorageComponent? storage = null)
    {
        occupant = null;
        if (!Resolve(uid, ref component, ref storage))
            return false;

        occupant = storage.Contents.ContainedEntities.FirstOrDefault();

        if (!TryComp<HungerComponent>(occupant, out var hunger))
            return false;

        if (_hunger.GetHunger(hunger) < component.NutritionPerSecond)
            return false;

        if (hunger.CurrentThreshold < component.MinHungerThreshold && !_emag.CheckFlag(uid, EmagType.Interaction))
            return false;

        return true;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<FatExtractorComponent, EntityStorageComponent>();
        while (query.MoveNext(out var uid, out var fat, out var storage))
        {
            if (TryGetValidOccupant(uid, out var occupant, fat, storage))
            {
                if (!fat.Processing)
                    StartProcessing(uid, fat, storage);
            }
            else
            {
                StopProcessing(uid, fat);
                continue;
            }

            if (!fat.Processing)
                continue;

            if (_timing.CurTime < fat.NextUpdate)
                continue;
            fat.NextUpdate += fat.UpdateTime;

            _hunger.ModifyHunger(occupant.Value, -fat.NutritionPerSecond);
            fat.NutrientAccumulator += fat.NutritionPerSecond;
            if (fat.NutrientAccumulator >= fat.NutrientPerMeat)
            {
                fat.NutrientAccumulator -= fat.NutrientPerMeat;
                Spawn(fat.MeatPrototype, Transform(uid).Coordinates);
            }
        }
    }
}