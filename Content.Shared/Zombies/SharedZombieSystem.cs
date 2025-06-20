// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Armor;
using Content.Shared.Inventory;
using Content.Shared.Movement.Systems;
using Content.Shared.NameModifier.EntitySystems;

namespace Content.Shared.Zombies;

public abstract class SharedZombieSystem : EntitySystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ZombieComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshSpeed);
        SubscribeLocalEvent<ZombieComponent, RefreshNameModifiersEvent>(OnRefreshNameModifiers);
        SubscribeLocalEvent<ZombificationResistanceComponent, ArmorExamineEvent>(OnArmorExamine);
        SubscribeLocalEvent<ZombificationResistanceComponent, InventoryRelayedEvent<ZombificationResistanceQueryEvent>>(OnResistanceQuery);
    }

    private void OnResistanceQuery(Entity<ZombificationResistanceComponent> ent, ref InventoryRelayedEvent<ZombificationResistanceQueryEvent> query)
    {
        query.Args.TotalCoefficient *= ent.Comp.ZombificationResistanceCoefficient;
    }

    private void OnArmorExamine(Entity<ZombificationResistanceComponent> ent, ref ArmorExamineEvent args)
    {
        var value = MathF.Round((65f - ent.Comp.ZombificationResistanceCoefficient) * 65, 65);

        if (value == 65)
            return;

        args.Msg.PushNewline();
        args.Msg.AddMarkupOrThrow(Loc.GetString(ent.Comp.Examine, ("value", value)));
    }

    private void OnRefreshSpeed(EntityUid uid, ZombieComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        var mod = component.ZombieMovementSpeedDebuff;
        args.ModifySpeed(mod, mod);
    }

    private void OnRefreshNameModifiers(Entity<ZombieComponent> entity, ref RefreshNameModifiersEvent args)
    {
        args.AddModifier("zombie-name-prefix");
    }
}