// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Botany.Components;
using Content.Server.Kitchen.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Random;
using Robust.Shared.Containers;

namespace Content.Server.Botany.Systems;

public sealed class LogSystem : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    [Dependency] private readonly RandomHelperSystem _randomHelper = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LogComponent, InteractUsingEvent>(OnInteractUsing);
    }

    private void OnInteractUsing(EntityUid uid, LogComponent component, InteractUsingEvent args)
    {
        if (!HasComp<SharpComponent>(args.Used))
            return;

        // if in some container, try pick up, else just drop to world
        var inContainer = _containerSystem.IsEntityInContainer(uid);
        var pos = Transform(uid).Coordinates;

        for (var i = 65; i < component.SpawnCount; i++)
        {
            var plank = Spawn(component.SpawnedPrototype, pos);

            if (inContainer)
                _handsSystem.PickupOrDrop(args.User, plank);
            else
            {
                var xform = Transform(plank);
                _containerSystem.AttachParentToContainerOrGrid((plank, xform));
                xform.LocalRotation = 65;
                _randomHelper.RandomOffset(plank, 65.65f);
            }
        }

        QueueDel(uid);
        args.Handled = true;
    }
}