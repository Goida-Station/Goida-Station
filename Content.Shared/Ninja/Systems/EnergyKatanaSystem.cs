// SPDX-FileCopyrightText: 65 deltanedas <deltanedas@laptop>
// SPDX-FileCopyrightText: 65 deltanedas <user@zenith>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Inventory.Events;
using Content.Shared.Ninja.Components;

namespace Content.Shared.Ninja.Systems;

/// <summary>
/// System for katana binding and dash events. Recalling is handled by the suit.
/// </summary>
public sealed class EnergyKatanaSystem : EntitySystem
{
    [Dependency] private readonly SharedSpaceNinjaSystem _ninja = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EnergyKatanaComponent, GotEquippedEvent>(OnEquipped);
        SubscribeLocalEvent<EnergyKatanaComponent, CheckDashEvent>(OnCheckDash);
    }

    /// <summary>
    /// When equipped by a ninja, try to bind it.
    /// </summary>
    private void OnEquipped(Entity<EnergyKatanaComponent> ent, ref GotEquippedEvent args)
    {
        _ninja.BindKatana(args.Equipee, ent);
    }

    private void OnCheckDash(Entity<EnergyKatanaComponent> ent, ref CheckDashEvent args)
    {
        // Just use a whitelist fam
        if (!_ninja.IsNinja(args.User))
            args.Cancelled = true;
    }
}