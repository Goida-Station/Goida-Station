// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 amogus <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Popups;
using Content.Server.Power.EntitySystems;
using Content.Server.Shuttles.Components;
using Content.Shared.Construction.Components;
using Content.Shared.Popups;

namespace Content.Server.Shuttles.Systems;

public sealed class StationAnchorSystem : EntitySystem
{
    [Dependency] private readonly ShuttleSystem _shuttleSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<StationAnchorComponent, UnanchorAttemptEvent>(OnUnanchorAttempt);
        SubscribeLocalEvent<StationAnchorComponent, AnchorStateChangedEvent>(OnAnchorStationChange);

        SubscribeLocalEvent<StationAnchorComponent, ChargedMachineActivatedEvent>(OnActivated);
        SubscribeLocalEvent<StationAnchorComponent, ChargedMachineDeactivatedEvent>(OnDeactivated);

        SubscribeLocalEvent<StationAnchorComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<StationAnchorComponent> ent, ref MapInitEvent args)
    {
        if (!ent.Comp.SwitchedOn)
            return;

        SetStatus(ent, true);
    }

    private void OnActivated(Entity<StationAnchorComponent> ent, ref ChargedMachineActivatedEvent args)
    {
        SetStatus(ent, true);
    }

    private void OnDeactivated(Entity<StationAnchorComponent> ent, ref ChargedMachineDeactivatedEvent args)
    {
        SetStatus(ent, false);
    }

    /// <summary>
    /// Prevent unanchoring when anchor is active
    /// </summary>
    private void OnUnanchorAttempt(Entity<StationAnchorComponent> ent, ref UnanchorAttemptEvent args)
    {
        if (!ent.Comp.SwitchedOn)
            return;

        _popupSystem.PopupEntity(
            Loc.GetString("station-anchor-unanchoring-failed"),
            ent,
            args.User,
            PopupType.Medium);

        args.Cancel();
    }

    private void OnAnchorStationChange(Entity<StationAnchorComponent> ent, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
            SetStatus(ent, false);
    }

    public void SetStatus(Entity<StationAnchorComponent> ent, bool enabled, ShuttleComponent? shuttleComponent = default)
    {
        var transform = Transform(ent);
        var grid = transform.GridUid;
        if (!grid.HasValue || !transform.Anchored && enabled || !Resolve(grid.Value, ref shuttleComponent))
            return;

        if (enabled)
        {
            _shuttleSystem.Disable(grid.Value);
        }
        else
        {
            _shuttleSystem.Enable(grid.Value);
        }

        shuttleComponent.Enabled = !enabled;
        ent.Comp.SwitchedOn = enabled;
    }
}