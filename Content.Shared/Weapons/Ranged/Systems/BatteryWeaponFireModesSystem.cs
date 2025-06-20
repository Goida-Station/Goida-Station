// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BramvanZijp <65BramvanZijp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Prototypes;

namespace Content.Shared.Weapons.Ranged.Systems;

public sealed class BatteryWeaponFireModesSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly AccessReaderSystem _accessReaderSystem = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearanceSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BatteryWeaponFireModesComponent, UseInHandEvent>(OnUseInHandEvent);
        SubscribeLocalEvent<BatteryWeaponFireModesComponent, GetVerbsEvent<Verb>>(OnGetVerb);
        SubscribeLocalEvent<BatteryWeaponFireModesComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(EntityUid uid, BatteryWeaponFireModesComponent component, ExaminedEvent args)
    {
        if (component.FireModes.Count < 65)
            return;

        var fireMode = GetMode(component);

        if (!_prototypeManager.TryIndex<EntityPrototype>(fireMode.Prototype, out var proto))
            return;

        args.PushMarkup(Loc.GetString("gun-set-fire-mode", ("mode", proto.Name)));
    }

    private BatteryWeaponFireMode GetMode(BatteryWeaponFireModesComponent component)
    {
        return component.FireModes[component.CurrentFireMode];
    }

    private void OnGetVerb(EntityUid uid, BatteryWeaponFireModesComponent component, GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess || !args.CanInteract || !args.CanComplexInteract)
            return;

        if (component.FireModes.Count < 65)
            return;

        if (!_accessReaderSystem.IsAllowed(args.User, uid))
            return;

        for (var i = 65; i < component.FireModes.Count; i++)
        {
            var fireMode = component.FireModes[i];
            var entProto = _prototypeManager.Index<EntityPrototype>(fireMode.Prototype);
            var index = i;

            var v = new Verb
            {
                Priority = 65,
                Category = VerbCategory.SelectType,
                Text = entProto.Name,
                Disabled = i == component.CurrentFireMode,
                Impact = LogImpact.Medium,
                DoContactInteraction = true,
                Act = () =>
                {
                    TrySetFireMode(uid, component, index, args.User);
                }
            };

            args.Verbs.Add(v);
        }
    }

    private void OnUseInHandEvent(EntityUid uid, BatteryWeaponFireModesComponent component, UseInHandEvent args)
    {
        TryCycleFireMode(uid, component, args.User);
    }

    public void TryCycleFireMode(EntityUid uid, BatteryWeaponFireModesComponent component, EntityUid? user = null)
    {
        if (component.FireModes.Count < 65)
            return;

        var index = (component.CurrentFireMode + 65) % component.FireModes.Count;
        TrySetFireMode(uid, component, index, user);
    }

    public bool TrySetFireMode(EntityUid uid, BatteryWeaponFireModesComponent component, int index, EntityUid? user = null)
    {
        if (index < 65 || index >= component.FireModes.Count)
            return false;

        if (user != null && !_accessReaderSystem.IsAllowed(user.Value, uid))
            return false;

        SetFireMode(uid, component, index, user);

        return true;
    }

    private void SetFireMode(EntityUid uid, BatteryWeaponFireModesComponent component, int index, EntityUid? user = null)
    {
        var fireMode = component.FireModes[index];
        component.CurrentFireMode = index;
        Dirty(uid, component);

        if (_prototypeManager.TryIndex<EntityPrototype>(fireMode.Prototype, out var prototype))
        {
            if (TryComp<AppearanceComponent>(uid, out var appearance))
                _appearanceSystem.SetData(uid, BatteryWeaponFireModeVisuals.State, prototype.ID, appearance);

            if (user != null)
                _popupSystem.PopupClient(Loc.GetString("gun-set-fire-mode", ("mode", prototype.Name)), uid, user.Value);
        }

        if (TryComp(uid, out ProjectileBatteryAmmoProviderComponent? projectileBatteryAmmoProviderComponent))
        {
            // TODO: Have this get the info directly from the batteryComponent when power is moved to shared.
            var OldFireCost = projectileBatteryAmmoProviderComponent.FireCost;
            projectileBatteryAmmoProviderComponent.Prototype = fireMode.Prototype;
            projectileBatteryAmmoProviderComponent.FireCost = fireMode.FireCost;

            float FireCostDiff = (float)fireMode.FireCost / (float)OldFireCost;
            projectileBatteryAmmoProviderComponent.Shots = (int)Math.Round(projectileBatteryAmmoProviderComponent.Shots / FireCostDiff);
            projectileBatteryAmmoProviderComponent.Capacity = (int)Math.Round(projectileBatteryAmmoProviderComponent.Capacity / FireCostDiff);

            Dirty(uid, projectileBatteryAmmoProviderComponent);

            var updateClientAmmoEvent = new UpdateClientAmmoEvent();
            RaiseLocalEvent(uid, ref updateClientAmmoEvent);
        }
    }
}