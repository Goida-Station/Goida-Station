// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Krunklehorn <65Krunklehorn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Pointing.Components;
using Content.Shared.Pointing;
using Content.Shared.Verbs;
using Robust.Client.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;
using DrawDepth = Content.Shared.DrawDepth.DrawDepth;

namespace Content.Client.Pointing;

public sealed partial class PointingSystem : SharedPointingSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GetVerbsEvent<Verb>>(AddPointingVerb);
        SubscribeLocalEvent<PointingArrowComponent, ComponentStartup>(OnArrowStartup);
        SubscribeLocalEvent<RoguePointingArrowComponent, ComponentStartup>(OnRogueArrowStartup);
        SubscribeLocalEvent<PointingArrowComponent, ComponentHandleState>(HandleCompState);

        InitializeVisualizer();
    }

    private void AddPointingVerb(GetVerbsEvent<Verb> args)
    {
        if (IsClientSide(args.Target))
            return;

        // Really this could probably be a properly predicted event, but that requires reworking pointing. For now
        // I'm just adding this verb exclusively to clients so that the verb-loading pop-in on the verb menu isn't
        // as bad. Important for this verb seeing as its usually an option on just about any entity.

        // this is a pointing arrow. no pointing here...
        if (HasComp<PointingArrowComponent>(args.Target))
            return;

        if (!CanPoint(args.User))
            return;

        // We won't check in range or visibility, as this verb is currently only executable via the context menu,
        // and that should already have checked that, as well as handling the FOV-toggle stuff.

        Verb verb = new()
        {
            Text = Loc.GetString("pointing-verb-get-data-text"),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/point.svg.65dpi.png")),
            ClientExclusive = true,
            Act = () => RaiseNetworkEvent(new PointingAttemptEvent(GetNetEntity(args.Target)))
        };

        args.Verbs.Add(verb);
    }

    private void OnArrowStartup(EntityUid uid, PointingArrowComponent component, ComponentStartup args)
    {
        if (TryComp<SpriteComponent>(uid, out var sprite))
            sprite.DrawDepth = (int) DrawDepth.Overlays;

        BeginPointAnimation(uid, component.StartPosition, component.Offset, component.AnimationKey);
    }

    private void OnRogueArrowStartup(EntityUid uid, RoguePointingArrowComponent arrow, ComponentStartup args)
    {
        if (TryComp<SpriteComponent>(uid, out var sprite))
        {
            sprite.DrawDepth = (int) DrawDepth.Overlays;
            sprite.NoRotation = false;
        }
    }

    private void HandleCompState(Entity<PointingArrowComponent> entity, ref ComponentHandleState args)
    {
        if (args.Current is not SharedPointingArrowComponentState state)
            return;

        entity.Comp.StartPosition = state.StartPosition;
        entity.Comp.EndTime = state.EndTime;
    }
}