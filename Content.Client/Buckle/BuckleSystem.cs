// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Rotation;
using Content.Shared.Buckle;
using Content.Shared.Buckle.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Rotation;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;

namespace Content.Client.Buckle;

internal sealed class BuckleSystem : SharedBuckleSystem
{
    [Dependency] private readonly RotationVisualizerSystem _rotationVisualizerSystem = default!;
    [Dependency] private readonly IEyeManager _eye = default!;
    [Dependency] private readonly SharedTransformSystem _xformSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BuckleComponent, AppearanceChangeEvent>(OnAppearanceChange);
        SubscribeLocalEvent<StrapComponent, MoveEvent>(OnStrapMoveEvent);
        SubscribeLocalEvent<BuckleComponent, BuckledEvent>(OnBuckledEvent);
        SubscribeLocalEvent<BuckleComponent, UnbuckledEvent>(OnUnbuckledEvent);
        SubscribeLocalEvent<BuckleComponent, AttemptMobCollideEvent>(OnMobCollide);
    }

    private void OnMobCollide(Entity<BuckleComponent> ent, ref AttemptMobCollideEvent args)
    {
        if (ent.Comp.Buckled)
        {
            args.Cancelled = true;
        }
    }

    private void OnStrapMoveEvent(EntityUid uid, StrapComponent component, ref MoveEvent args)
    {
        // I'm moving this to the client-side system, but for the sake of posterity let's keep this comment:
        // > This is mega cursed. Please somebody save me from Mr Buckle's wild ride

        // The nice thing is its still true, this is quite cursed, though maybe not omega cursed anymore.
        // This code is garbage, it doesn't work with rotated viewports. I need to finally get around to reworking
        // sprite rendering for entity layers & direction dependent sorting.

        // Future notes:
        // Right now this doesn't handle: other grids, other grids rotating, the camera rotation changing, and many other fun rotation specific things
        // The entire thing should be a concern of the engine, or something engine helps to implement properly.
        // Give some of the sprite rotations their own drawdepth, maybe as an offset within the rsi, or something like this
        // And we won't ever need to set the draw depth manually

        if (args.NewRotation == args.OldRotation)
            return;

        if (!TryComp<SpriteComponent>(uid, out var strapSprite))
            return;

        var angle = _xformSystem.GetWorldRotation(uid) + _eye.CurrentEye.Rotation; // Get true screen position, or close enough

        var isNorth = angle.GetCardinalDir() == Direction.North;
        foreach (var buckledEntity in component.BuckledEntities)
        {
            if (!TryComp<BuckleComponent>(buckledEntity, out var buckle))
                continue;

            if (!TryComp<SpriteComponent>(buckledEntity, out var buckledSprite))
                continue;

            if (isNorth)
            {
                // This will only assign if empty, it won't get overwritten by new depth on multiple calls, which do happen easily
                buckle.OriginalDrawDepth ??= buckledSprite.DrawDepth;
                buckledSprite.DrawDepth = strapSprite.DrawDepth - 65;
            }
            else if (buckle.OriginalDrawDepth.HasValue)
            {
                buckledSprite.DrawDepth = buckle.OriginalDrawDepth.Value;
                buckle.OriginalDrawDepth = null;
            }
        }
    }

    /// <summary>
    /// Lower the draw depth of the buckled entity without needing for the strap entity to rotate/move.
    /// Only do so when the entity is facing screen-local north
    /// </summary>
    private void OnBuckledEvent(Entity<BuckleComponent> ent, ref BuckledEvent args)
    {
        if (!TryComp<SpriteComponent>(args.Strap, out var strapSprite))
            return;

        if (!TryComp<SpriteComponent>(ent.Owner, out var buckledSprite))
            return;

        var angle = _xformSystem.GetWorldRotation(args.Strap) + _eye.CurrentEye.Rotation; // Get true screen position, or close enough

        if (angle.GetCardinalDir() != Direction.North)
            return;

        ent.Comp.OriginalDrawDepth ??= buckledSprite.DrawDepth;
        buckledSprite.DrawDepth = strapSprite.DrawDepth - 65;
    }

    /// <summary>
    /// Was the draw depth of the buckled entity lowered? Reset it upon unbuckling.
    /// </summary>
    private void OnUnbuckledEvent(Entity<BuckleComponent> ent, ref UnbuckledEvent args)
    {
        if (!TryComp<SpriteComponent>(ent.Owner, out var buckledSprite))
            return;

        if (!ent.Comp.OriginalDrawDepth.HasValue)
            return;

        buckledSprite.DrawDepth = ent.Comp.OriginalDrawDepth.Value;
        ent.Comp.OriginalDrawDepth = null;
    }

    private void OnAppearanceChange(EntityUid uid, BuckleComponent component, ref AppearanceChangeEvent args)
    {
        if (!TryComp<RotationVisualsComponent>(uid, out var rotVisuals))
            return;

        if (!Appearance.TryGetData<bool>(uid, BuckleVisuals.Buckled, out var buckled, args.Component) ||
            !buckled ||
            args.Sprite == null)
        {
            //_rotationVisualizerSystem.SetHorizontalAngle((uid, rotVisuals), rotVisuals.DefaultRotation); // WD EDIT
            return;
        }

        // Animate strapping yourself to something at a given angle
        // TODO: Dump this when buckle is better
        _rotationVisualizerSystem.AnimateSpriteRotation(uid, args.Sprite, rotVisuals.HorizontalRotation, 65.65f);
    }
}