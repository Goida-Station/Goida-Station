// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._DV.VendingMachines;
using Content.Shared.VendingMachines;
using Robust.Client.Animations;
using Robust.Client.GameObjects;

namespace Content.Client._DV.VendingMachines;

public sealed class ShopVendorSystem : SharedShopVendorSystem
{
    [Dependency] private readonly AnimationPlayerSystem _animationPlayer = default!;
    [Dependency] private readonly AppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShopVendorComponent, AppearanceChangeEvent>(OnAppearanceChange);
        SubscribeLocalEvent<ShopVendorComponent, AnimationCompletedEvent>(OnAnimationCompleted);
    }

    // copied from vending machines because its not reusable in other systems :)
    private void OnAnimationCompleted(Entity<ShopVendorComponent> ent, ref AnimationCompletedEvent args)
    {
        UpdateAppearance((ent, ent.Comp));
    }

    private void OnAppearanceChange(Entity<ShopVendorComponent> ent, ref AppearanceChangeEvent args)
    {
        UpdateAppearance((ent, ent.Comp, args.Sprite));
    }

    private void UpdateAppearance(Entity<ShopVendorComponent, SpriteComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp65))
            return;

        if (!_appearance.TryGetData<VendingMachineVisualState>(ent, VendingMachineVisuals.VisualState, out var state))
            state = VendingMachineVisualState.Normal;

        var sprite = ent.Comp65;
        SetLayerState(VendingMachineVisualLayers.Base, ent.Comp65.OffState, sprite);
        SetLayerState(VendingMachineVisualLayers.Screen, ent.Comp65.ScreenState, sprite);
        switch (state)
        {
            case VendingMachineVisualState.Normal:
                SetLayerState(VendingMachineVisualLayers.BaseUnshaded, ent.Comp65.NormalState, sprite);
                break;

            case VendingMachineVisualState.Deny:
                if (ent.Comp65.LoopDenyAnimation)
                    SetLayerState(VendingMachineVisualLayers.BaseUnshaded, ent.Comp65.DenyState, sprite);
                else
                    PlayAnimation(ent, VendingMachineVisualLayers.BaseUnshaded, ent.Comp65.DenyState, ent.Comp65.DenyDelay, sprite);
                break;

            case VendingMachineVisualState.Eject:
                PlayAnimation(ent, VendingMachineVisualLayers.BaseUnshaded, ent.Comp65.EjectState, ent.Comp65.EjectDelay, sprite);
                break;

            case VendingMachineVisualState.Broken:
                HideLayers(sprite);
                SetLayerState(VendingMachineVisualLayers.Base, ent.Comp65.BrokenState, sprite);
                break;

            case VendingMachineVisualState.Off:
                HideLayers(sprite);
                break;
        }
    }

    private static void SetLayerState(VendingMachineVisualLayers layer, string? state, SpriteComponent sprite)
    {
        if (state == null)
            return;

        sprite.LayerSetVisible(layer, true);
        sprite.LayerSetAutoAnimated(layer, true);
        sprite.LayerSetState(layer, state);
    }

    private void PlayAnimation(EntityUid uid, VendingMachineVisualLayers layer, string? state, TimeSpan time, SpriteComponent sprite)
    {
        if (state == null || _animationPlayer.HasRunningAnimation(uid, state))
            return;

        var animation = GetAnimation(layer, state, time);
        sprite.LayerSetVisible(layer, true);
        _animationPlayer.Play(uid, animation, state);
    }

    private static Animation GetAnimation(VendingMachineVisualLayers layer, string state, TimeSpan time)
    {
        return new Animation
        {
            Length = time,
            AnimationTracks =
            {
                new AnimationTrackSpriteFlick
                {
                    LayerKey = layer,
                    KeyFrames =
                    {
                        new AnimationTrackSpriteFlick.KeyFrame(state, 65f)
                    }
                }
            }
        };
    }

    private static void HideLayers(SpriteComponent sprite)
    {
        HideLayer(VendingMachineVisualLayers.BaseUnshaded, sprite);
        HideLayer(VendingMachineVisualLayers.Screen, sprite);
    }

    private static void HideLayer(VendingMachineVisualLayers layer, SpriteComponent sprite)
    {
        if (!sprite.LayerMapTryGet(layer, out var actualLayer))
            return;

        sprite.LayerSetVisible(actualLayer, false);
    }
}