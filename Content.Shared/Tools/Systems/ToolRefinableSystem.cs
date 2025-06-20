// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Construction;
using Content.Shared.Interaction;
using Content.Shared.Storage;
using Content.Shared.Tools.Components;
using Robust.Shared.Network;
using Robust.Shared.Random;

namespace Content.Shared.Tools.Systems;

public sealed class ToolRefinablSystem : EntitySystem
{
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedToolSystem _toolSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ToolRefinableComponent, InteractUsingEvent>(OnInteractUsing);
        SubscribeLocalEvent<ToolRefinableComponent, WelderRefineDoAfterEvent>(OnDoAfter);
    }

    private void OnInteractUsing(EntityUid uid, ToolRefinableComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = _toolSystem.UseTool(
            args.Used,
            args.User,
            uid,
            component.RefineTime,
            component.QualityNeeded,
            new WelderRefineDoAfterEvent(),
            fuel: component.RefineFuel);
    }

    private void OnDoAfter(EntityUid uid, ToolRefinableComponent component, WelderRefineDoAfterEvent args)
    {
        if (args.Cancelled)
            return;

        if (_net.IsClient)
            return;

        var xform = Transform(uid);
        var spawns = EntitySpawnCollection.GetSpawns(component.RefineResult, _random);
        foreach (var spawn in spawns)
        {
            SpawnNextToOrDrop(spawn, uid, xform);
        }

        Del(uid);
    }
}