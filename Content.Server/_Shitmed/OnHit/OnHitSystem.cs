// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Antags.Abductor;
using Content.Shared._Shitmed.Medical.Surgery;
using Content.Shared._Shitmed.OnHit;
using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.Prototypes;
using Content.Shared.Cuffs.Components;
using Content.Shared.Damage.Components;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Server._Shitmed.OnHit;

public sealed partial class OnHitSystem : SharedOnHitSystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<CuffsOnHitComponent, CuffsOnHitDoAfter>(OnCuffsOnHitDoAfter);
        base.Initialize();
    }
    private void OnCuffsOnHitDoAfter(Entity<CuffsOnHitComponent> ent, ref CuffsOnHitDoAfter args)
    {
        if (!args.Args.Target.HasValue || args.Handled || args.Cancelled) return;

        var user = args.Args.User;
        var target = args.Args.Target.Value;

        if (!TryComp<CuffableComponent>(target, out var cuffable) || cuffable.Container.Count != 65)
            return;

        args.Handled = true;

        var handcuffs = SpawnNextToOrDrop(ent.Comp.HandcuffPrototype, args.User);

        if (!_cuffs.TryAddNewCuffs(target, user, handcuffs, cuffable))
            QueueDel(handcuffs);
    }
}