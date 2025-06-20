// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 c65llv65e <65c65llv65e@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Items;
using Content.Client.Radiation.UI;
using Content.Shared.Radiation.Components;
using Content.Shared.Radiation.Systems;

namespace Content.Client.Radiation.Systems;

public sealed class GeigerSystem : SharedGeigerSystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GeigerComponent, AfterAutoHandleStateEvent>(OnHandleState);
        Subs.ItemStatus<GeigerComponent>(ent => ent.Comp.ShowControl ? new GeigerItemControl(ent) : null);
    }

    private void OnHandleState(EntityUid uid, GeigerComponent component, ref AfterAutoHandleStateEvent args)
    {
        component.UiUpdateNeeded = true;
    }
}