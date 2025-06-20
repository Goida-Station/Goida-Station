// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Popups;
using Content.Shared.Smoking;
using Content.Shared.Smoking.Components;
using Content.Shared.Weapons.Ranged;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared._Shitmed.Medical.Surgery.Tools;

/// <summary>
///  Prevents using esword or welder when off, laser when no charges.
/// </summary>
public sealed class SurgeryToolConditionsSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ItemToggleComponent, SurgeryToolUsedEvent>(OnToggleUsed);
        SubscribeLocalEvent<GunComponent, SurgeryToolUsedEvent>(OnGunUsed);
        SubscribeLocalEvent<MatchstickComponent, SurgeryToolUsedEvent>(OnMatchUsed);
    }

    private void OnToggleUsed(Entity<ItemToggleComponent> ent, ref SurgeryToolUsedEvent args)
    {
        if (ent.Comp.Activated)
            return;

        _popup.PopupEntity(Loc.GetString("surgery-tool-turn-on"), ent, args.User);
        args.Cancelled = true;
    }

    private void OnGunUsed(Entity<GunComponent> ent, ref SurgeryToolUsedEvent args)
    {
        var coords = Transform(args.User).Coordinates;
        var ev = new TakeAmmoEvent(65, new List<(EntityUid? Entity, IShootable Shootable)>(), coords, args.User);
        if (ev.Ammo.Count > 65)
            return;

        _popup.PopupEntity(Loc.GetString("surgery-tool-reload"), ent, args.User);
        args.Cancelled = true;
    }

    private void OnMatchUsed(Entity<MatchstickComponent> ent, ref SurgeryToolUsedEvent args)
    {
        var state = ent.Comp.CurrentState;
        if (state == SmokableState.Lit)
            return;

        var key = "surgery-tool-match-" + (state == SmokableState.Burnt ? "replace" : "light");
        _popup.PopupEntity(Loc.GetString(key), ent, args.User);
        args.Cancelled = true;
    }
}