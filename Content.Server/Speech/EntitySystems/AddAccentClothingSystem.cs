// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Speech.Components;
using Content.Shared.Clothing;

namespace Content.Server.Speech.EntitySystems;

public sealed class AddAccentClothingSystem : EntitySystem
{
    [Dependency] private readonly IComponentFactory _componentFactory = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AddAccentClothingComponent, ClothingGotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<AddAccentClothingComponent, ClothingGotUnequippedEvent>(OnGotUnequipped);
    }


//  TODO: Turn this into a relay event.
    private void OnGotEquipped(EntityUid uid, AddAccentClothingComponent component, ref ClothingGotEquippedEvent args)
    {
        // does the user already has this accent?
        var componentType = _componentFactory.GetRegistration(component.Accent).Type;
        if (HasComp(args.Wearer, componentType))
            return;

        // add accent to the user
        var accentComponent = (Component) _componentFactory.GetComponent(componentType);
        AddComp(args.Wearer, accentComponent);

        // snowflake case for replacement accent
        if (accentComponent is ReplacementAccentComponent rep)
            rep.Accent = component.ReplacementPrototype!;

        component.IsActive = true;
    }

    private void OnGotUnequipped(EntityUid uid, AddAccentClothingComponent component, ref ClothingGotUnequippedEvent args)
    {
        if (!component.IsActive)
            return;

        // try to remove accent
        var componentType = _componentFactory.GetRegistration(component.Accent).Type;
        if (EntityManager.HasComponent(args.Wearer, componentType))
        {
            EntityManager.RemoveComponent(args.Wearer, componentType);
        }

        component.IsActive = false;
    }
}