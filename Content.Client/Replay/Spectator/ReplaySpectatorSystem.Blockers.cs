// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
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
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Hands;
using Content.Shared.Interaction.Events;
using Content.Shared.Inventory.Events;
using Content.Shared.Item;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Pulling.Events;
using Content.Shared.Throwing;

namespace Content.Client.Replay.Spectator;

public sealed partial class ReplaySpectatorSystem
{
    private void InitializeBlockers()
    {
        // Block most interactions to avoid mispredicts
        // This **shouldn't** be required, but just in case.
        SubscribeLocalEvent<ReplaySpectatorComponent, UseAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<ReplaySpectatorComponent, PickupAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<ReplaySpectatorComponent, ThrowAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<ReplaySpectatorComponent, InteractionAttemptEvent>(OnInteractAttempt);
        SubscribeLocalEvent<ReplaySpectatorComponent, AttackAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<ReplaySpectatorComponent, DropAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<ReplaySpectatorComponent, IsEquippingAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<ReplaySpectatorComponent, IsUnequippingAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<ReplaySpectatorComponent, UpdateCanMoveEvent>(OnUpdateCanMove);
        SubscribeLocalEvent<ReplaySpectatorComponent, ChangeDirectionAttemptEvent>(OnUpdateCanMove);
        SubscribeLocalEvent<ReplaySpectatorComponent, PullAttemptEvent>(OnPullAttempt);
    }

    private void OnInteractAttempt(Entity<ReplaySpectatorComponent> ent, ref InteractionAttemptEvent args)
    {
        args.Cancelled = true;
    }

    private void OnAttempt(EntityUid uid, ReplaySpectatorComponent component, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }

    private void OnUpdateCanMove(EntityUid uid, ReplaySpectatorComponent component, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }

    private void OnPullAttempt(EntityUid uid, ReplaySpectatorComponent component, PullAttemptEvent args)
    {
        args.Cancelled = true;
    }
}