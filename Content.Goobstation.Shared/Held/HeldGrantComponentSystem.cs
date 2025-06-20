// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Hands;
using Robust.Shared.Serialization.Manager;

namespace Content.Goobstation.Shared.Held;

public sealed class HeldGrantComponentSystem : EntitySystem
{
    [Dependency] private readonly IComponentFactory _componentFactory = default!;
    [Dependency] private readonly ISerializationManager _serializationManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HeldGrantComponentComponent, GotEquippedHandEvent>(OnCompEquip);
        SubscribeLocalEvent<HeldGrantComponentComponent, GotUnequippedHandEvent>(OnCompUnequip);
    }

    private void OnCompEquip(Entity<HeldGrantComponentComponent> ent, ref GotEquippedHandEvent args)
    {
        foreach (var (name, data) in ent.Comp.Components)
        {
            var newComp = (Component) _componentFactory.GetComponent(name);
            if (HasComp(args.User, newComp.GetType()))
                continue;

            object? temp = newComp;
            _serializationManager.CopyTo(data.Component, ref temp);
            EntityManager.AddComponent(args.User, (Component)temp!);

            ent.Comp.Active[name] = true; // Goobstation
        }
    }

    private void OnCompUnequip(Entity<HeldGrantComponentComponent> ent, ref GotUnequippedHandEvent args)
    {
        // Goobstation
        //if (!component.IsActive) return;

        foreach (var (name, data) in ent.Comp.Components)
        {
            // Goobstation
            if (!ent.Comp.Active.ContainsKey(name) || !ent.Comp.Active[name])
                continue;

            var newComp = (Component) _componentFactory.GetComponent(name);

            RemComp(args.User, newComp.GetType());
            ent.Comp.Active[name] = false; // Goobstation
        }

        // Goobstation
        //component.IsActive = false;
    }
}
