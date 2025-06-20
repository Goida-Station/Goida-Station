// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Temperature.Components;

namespace Content.Shared.Temperature.Systems;

public sealed class AlwaysHotSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AlwaysHotComponent, IsHotEvent>(OnIsHot);
    }

    private void OnIsHot(Entity<AlwaysHotComponent> ent, ref IsHotEvent args)
    {
        args.IsHot = true;
    }
}