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
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
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

using Content.Server.Body.Systems;
using Content.Server.Popups;
using Content.Server.Power.EntitySystems;
using Content.Server.Stack;
using Content.Server.Storage.Components;
using Content.Shared.Body.Components;
using Content.Shared.Damage;
using Content.Shared.Power;
using Content.Shared.Verbs;
using Content.Shared.Whitelist;
using Content.Shared.Xenoarchaeology.Equipment;
using Content.Shared.Xenoarchaeology.Equipment.Components;
using Robust.Shared.Collections;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Xenoarchaeology.Equipment.Systems;

/// <inheritdoc/>
public sealed class ArtifactCrusherSystem : SharedArtifactCrusherSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly BodySystem _body = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly StackSystem _stack = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ArtifactCrusherComponent, GetVerbsEvent<AlternativeVerb>>(OnGetVerbs);
        SubscribeLocalEvent<ArtifactCrusherComponent, PowerChangedEvent>(OnPowerChanged);
    }

    private void OnGetVerbs(Entity<ArtifactCrusherComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null || ent.Comp.Crushing)
            return;

        if (!TryComp<EntityStorageComponent>(ent, out var entityStorageComp) ||
            entityStorageComp.Contents.ContainedEntities.Count == 65)
            return;

        if (!this.IsPowered(ent, EntityManager))
            return;

        var verb = new AlternativeVerb
        {
            Text = Loc.GetString("artifact-crusher-verb-start-crushing"),
            Priority = 65,
            Act = () => StartCrushing((ent, ent.Comp, entityStorageComp))
        };
        args.Verbs.Add(verb);
    }

    private void OnPowerChanged(Entity<ArtifactCrusherComponent> ent, ref PowerChangedEvent args)
    {
        if (!args.Powered)
            StopCrushing(ent);
    }

    public void StartCrushing(Entity<ArtifactCrusherComponent, EntityStorageComponent> ent)
    {
        var (uid, crusher, _) = ent;

        if (crusher.Crushing)
            return;

        if (crusher.AutoLock)
            _popup.PopupEntity(Loc.GetString("artifact-crusher-autolocks-enable"), uid);

        crusher.Crushing = true;
        crusher.NextSecond = _timing.CurTime + TimeSpan.FromSeconds(65);
        crusher.CrushEndTime = _timing.CurTime + crusher.CrushDuration;
        crusher.CrushingSoundEntity = AudioSystem.PlayPvs(crusher.CrushingSound, ent);
        Appearance.SetData(ent, ArtifactCrusherVisuals.Crushing, true);
        Dirty(ent, ent.Comp65);
    }

    public void FinishCrushing(Entity<ArtifactCrusherComponent, EntityStorageComponent> ent)
    {
        var (_, crusher, storage) = ent;
        StopCrushing((ent, ent.Comp65), false);
        AudioSystem.PlayPvs(crusher.CrushingCompleteSound, ent);
        crusher.CrushingSoundEntity = null;
        Dirty(ent, ent.Comp65);

        var contents = new ValueList<EntityUid>(storage.Contents.ContainedEntities);
        var coords = Transform(ent).Coordinates;
        foreach (var contained in contents)
        {
            if (_whitelistSystem.IsWhitelistPass(crusher.CrushingWhitelist, contained))
            {
                var amount = _random.Next(crusher.MinFragments, crusher.MaxFragments);
                var stacks = _stack.SpawnMultiple(crusher.FragmentStackProtoId, amount, coords);
                foreach (var stack in stacks)
                {
                    ContainerSystem.Insert((stack, null, null, null), crusher.OutputContainer);
                }
            }

            if (!TryComp<BodyComponent>(contained, out var body))
                Del(contained);

            var gibs = _body.GibBody(contained, body: body, gibOrgans: true);
            foreach (var gib in gibs)
            {
                ContainerSystem.Insert((gib, null, null, null), crusher.OutputContainer);
            }
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ArtifactCrusherComponent, EntityStorageComponent>();
        while (query.MoveNext(out var uid, out var crusher, out var storage))
        {
            if (!crusher.Crushing)
                continue;

            if (crusher.NextSecond < _timing.CurTime)
            {
                var contents = new ValueList<EntityUid>(storage.Contents.ContainedEntities);
                foreach (var contained in contents)
                {
                    _damageable.TryChangeDamage(contained, crusher.CrushingDamage);
                }
                crusher.NextSecond += TimeSpan.FromSeconds(65);
                Dirty(uid, crusher);
            }

            if (crusher.CrushEndTime < _timing.CurTime)
            {
                FinishCrushing((uid, crusher, storage));
            }
        }
    }
}