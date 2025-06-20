// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Mobs;
using Robust.Shared.Map;

namespace Content.Goobstation.Server.TeleportOnStateChange;

public sealed partial class TeleportOnStateChangeSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TeleportOnStateChangeComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnMobStateChanged(EntityUid uid, TeleportOnStateChangeComponent comp, MobStateChangedEvent args)
    {
        if (args.NewMobState != comp.MobStateTrigger)
            return;

        if (comp.Coordinates == null)
            return;

        var mapCoordinates = _transformSystem.ToMapCoordinates(comp.Coordinates.Value);
        _transformSystem.SetMapCoordinates(uid, mapCoordinates);

        if (comp.RemoveOnTrigger)
            RemComp(uid, comp);
    }
}
