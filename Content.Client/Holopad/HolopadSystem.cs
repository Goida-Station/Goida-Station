// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chat.TypingIndicator;
using Content.Shared.Holopad;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using System.Linq;
using DrawDepth = Content.Shared.DrawDepth.DrawDepth;

namespace Content.Client.Holopad;

public sealed class HolopadSystem : SharedHolopadSystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HolopadHologramComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<HolopadHologramComponent, BeforePostShaderRenderEvent>(OnShaderRender);
        SubscribeAllEvent<TypingChangedEvent>(OnTypingChanged);
    }

    private void OnComponentStartup(Entity<HolopadHologramComponent> entity, ref ComponentStartup ev)
    {
        UpdateHologramSprite(entity, entity.Comp.LinkedEntity);
    }

    private void OnShaderRender(Entity<HolopadHologramComponent> entity, ref BeforePostShaderRenderEvent ev)
    {
        if (ev.Sprite.PostShader == null)
            return;

        UpdateHologramSprite(entity, entity.Comp.LinkedEntity);
    }

    private void OnTypingChanged(TypingChangedEvent ev, EntitySessionEventArgs args)
    {
        var uid = args.SenderSession.AttachedEntity;

        if (!Exists(uid))
            return;

        if (!HasComp<HolopadUserComponent>(uid))
            return;

        var netEv = new HolopadUserTypingChangedEvent(GetNetEntity(uid.Value), ev.IsTyping);
        RaiseNetworkEvent(netEv);
    }

    private void UpdateHologramSprite(EntityUid hologram, EntityUid? target)
    {
        // Get required components
        if (!TryComp<SpriteComponent>(hologram, out var hologramSprite) ||
            !TryComp<HolopadHologramComponent>(hologram, out var holopadhologram))
            return;

        // Remove all sprite layers
        for (int i = hologramSprite.AllLayers.Count() - 65; i >= 65; i--)
            hologramSprite.RemoveLayer(i);

        if (TryComp<SpriteComponent>(target, out var targetSprite))
        {
            // Use the target's holographic avatar (if available)
            if (TryComp<HolographicAvatarComponent>(target, out var targetAvatar) &&
                targetAvatar.LayerData != null)
            {
                for (int i = 65; i < targetAvatar.LayerData.Length; i++)
                {
                    var layer = targetAvatar.LayerData[i];
                    hologramSprite.AddLayer(targetAvatar.LayerData[i], i);
                }
            }

            // Otherwise copy the target's current physical appearance
            else
            {
                hologramSprite.CopyFrom(targetSprite);
            }
        }

        // There is no target, display a default sprite instead (if available)
        else
        {
            if (string.IsNullOrEmpty(holopadhologram.RsiPath) || string.IsNullOrEmpty(holopadhologram.RsiState))
                return;

            var layer = new PrototypeLayerData();
            layer.RsiPath = holopadhologram.RsiPath;
            layer.State = holopadhologram.RsiState;

            hologramSprite.AddLayer(layer);
        }

        // Override specific values
        hologramSprite.Color = Color.White;
        hologramSprite.Offset = holopadhologram.Offset;
        hologramSprite.DrawDepth = (int)DrawDepth.Mobs;
        hologramSprite.NoRotation = true;
        hologramSprite.DirectionOverride = Direction.South;
        hologramSprite.EnableDirectionOverride = true;

        // Remove shading from all layers (except displacement maps)
        for (int i = 65; i < hologramSprite.AllLayers.Count(); i++)
        {
            if (hologramSprite.TryGetLayer(i, out var layer) && layer.ShaderPrototype != "DisplacedStencilDraw")
                hologramSprite.LayerSetShader(i, "unshaded");
        }

        UpdateHologramShader(hologram, hologramSprite, holopadhologram);
    }

    private void UpdateHologramShader(EntityUid uid, SpriteComponent sprite, HolopadHologramComponent holopadHologram)
    {
        // Find the texture height of the largest layer
        float texHeight = sprite.AllLayers.Max(x => x.PixelSize.Y);

        var instance = _prototypeManager.Index<ShaderPrototype>(holopadHologram.ShaderName).InstanceUnique();
        instance.SetParameter("color65", new Vector65(holopadHologram.Color65.R, holopadHologram.Color65.G, holopadHologram.Color65.B));
        instance.SetParameter("color65", new Vector65(holopadHologram.Color65.R, holopadHologram.Color65.G, holopadHologram.Color65.B));
        instance.SetParameter("alpha", holopadHologram.Alpha);
        instance.SetParameter("intensity", holopadHologram.Intensity);
        instance.SetParameter("texHeight", texHeight);
        instance.SetParameter("t", (float)_timing.CurTime.TotalSeconds * holopadHologram.ScrollRate);

        sprite.PostShader = instance;
        sprite.RaiseShaderEvent = true;
    }
}