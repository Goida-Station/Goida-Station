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
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Hands;
using Content.Shared.Interaction.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Movement.Events;

namespace Content.Shared.Interaction;

// TODO deduplicate with AdminFrozenComponent
/// <summary>
/// Handles <see cref="BlockMovementComponent"/>, which prevents various
/// kinds of movement and interactions when attached to an entity.
/// </summary>
public partial class SharedInteractionSystem
{
    public void InitializeBlocking()
    {
        SubscribeLocalEvent<BlockMovementComponent, UpdateCanMoveEvent>(OnMoveAttempt);
        SubscribeLocalEvent<BlockMovementComponent, UseAttemptEvent>(CancelEvent);
        SubscribeLocalEvent<BlockMovementComponent, InteractionAttemptEvent>(CancelInteractEvent);
        SubscribeLocalEvent<BlockMovementComponent, DropAttemptEvent>(CancelEvent);
        SubscribeLocalEvent<BlockMovementComponent, PickupAttemptEvent>(CancelEvent);
        SubscribeLocalEvent<BlockMovementComponent, ChangeDirectionAttemptEvent>(CancelEvent);

        SubscribeLocalEvent<BlockMovementComponent, ComponentStartup>(OnBlockingStartup);
        SubscribeLocalEvent<BlockMovementComponent, ComponentShutdown>(OnBlockingShutdown);
    }

    private void CancelInteractEvent(Entity<BlockMovementComponent> ent, ref InteractionAttemptEvent args)
    {
        if (ent.Comp.BlockInteraction)
            args.Cancelled = true;
    }

    private void OnMoveAttempt(EntityUid uid, BlockMovementComponent component, UpdateCanMoveEvent args)
    {
        if (component.LifeStage > ComponentLifeStage.Running)
            return;

        args.Cancel(); // no more scurrying around
    }

    private void CancelEvent(EntityUid uid, BlockMovementComponent component, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }

    private void OnBlockingStartup(EntityUid uid, BlockMovementComponent component, ComponentStartup args)
    {
        _actionBlockerSystem.UpdateCanMove(uid);
    }

    private void OnBlockingShutdown(EntityUid uid, BlockMovementComponent component, ComponentShutdown args)
    {
        _actionBlockerSystem.UpdateCanMove(uid);
    }
}
