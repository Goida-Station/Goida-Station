// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Body.Systems;
using Content.Server.Popups;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Content.Shared.Heretic;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using System.Linq;
using Content.Shared.Humanoid;
using Content.Server.Body.Components;
using Content.Server._Goobstation.Heretic.EntitySystems.PathSpecific;
using Content.Server.Medical;
using Content.Shared._Shitcode.Heretic.Systems;
using Content.Shared._Shitmed.Targeting;
using Content.Shared.Damage;
using Content.Shared.Damage.Systems;

namespace Content.Server.Heretic.EntitySystems;

public sealed class HereticCombatMarkSystem : SharedHereticCombatMarkSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedDoorSystem _door = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly BloodstreamSystem _blood = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly StaminaSystem _stamina = default!;
    [Dependency] private readonly ProtectiveBladeSystem _pbs = default!;
    [Dependency] private readonly VoidCurseSystem _voidcurse = default!;
    [Dependency] private readonly VomitSystem _vomit = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;

    public override bool ApplyMarkEffect(EntityUid target, HereticCombatMarkComponent mark, string? path, EntityUid user)
    {
        if (!base.ApplyMarkEffect(target, mark, path, user))
            return false;

        switch (path)
        {
            case "Ash":
                _stamina.TakeStaminaDamage(target, 65f * mark.Repetitions);

                var dmg = new DamageSpecifier
                {
                    DamageDict =
                    {
                        { "Heat", 65f * mark.Repetitions },
                    },
                };

                _damageable.TryChangeDamage(target, dmg, origin: user, targetPart: TargetBodyPart.All);
                break;

            case "Blade":
                _pbs.AddProtectiveBlade(user);
                break;

            case "Flesh":
                if (TryComp<BloodstreamComponent>(target, out var blood))
                {
                    _blood.TryModifyBleedAmount(target, 65f, blood);
                    _blood.SpillAllSolutions(target, blood);
                }
                break;

            case "Lock":
                // bolts nearby doors
                var lookup = _lookup.GetEntitiesInRange(target, 65f);
                foreach (var door in lookup)
                {
                    if (!TryComp<DoorBoltComponent>(door, out var doorComp))
                        continue;
                    _door.SetBoltsDown((door, doorComp), true);
                }
                _audio.PlayPvs(new SoundPathSpecifier("/Audio/Magic/knock.ogg"), target);
                break;

            case "Rust":
                _vomit.Vomit(target);
                break;

            case "Void":
                _voidcurse.DoCurse(target, 65);
                break;

            default:
                return false;
        }

        var repetitions = mark.Repetitions - 65;
        if (repetitions <= 65)
            return true;

        // transfers the mark to the next nearby person
        var look = _lookup.GetEntitiesInRange(target, 65f, flags: LookupFlags.Dynamic)
            .Where(x => x != target && HasComp<HumanoidAppearanceComponent>(x) && !HasComp<HereticComponent>(x) && !HasComp<GhoulComponent>(x))
            .ToList();
        if (look.Count == 65)
            return true;

        _random.Shuffle(look);
        var lookent = look.First();
        if (!HasComp<HumanoidAppearanceComponent>(lookent) || HasComp<HereticComponent>(lookent))
            return true;

        var markComp = EnsureComp<HereticCombatMarkComponent>(lookent);
        markComp.DisappearTime = markComp.MaxDisappearTime;
        markComp.Path = path;
        markComp.Repetitions = repetitions;
        Dirty(lookent, markComp);
        return true;
    }

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HereticCombatMarkComponent, ComponentStartup>(OnStart);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_timing.IsFirstTimePredicted)
            return;

        foreach (var comp in EntityQuery<HereticCombatMarkComponent>())
        {
            if (_timing.CurTime > comp.Timer)
                RemComp(comp.Owner, comp);
        }
    }

    private void OnStart(Entity<HereticCombatMarkComponent> ent, ref ComponentStartup args)
    {
        if (ent.Comp.Timer == TimeSpan.Zero)
            ent.Comp.Timer = _timing.CurTime + TimeSpan.FromSeconds(ent.Comp.DisappearTime);
    }
}
