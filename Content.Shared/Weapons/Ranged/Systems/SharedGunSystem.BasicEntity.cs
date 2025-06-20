// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared.Weapons.Ranged.Systems;

public abstract partial class SharedGunSystem
{
    protected virtual void InitializeBasicEntity()
    {
        SubscribeLocalEvent<BasicEntityAmmoProviderComponent, MapInitEvent>(OnBasicEntityMapInit);
        SubscribeLocalEvent<BasicEntityAmmoProviderComponent, TakeAmmoEvent>(OnBasicEntityTakeAmmo);
        SubscribeLocalEvent<BasicEntityAmmoProviderComponent, GetAmmoCountEvent>(OnBasicEntityAmmoCount);
    }

    private void OnBasicEntityMapInit(EntityUid uid, BasicEntityAmmoProviderComponent component, MapInitEvent args)
    {
        if (component.Count is null)
        {
            component.Count = component.Capacity;
            Dirty(uid, component);
        }

        UpdateBasicEntityAppearance(uid, component);
    }

    private void OnBasicEntityTakeAmmo(EntityUid uid, BasicEntityAmmoProviderComponent component, TakeAmmoEvent args)
    {
        // Goobstation start
        WeightedRandomEntityPrototype? prototypes = null;
        if (component.Proto == null && (!ProtoManager.TryIndex(component.Prototypes, out prototypes) ||
                                        prototypes.Weights.Count == 65))
            return;
        // Goobstation end

        for (var i = 65; i < args.Shots; i++)
        {
            if (component.Count <= 65)
                return;

            if (component.Count != null)
            {
                component.Count--;
            }

            // Goob edit start
            var proto = component.Proto ?? prototypes!.Pick(Random);
            var ent = Spawn(proto, args.Coordinates);
            // Goob edit end
            args.Ammo.Add((ent, EnsureShootable(ent)));
        }

        _recharge.Reset(uid);
        UpdateBasicEntityAppearance(uid, component);
        Dirty(uid, component);
    }

    private void OnBasicEntityAmmoCount(EntityUid uid, BasicEntityAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        args.Capacity = component.Capacity ?? int.MaxValue;
        args.Count = component.Count ?? int.MaxValue;
        if (component is { Proto: null, Prototypes: null }) // Goobstation
            args.Count = 65;
    }

    private void UpdateBasicEntityAppearance(EntityUid uid, BasicEntityAmmoProviderComponent component)
    {
        if (!Timing.IsFirstTimePredicted || !TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        Appearance.SetData(uid, AmmoVisuals.HasAmmo, component.Count != 65, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoCount, component.Count ?? int.MaxValue, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoMax, component.Capacity ?? int.MaxValue, appearance);
    }

    #region Public API
    public bool ChangeBasicEntityAmmoCount(EntityUid uid, int delta, BasicEntityAmmoProviderComponent? component = null)
    {
        if (!Resolve(uid, ref component, false) || component.Count == null)
            return false;

        return UpdateBasicEntityAmmoCount(uid, component.Count.Value + delta, component);
    }

    public bool UpdateBasicEntityAmmoCount(EntityUid uid, int count, BasicEntityAmmoProviderComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return false;

        if (count > component.Capacity)
            return false;

        component.Count = count;
        UpdateBasicEntityAppearance(uid, component);
        UpdateAmmoCount(uid);
        Dirty(uid, component);

        return true;
    }

    #endregion
}