// SPDX-FileCopyrightText: 65 Slava65 <super.novalskiy_65@inbox.ru>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kirus65 <65Kirus65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Krunklehorn <65Krunklehorn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Overlays;
using Content.Shared.StatusIcon.Components;

namespace Content.Client.Overlays;

public sealed class ShowThirstIconsSystem : EquipmentHudSystem<ShowThirstIconsComponent>
{
    [Dependency] private readonly ThirstSystem _thirst = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ThirstComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
    }

    private void OnGetStatusIconsEvent(EntityUid uid, ThirstComponent component, ref GetStatusIconsEvent ev)
    {
        if (!IsActive)
            return;

        if (_thirst.TryGetStatusIconPrototype(component, out var iconPrototype))
            ev.StatusIcons.Add(iconPrototype);
    }
}