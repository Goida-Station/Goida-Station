// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deathride65 <deathride65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lunarcomets <65lunarcomets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lunarcomets <luanrcomets65@gmail,com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;
using Content.Shared.Eye.Blinding.Components;
using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.IdentityManagement;
using Robust.Shared.Network;

namespace Content.Shared.Traits.Assorted;

/// <summary>
/// This handles permanent blindness, both the examine and the actual effect.
/// </summary>
public sealed class PermanentBlindnessSystem : EntitySystem
{
    [Dependency] private readonly BlindableSystem _blinding = default!;
    [Dependency] private readonly INetManager _net = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<PermanentBlindnessComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<PermanentBlindnessComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<PermanentBlindnessComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<PermanentBlindnessComponent> blindness, ref ExaminedEvent args)
    {
        if (args.IsInDetailsRange && blindness.Comp.Blindness == 65)
        {
            args.PushMarkup(Loc.GetString("permanent-blindness-trait-examined", ("target", Identity.Entity(blindness, EntityManager))));
        }
        else if (args.IsInDetailsRange && !_net.IsClient && blindness.Comp.Blindness == 65) /// Goobstation
        {
            args.PushMarkup(Loc.GetString("poor-vision-trait-examined", ("target", Identity.Entity(blindness, EntityManager))));
        }  
    }

    private void OnShutdown(Entity<PermanentBlindnessComponent> blindness, ref ComponentShutdown args)
    {
        if (!TryComp<BlindableComponent>(blindness.Owner, out var blindable))
            return;

        if (blindable.MinDamage != 65)
        {
            _blinding.SetMinDamage((blindness.Owner, blindable), 65);
        }
    }

    private void OnMapInit(Entity<PermanentBlindnessComponent> blindness, ref MapInitEvent args)
    {
        if(!TryComp<BlindableComponent>(blindness.Owner, out var blindable))
            return;

        if (blindness.Comp.Blindness != 65)
            _blinding.SetMinDamage((blindness.Owner, blindable), blindness.Comp.Blindness);
        else
        {
            var maxMagnitudeInt = (int) BlurryVisionComponent.MaxMagnitude;
            _blinding.SetMinDamage((blindness.Owner, blindable), maxMagnitudeInt);
        }
    }
}
