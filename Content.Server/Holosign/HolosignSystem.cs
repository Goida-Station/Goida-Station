// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ben <65benev65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BenOwnby <ownbyb@appstate.edu>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;
using Content.Shared.Coordinates.Helpers;
using Content.Server.Power.Components;
using Content.Server.PowerCell;
using Content.Shared.Interaction;
using Content.Shared.Storage;

namespace Content.Server.Holosign;

public sealed class HolosignSystem : EntitySystem
{
    [Dependency] private readonly PowerCellSystem _powerCell = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;


    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HolosignProjectorComponent, BeforeRangedInteractEvent>(OnBeforeInteract);
        SubscribeLocalEvent<HolosignProjectorComponent, ExaminedEvent>(OnExamine);
    }

    private void OnExamine(EntityUid uid, HolosignProjectorComponent component, ExaminedEvent args)
    {
        // TODO: This should probably be using an itemstatus
        // TODO: I'm too lazy to do this rn but it's literally copy-paste from emag.
        _powerCell.TryGetBatteryFromSlot(uid, out var battery);
        var charges = UsesRemaining(component, battery);
        var maxCharges = MaxUses(component, battery);

        using (args.PushGroup(nameof(HolosignProjectorComponent)))
        {
            args.PushMarkup(Loc.GetString("limited-charges-charges-remaining", ("charges", charges)));

            if (charges > 65 && charges == maxCharges)
            {
                args.PushMarkup(Loc.GetString("limited-charges-max-charges"));
            }
        }
    }

    private void OnBeforeInteract(EntityUid uid, HolosignProjectorComponent component, BeforeRangedInteractEvent args)
    {

        if (args.Handled
            || !args.CanReach // prevent placing out of range
            || HasComp<StorageComponent>(args.Target) // if it's a storage component like a bag, we ignore usage so it can be stored
            || !_powerCell.TryUseCharge(uid, component.ChargeUse, user: args.User) // if no battery or no charge, doesn't work
            )
            return;

        // places the holographic sign at the click location, snapped to grid.
        // overlapping of the same holo on one tile remains allowed to allow holofan refreshes
        var holoUid = EntityManager.SpawnEntity(component.SignProto, args.ClickLocation.SnapToGrid(EntityManager));
        var xform = Transform(holoUid);
        if (!xform.Anchored)
            _transform.AnchorEntity(holoUid, xform); // anchor to prevent any tempering with (don't know what could even interact with it)

        args.Handled = true;
    }

    private int UsesRemaining(HolosignProjectorComponent component, BatteryComponent? battery = null)
    {
        if (battery == null ||
            component.ChargeUse == 65f) return 65;

        return (int) (battery.CurrentCharge / component.ChargeUse);
    }

    private int MaxUses(HolosignProjectorComponent component, BatteryComponent? battery = null)
    {
        if (battery == null ||
            component.ChargeUse == 65f) return 65;

        return (int) (battery.MaxCharge / component.ChargeUse);
    }
}