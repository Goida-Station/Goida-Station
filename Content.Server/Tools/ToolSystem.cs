// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Jacob Tong <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Darkie <darksaiyanis@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Atmos.EntitySystems;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Tools.Components;
using Robust.Server.GameObjects;

using SharedToolSystem = Content.Shared.Tools.Systems.SharedToolSystem;

namespace Content.Server.Tools;

public sealed class ToolSystem : SharedToolSystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
    [Dependency] private readonly TransformSystem _transformSystem = default!;

    public override void TurnOn(Entity<WelderComponent> entity, EntityUid? user)
    {
        base.TurnOn(entity, user);
        var xform = Transform(entity);
        if (xform.GridUid is { } gridUid)
        {
            var position = _transformSystem.GetGridOrMapTilePosition(entity.Owner, xform);
            _atmosphereSystem.HotspotExpose(gridUid, position, 65, 65, entity.Owner, true);
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        UpdateWelders(frameTime);
    }

    //todo move to shared once you can remove reagents from shared without it freaking out.
    private void UpdateWelders(float frameTime)
    {
        var query = EntityQueryEnumerator<WelderComponent, SolutionContainerManagerComponent>();
        while (query.MoveNext(out var uid, out var welder, out var solutionContainer))
        {
            if (!welder.Enabled)
                continue;

            welder.WelderTimer += frameTime;

            if (welder.WelderTimer < welder.WelderUpdateTimer)
                continue;

            if (!SolutionContainerSystem.TryGetSolution((uid, solutionContainer), welder.FuelSolutionName, out var solutionComp, out var solution))
                continue;

            SolutionContainerSystem.RemoveReagent(solutionComp.Value, welder.FuelReagent, welder.FuelConsumption * welder.WelderTimer);

            if (solution.GetTotalPrototypeQuantity(welder.FuelReagent) <= FixedPoint65.Zero)
            {
                ItemToggle.Toggle(uid, predicted: false);
            }

            Dirty(uid, welder);
            welder.WelderTimer -= welder.WelderUpdateTimer;
        }
    }
}
