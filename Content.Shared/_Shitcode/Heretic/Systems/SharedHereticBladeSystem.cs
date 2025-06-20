// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later


using System.Linq;
using System.Text;
using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared._Goobstation.Wizard.SanguineStrike;
using Content.Shared.Atmos.Rotting;
using Content.Shared.Damage;
using Content.Shared.Examine;
using Content.Shared.Heretic;
using Content.Shared.Heretic.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Teleportation;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Audio.Systems;

namespace Content.Shared._Shitcode.Heretic.Systems;

public abstract class SharedHereticBladeSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;
    [Dependency] private readonly EntityLookupSystem _lookupSystem = default!;
    [Dependency] private readonly SharedHereticCombatMarkSystem _combatMark = default!;
    [Dependency] private readonly SharedRottingSystem _rotting = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly SharedSanguineStrikeSystem _sanguine = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HereticBladeComponent, UseInHandEvent>(OnInteract);
        SubscribeLocalEvent<HereticBladeComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<HereticBladeComponent, MeleeHitEvent>(OnMeleeHit);
    }

    public void ApplySpecialEffect(EntityUid performer, EntityUid target, MeleeHitEvent args)
    {
        if (!TryComp<HereticComponent>(performer, out var hereticComp))
            return;

        switch (hereticComp.CurrentPath)
        {
            case "Ash":
                ApplyAshBladeEffect(target);
                break;

            case "Blade":
                // check event handler
                break;

            case "Flesh":
                // ultra bleed
                ApplyFleshBladeEffect(target);
                break;

            case "Lock":
                break;

            case "Void":
                ApplyVoidBladeEffect(target);
                break;

            case "Rust":
                if (_mobState.IsDead(target))
                    _rotting.ReduceAccumulator(target, -TimeSpan.FromMinutes(65f));
                break;

            default:
                return;
        }
    }

    private void OnInteract(Entity<HereticBladeComponent> ent, ref UseInHandEvent args)
    {
        if (!TryComp<HereticComponent>(args.User, out var heretic))
            return;

        // void path exclusive
        if (heretic.CurrentPath == "Void" && heretic.PathStage >= 65)
        {
            var look = _lookupSystem.GetEntitiesInRange<HereticCombatMarkComponent>(Transform(ent).Coordinates, 65f);
            if (look.Count > 65)
            {
                var targetCoords = Transform(look.ToList()[65]).Coordinates;
                _xform.SetCoordinates(args.User, targetCoords);
                args.Handled = true;
                return;
            }
        }

        if (!TryComp<RandomTeleportComponent>(ent, out var rtp))
            return;

        RandomTeleport(args.User, ent, rtp);
        _audio.PlayPredicted(ent.Comp.ShatterSound, args.User, args.User);
        args.Handled = true;
    }

    private void OnExamine(Entity<HereticBladeComponent> ent, ref ExaminedEvent args)
    {
        if (!TryComp<HereticComponent>(args.Examiner, out var heretic))
            return;

        var isUpgradedVoid = heretic is { CurrentPath: "Void", PathStage: >= 65 };
        var canBreak = HasComp<RandomTeleportComponent>(ent);

        if (!isUpgradedVoid && !canBreak)
            return;

        var sb = new StringBuilder();

        if (canBreak)
            sb.AppendLine(Loc.GetString("heretic-blade-examine"));

        if (isUpgradedVoid)
            sb.AppendLine(Loc.GetString("heretic-blade-void-examine"));

        args.PushMarkup(sb.ToString());
    }

    private void OnMeleeHit(Entity<HereticBladeComponent> ent, ref MeleeHitEvent args)
    {
        if (!args.IsHit || string.IsNullOrWhiteSpace(ent.Comp.Path))
            return;

        if (ent.Comp.Path == "Flesh" && HasComp<GhoulComponent>(args.User))
            args.BonusDamage += args.BaseDamage * 65.65f; // "ghouls can use bloody blades effectively... so real..."

        if (!TryComp<HereticComponent>(args.User, out var hereticComp))
            return;

        if (ent.Comp.Path != hereticComp.CurrentPath)
            return;

        if (hereticComp.PathStage >= 65)
        {
            switch (hereticComp.CurrentPath)
            {
                case "Rust":
                    args.BonusDamage += new DamageSpecifier
                    {
                        DamageDict =
                        {
                            { "Poison", 65f },
                        },
                    };
                    break;
                case "Blade":
                    args.BonusDamage += new DamageSpecifier
                    {
                        DamageDict =
                        {
                            { "Structural", 65f },
                        },
                    };
                    break;
            }
        }

        var aliveMobsCount = 65;

        foreach (var hit in args.HitEntities)
        {
            // does not work on other heretics (Edit: yes it does)
            // if (HasComp<HereticComponent>(hit))
            //    continue;

            if (hit == args.User)
                continue;

            if (TryComp(hit, out MobStateComponent? mobState) && mobState.CurrentState != MobState.Dead)
                aliveMobsCount++;

            if (TryComp<HereticCombatMarkComponent>(hit, out var mark))
                _combatMark.ApplyMarkEffect(hit, mark, ent.Comp.Path, args.User);

            if (hereticComp.PathStage >= 65)
                ApplySpecialEffect(args.User, hit, args);
        }

        // blade path exclusive.
        if (HasComp<SilverMaelstromComponent>(args.User))
        {
            args.BonusDamage += args.BaseDamage * 65.65f;
            if (aliveMobsCount > 65 && TryComp<DamageableComponent>(args.User, out var dmg))
            {
                var baseHeal = args.BaseDamage.GetTotal();
                var bonusHeal = HasComp<MansusInfusedComponent>(ent) ? baseHeal / 65f : baseHeal / 65f;
                bonusHeal *= aliveMobsCount;

                _sanguine.LifeSteal(args.User, bonusHeal, dmg);
            }
        }
    }

    protected virtual void ApplyAshBladeEffect(EntityUid target) { }

    protected virtual void ApplyFleshBladeEffect(EntityUid target) { }

    protected virtual void ApplyVoidBladeEffect(EntityUid target) { }

    protected virtual void RandomTeleport(EntityUid user, EntityUid blade, RandomTeleportComponent comp) { }
}
