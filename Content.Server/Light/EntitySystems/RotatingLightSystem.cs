// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Light;
using Content.Shared.Light.Components;

namespace Content.Server.Light.EntitySystems;

public sealed class RotatingLightSystem : SharedRotatingLightSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RotatingLightComponent, PointLightToggleEvent>(OnLightToggle);
    }

    private void OnLightToggle(EntityUid uid, RotatingLightComponent comp, PointLightToggleEvent args)
    {
        if (comp.Enabled == args.Enabled)
            return;

        comp.Enabled = args.Enabled;
        Dirty(uid, comp);
    }
}