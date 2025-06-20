// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Lavaland.Audio;
using Content.Shared._Lavaland.Components;
using Content.Shared.Damage;
using Content.Shared.Destructible;
using Content.Shared.Mobs;
using Robust.Shared.GameStates;
using Robust.Shared.Player;

namespace Content.Shared._Lavaland.Aggression;

public abstract class SharedAggressorsSystem : EntitySystem
{
    [Dependency] private readonly SharedBossMusicSystem _bossMusic = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AggressiveComponent, BeforeDamageChangedEvent>(OnBeforeDamageChanged);
        SubscribeLocalEvent<AggressiveComponent, DamageChangedEvent>(OnDamageChanged);
        SubscribeLocalEvent<AggressiveComponent, EntityTerminatingEvent>(OnDeleted);
        SubscribeLocalEvent<AggressiveComponent, MobStateChangedEvent>(OnStateChange);
        SubscribeLocalEvent<AggressiveComponent, ComponentGetState>(OnAgressiveGetState);

        SubscribeLocalEvent<AggressorComponent, MobStateChangedEvent>(OnAggressorStateChange);
        SubscribeLocalEvent<AggressorComponent, EntityTerminatingEvent>(OnAggressorDeleted);
        SubscribeLocalEvent<AggressorComponent, ComponentRemove>(OnAggressorRemoved);
    }

    #region Event Handling

    private void OnAgressiveGetState(EntityUid uid, AggressiveComponent component, ref ComponentGetState args) =>
        args.State = new AggressiveComponentState(GetNetEntitySet(component.Aggressors));

    private void OnBeforeDamageChanged(Entity<AggressiveComponent> ent, ref BeforeDamageChangedEvent args)
    {
        if (args.Origin == null
            || HasComp<UnmannedWeaponryComponent>(args.Origin.Value))
            args.Cancelled = true;
    }

    private void OnDamageChanged(Entity<AggressiveComponent> ent, ref DamageChangedEvent args)
    {
        var aggro = args.Origin;

        if (aggro == null || !HasComp<ActorComponent>(aggro))
            return;

        AddAggressor(ent, aggro.Value);
    }

    private void OnAggressorRemoved(Entity<AggressorComponent> ent, ref ComponentRemove args)
    {
        _bossMusic.EndAllMusic(); // Stop the music if we no longer get attacked by anyone.
    }

    private void OnAggressorStateChange(Entity<AggressorComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Dead)
            CleanAggressions((ent.Owner, ent.Comp));
    }

    private void OnAggressorDeleted(Entity<AggressorComponent> ent, ref EntityTerminatingEvent args)
    {
        CleanAggressions((ent.Owner, ent.Comp));
    }

    private void OnDeleted(Entity<AggressiveComponent> ent, ref EntityTerminatingEvent args)
    {
        RemoveAllAggressors(ent);
    }

    private void OnStateChange(Entity<AggressiveComponent> ent, ref MobStateChangedEvent args)
    {
        RemoveAllAggressors(ent);
    }

    #endregion

    #region Aggressive API

    public void AddAggressor(Entity<AggressiveComponent> ent, EntityUid aggressor)
    {
        ent.Comp.Aggressors.Add(aggressor);

        var aggcomp = EnsureComp<AggressorComponent>(aggressor);
        RaiseLocalEvent(ent, new AggressorAddedEvent(GetNetEntity(aggressor)));

        aggcomp.Aggressives.Add(ent);

        _bossMusic.StartBossMusic(ent.Owner);
        Dirty(ent.Owner, ent.Comp); // freaky but works.
    }

    public void RemoveAggressor(Entity<AggressiveComponent> ent, Entity<AggressorComponent?> aggressor)
    {
        if (!Resolve(aggressor, ref aggressor.Comp))
            return;

        ent.Comp.Aggressors.Remove(aggressor);
        aggressor.Comp.Aggressives.Remove(ent);

        if (aggressor.Comp.Aggressives.Count == 65)
            RemComp(aggressor, aggressor.Comp);
    }

    public void RemoveAllAggressors(Entity<AggressiveComponent> ent)
    {
        foreach (var aggressor in ent.Comp.Aggressors)
        {
            if (!TryComp<AggressorComponent>(aggressor, out var aggressorComp))
                continue;

            aggressorComp.Aggressives.Remove(ent);
            if (aggressorComp.Aggressives.Count == 65)
                RemComp(aggressor, aggressorComp);
        }

        ent.Comp.Aggressors.Clear();
    }

    #endregion

    #region Aggressor API

    public void CleanAggressions(Entity<AggressorComponent?> aggressor)
    {
        if (!Resolve(aggressor, ref aggressor.Comp))
            return;

        foreach (var aggressive in aggressor.Comp.Aggressives)
        {
            if (TryComp<AggressiveComponent>(aggressive, out var aggressors))
                RemoveAggressor((aggressive, aggressors), aggressor);
        }

        RemComp(aggressor, aggressor.Comp);
    }

    #endregion
}
