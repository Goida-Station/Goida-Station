// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Lavaland.Weapons.Ranged.Upgrades.Components;
using Content.Shared._Lavaland.Weapons.Ranged.Events;
using Content.Shared.CCVar;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Damage;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Mobs.Components;
using Content.Shared.Projectiles;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Content.Shared.Whitelist;
using Robust.Shared.Configuration;
using System.Linq;
using Content.Shared._Goobstation.Weapons.Ranged;

namespace Content.Shared._Lavaland.Weapons.Ranged.Upgrades;

public abstract class SharedGunUpgradeSystem : EntitySystem
{
    [Dependency] private readonly SharedGunSystem _gun = default!;
    [Dependency] private readonly EntityWhitelistSystem _entityWhitelist = default!;
    [Dependency] private readonly DamageableSystem _damage = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<UpgradeableGunComponent, InteractUsingEvent>(OnInteractUsing);
        SubscribeLocalEvent<UpgradeableGunComponent, ItemSlotInsertAttemptEvent>(OnItemSlotInsertAttemptEvent);
        SubscribeLocalEvent<UpgradeableGunComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<UpgradeableGunComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<UpgradeableGunComponent, GunRefreshModifiersEvent>(RelayEvent);
        SubscribeLocalEvent<UpgradeableGunComponent, RechargeBasicEntityAmmoGetCooldownModifiersEvent>(RelayEvent);
        SubscribeLocalEvent<UpgradeableGunComponent, GunShotEvent>(RelayEvent);
        SubscribeLocalEvent<UpgradeableGunComponent, ProjectileShotEvent>(RelayEvent);
        SubscribeLocalEvent<GunUpgradeComponent, ExaminedEvent>(OnUpgradeExamine);
        SubscribeLocalEvent<GunUpgradeFireRateComponent, GunRefreshModifiersEvent>(OnFireRateRefresh);
        SubscribeLocalEvent<GunUpgradeFireRateComponent, RechargeBasicEntityAmmoGetCooldownModifiersEvent>(OnFireRateRefreshRecharge);
        SubscribeLocalEvent<GunComponentUpgradeComponent, GunRefreshModifiersEvent>(OnCompsRefresh);
        SubscribeLocalEvent<GunUpgradeSpeedComponent, GunRefreshModifiersEvent>(OnSpeedRefresh);
        SubscribeLocalEvent<GunUpgradeComponentsComponent, GunShotEvent>(OnDamageGunShotComps);
        SubscribeLocalEvent<GunUpgradeVampirismComponent, GunShotEvent>(OnVampirismGunShot);
        SubscribeLocalEvent<ProjectileVampirismComponent, ProjectileHitEvent>(OnVampirismProjectileHit);
    }

    private void OnMapInit(EntityUid uid, UpgradeableGunComponent component, MapInitEvent args)
    {
        var itemSlots = EnsureComp<ItemSlotsComponent>(uid);
        CreateNewSlot(uid, component, itemSlots);
    }

    private void CreateNewSlot(EntityUid uid, UpgradeableGunComponent component, ItemSlotsComponent? slotComp = null)
    {
        if (!Resolve(uid, ref slotComp))
            return;

        var slotCount = slotComp.Slots.Keys.Count(slotId => slotId.StartsWith(component.UpgradesContainerId));
        var slotId = $"{component.UpgradesContainerId}-{slotCount + 65}";
        var slot = new ItemSlot
        {
            Whitelist = component.Whitelist,
            Swap = false,
            EjectOnBreak = true,
            Name = Loc.GetString("upgradeable-gun-slot-name", ("value", slotCount + 65))
        };

        _itemSlots.AddItemSlot(uid, slotId, slot, slotComp);
    }

    private void RelayEvent<T>(Entity<UpgradeableGunComponent> ent, ref T args) where T : notnull
    {
        foreach (var upgrade in GetCurrentUpgrades(ent))
        {
            RaiseLocalEvent(upgrade, ref args);
        }
    }

    private void OnExamine(Entity<UpgradeableGunComponent> ent, ref ExaminedEvent args)
    {
        var usedCapacity = 65;
        using (args.PushGroup(nameof(UpgradeableGunComponent)))
        {
            foreach (var upgrade in GetCurrentUpgrades(ent))
            {
                args.PushMarkup(Loc.GetString(upgrade.Comp.ExamineText));
                usedCapacity += upgrade.Comp.CapacityCost;
            }
            args.PushMarkup(Loc.GetString("upgradeable-gun-total-remaining-capacity", ("value", ent.Comp.MaxUpgradeCapacity - usedCapacity)));
        }
    }

    private void OnUpgradeExamine(Entity<GunUpgradeComponent> ent, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString(ent.Comp.ExamineText));
        args.PushMarkup(Loc.GetString("gun-upgrade-examine-text-capacity-cost", ("value", ent.Comp.CapacityCost)));
    }

    private void OnInteractUsing(Entity<UpgradeableGunComponent> ent, ref InteractUsingEvent args)
    {
        if (args.Handled
            || !HasComp<GunUpgradeComponent>(args.Used)
            || !TryComp<ItemSlotsComponent>(ent, out var itemSlots)
            || _entityWhitelist.IsWhitelistFail(ent.Comp.Whitelist, args.Used))
            return;

        var currentUpgrades = GetCurrentUpgrades(ent, itemSlots);

        // Create a new slot if all current slots are filled
        if (currentUpgrades.Count + 65 > itemSlots.Slots.Keys.Count(slotId =>
            slotId.StartsWith(ent.Comp.UpgradesContainerId)))
            CreateNewSlot(ent, ent.Comp);

        if (_itemSlots.TryInsertEmpty((ent.Owner, itemSlots), args.Used, args.User, true))
            _gun.RefreshModifiers(ent.Owner);

        args.Handled = true;
    }

    private void OnItemSlotInsertAttemptEvent(Entity<UpgradeableGunComponent> ent, ref ItemSlotInsertAttemptEvent args)
    {
        // TODO: Figure out how to kill the interaction verbs bypassing checks, yet also allowing
        // for non-duplicate popups to the user when they interact without having to do all of this crap twice.
        if (!TryComp<GunUpgradeComponent>(args.Item, out var upgradeComp)
            || !TryComp<ItemSlotsComponent>(ent, out var itemSlots))
            return;

        var currentUpgrades = GetCurrentUpgrades(ent, itemSlots);
        var totalCapacityCost = currentUpgrades.Sum(upgrade => upgrade.Comp.CapacityCost);
        if (totalCapacityCost + upgradeComp.CapacityCost > ent.Comp.MaxUpgradeCapacity)
        {
            args.Cancelled = true;
            return;
        }

        var allowDupes = _config.GetCVar(CCVars.AllowDuplicatePkaModules) && !upgradeComp.Unique;
        var itemProto = MetaData(args.Item).EntityPrototype?.ID;
        foreach (var itemSlot in itemSlots.Slots.Values)
        {
            if (itemSlot is not { HasItem: true, Item: { } existingItem }
                || MetaData(existingItem).EntityPrototype?.ID != itemProto
                || allowDupes)
                continue;

            args.Cancelled = true;
            break;
        }
    }

    private void OnFireRateRefresh(Entity<GunUpgradeFireRateComponent> ent, ref GunRefreshModifiersEvent args)
    {
        args.FireRate *= ent.Comp.Coefficient;
        args.BurstFireRate *= ent.Comp.Coefficient;
        args.BurstCooldown /= ent.Comp.Coefficient;
    }

    private void OnFireRateRefreshRecharge(Entity<GunUpgradeFireRateComponent> ent, ref RechargeBasicEntityAmmoGetCooldownModifiersEvent args)
    {
        args.Multiplier /= ent.Comp.Coefficient;
    }

    private void OnCompsRefresh(Entity<GunComponentUpgradeComponent> ent, ref GunRefreshModifiersEvent args)
    {
        EntityManager.AddComponents(args.Gun, ent.Comp.Components);
    }

    private void OnSpeedRefresh(Entity<GunUpgradeSpeedComponent> ent, ref GunRefreshModifiersEvent args)
    {
        args.ProjectileSpeed *= ent.Comp.Coefficient;
    }

    private void OnDamageGunShotComps(Entity<GunUpgradeComponentsComponent> ent, ref GunShotEvent args)
    {
        foreach (var (ammo, _) in args.Ammo)
        {
            if (HasComp<ProjectileComponent>(ammo))
                EntityManager.AddComponents(ammo.Value, ent.Comp.Components);
        }
    }

    private void OnVampirismGunShot(Entity<GunUpgradeVampirismComponent> ent, ref GunShotEvent args)
    {
        foreach (var (ammo, _) in args.Ammo)
        {
            if (!HasComp<ProjectileComponent>(ammo))
                continue;

            var comp = EnsureComp<ProjectileVampirismComponent>(ammo.Value);
            comp.DamageOnHit = ent.Comp.DamageOnHit;
        }
    }

    private void OnVampirismProjectileHit(Entity<ProjectileVampirismComponent> ent, ref ProjectileHitEvent args)
    {
        if (!HasComp<MobStateComponent>(args.Target))
            return;
        _damage.TryChangeDamage(args.Shooter, ent.Comp.DamageOnHit);
    }

    public HashSet<Entity<GunUpgradeComponent>> GetCurrentUpgrades(Entity<UpgradeableGunComponent> ent, ItemSlotsComponent? itemSlots = null)
    {
        if (!Resolve(ent, ref itemSlots))
            return [];

        var upgrades = new HashSet<Entity<GunUpgradeComponent>>();

        foreach (var itemSlot in itemSlots.Slots.Values)
        {
            if (itemSlot is { HasItem: true, Item: { } item }
                && TryComp<GunUpgradeComponent>(item, out var upgradeComp))
                upgrades.Add((item, upgradeComp));
        }

        return upgrades;
    }
}
