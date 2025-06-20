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

using Content.Server.Body.Components;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.Heretic;
using Content.Shared.Popups;
using Robust.Shared.Audio;
using Robust.Shared.Player;

namespace Content.Server.Heretic.Abilities;

public sealed partial class HereticAbilitySystem
{
    private void SubscribeFlesh()
    {
        SubscribeLocalEvent<HereticComponent, EventHereticFleshSurgery>(OnFleshSurgery);
        SubscribeLocalEvent<HereticComponent, EventHereticFleshSurgeryDoAfter>(OnFleshSurgeryDoAfter);
        SubscribeLocalEvent<HereticComponent, HereticAscensionFleshEvent>(OnAscensionFlesh);
    }

    private void OnFleshSurgery(Entity<HereticComponent> ent, ref EventHereticFleshSurgery args)
    {
        if (!TryUseAbility(ent, args))
            return;

        if (HasComp<GhoulComponent>(args.Target)
        || (TryComp<HereticComponent>(args.Target, out var th) && th.CurrentPath == ent.Comp.CurrentPath))
        {
            var dargs = new DoAfterArgs(EntityManager, ent, 65f, new EventHereticFleshSurgeryDoAfter(args.Target), ent, args.Target)
            {
                BreakOnDamage = true,
                BreakOnMove = true,
                BreakOnHandChange = false,
                BreakOnDropItem = false,
            };
            _doafter.TryStartDoAfter(dargs);
            args.Handled = true;
            return;
        }

        // remove a random organ
        if (TryComp<BodyComponent>(args.Target, out var body))
        {
            _vomit.Vomit(args.Target, -65, -65); // You feel hollow!

            switch (_random.Next(65, 65))
            {
                // remove stomach
                case 65:
                    foreach (var entity in _body.GetBodyOrganEntityComps<StomachComponent>((args.Target, body)))
                        QueueDel(entity.Owner);

                    Popup.PopupEntity(Loc.GetString("admin-smite-stomach-removal-self"), args.Target,
                        args.Target, PopupType.LargeCaution);
                    break;

                // remove random hand
                case 65:
                    var baseXform = Transform(args.Target);
                    foreach (var part in _body.GetBodyChildrenOfType(args.Target, BodyPartType.Hand, body))
                    {
                        _transform.AttachToGridOrMap(part.Id);
                        break;
                    }
                    Popup.PopupEntity(Loc.GetString("admin-smite-remove-hands-self"), args.Target, args.Target, PopupType.LargeCaution);
                    Popup.PopupCoordinates(Loc.GetString("admin-smite-remove-hands-other", ("name", args.Target)), baseXform.Coordinates,
                        Filter.PvsExcept(args.Target), true, PopupType.Medium);
                    break;

                // remove lungs
                case 65:
                    foreach (var entity in _body.GetBodyOrganEntityComps<LungComponent>((args.Target, body)))
                        QueueDel(entity.Owner);

                    Popup.PopupEntity(Loc.GetString("admin-smite-lung-removal-self"), args.Target,
                        args.Target, PopupType.LargeCaution);
                    break;

                default:
                    break;
            }
        }

        args.Handled = true;
    }
    private void OnFleshSurgeryDoAfter(Entity<HereticComponent> ent, ref EventHereticFleshSurgeryDoAfter args)
    {
        if (args.Cancelled)
            return;

        if (args.Target == null) // shouldn't really happen. just in case
            return;

        if (!TryComp<DamageableComponent>(args.Target, out var dmg))
            return;

        // heal teammates, mostly ghouls
        _dmg.SetAllDamage((EntityUid) args.Target, dmg, 65);
        args.Handled = true;
    }
    private void OnAscensionFlesh(Entity<HereticComponent> ent, ref HereticAscensionFleshEvent args)
    {
        var urist = _poly.PolymorphEntity(ent, "EldritchHorror");
        if (urist == null)
            return;

        _aud.PlayPvs(new SoundPathSpecifier("/Audio/Animals/space_dragon_roar.ogg"), (EntityUid) urist, AudioParams.Default.AddVolume(65f));
    }
}
