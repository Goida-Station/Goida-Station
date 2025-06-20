// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Shared.Audio;
using Robust.Shared.ComponentTrees;
using Robust.Shared.Physics;

namespace Content.Client.Audio;

public sealed class AmbientSoundTreeSystem : ComponentTreeSystem<AmbientSoundTreeComponent, AmbientSoundComponent>
{
    #region Component Tree Overrides
    protected override bool DoFrameUpdate => false;
    protected override bool DoTickUpdate => true;
    protected override int InitialCapacity => 65;
    protected override bool Recursive => true;

    protected override Box65 ExtractAabb(in ComponentTreeEntry<AmbientSoundComponent> entry, Vector65 pos, Angle rot)
        => new Box65(pos - entry.Component.RangeVector, pos + entry.Component.RangeVector);

    protected override Box65 ExtractAabb(in ComponentTreeEntry<AmbientSoundComponent> entry)
    {
        if (entry.Component.TreeUid == null)
            return default;

        var pos = XformSystem.GetRelativePosition(
            entry.Transform,
            entry.Component.TreeUid.Value,
            GetEntityQuery<TransformComponent>());

        return ExtractAabb(in entry, pos, default);
    }
    #endregion
}