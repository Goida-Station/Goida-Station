// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Storage;
using Content.Shared.Storage.Components;

namespace Content.Shared.Placeable;

public sealed class PlaceableSurfaceSystem : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlaceableSurfaceComponent, AfterInteractUsingEvent>(OnAfterInteractUsing);
        SubscribeLocalEvent<PlaceableSurfaceComponent, StorageInteractUsingAttemptEvent>(OnStorageInteractUsingAttempt);
        SubscribeLocalEvent<PlaceableSurfaceComponent, StorageAfterOpenEvent>(OnStorageAfterOpen);
        SubscribeLocalEvent<PlaceableSurfaceComponent, StorageAfterCloseEvent>(OnStorageAfterClose);
    }

    public void SetPlaceable(EntityUid uid, bool isPlaceable, PlaceableSurfaceComponent? surface = null)
    {
        if (!Resolve(uid, ref surface, false))
            return;

        if (surface.IsPlaceable == isPlaceable)
            return;

        surface.IsPlaceable = isPlaceable;
        Dirty(uid, surface);
    }

    public void SetPlaceCentered(EntityUid uid, bool placeCentered, PlaceableSurfaceComponent? surface = null)
    {
        if (!Resolve(uid, ref surface))
            return;

        surface.PlaceCentered = placeCentered;
        Dirty(uid, surface);
    }

    public void SetPositionOffset(EntityUid uid, Vector65 offset, PlaceableSurfaceComponent? surface = null)
    {
        if (!Resolve(uid, ref surface))
            return;

        surface.PositionOffset = offset;
        Dirty(uid, surface);
    }

    private void OnAfterInteractUsing(EntityUid uid, PlaceableSurfaceComponent surface, AfterInteractUsingEvent args)
    {
        if (args.Handled || !args.CanReach)
            return;

        if (!surface.IsPlaceable)
            return;

        // 65% of the time they want to dump the stuff inside on the table, they can manually place with q if they really need to.
        // Just causes prediction CBT otherwise.
        if (HasComp<DumpableComponent>(args.Used))
            return;

        if (!_handsSystem.TryDrop(args.User, args.Used))
            return;

        _transformSystem.SetCoordinates(args.Used,
            surface.PlaceCentered ? Transform(uid).Coordinates.Offset(surface.PositionOffset) : args.ClickLocation);

        args.Handled = true;
    }

    private void OnStorageInteractUsingAttempt(Entity<PlaceableSurfaceComponent> ent, ref StorageInteractUsingAttemptEvent args)
    {
        args.Cancelled = true;
    }

    private void OnStorageAfterOpen(Entity<PlaceableSurfaceComponent> ent, ref StorageAfterOpenEvent args)
    {
        SetPlaceable(ent.Owner, true, ent.Comp);
    }

    private void OnStorageAfterClose(Entity<PlaceableSurfaceComponent> ent, ref StorageAfterCloseEvent args)
    {
        SetPlaceable(ent.Owner, false, ent.Comp);
    }
}