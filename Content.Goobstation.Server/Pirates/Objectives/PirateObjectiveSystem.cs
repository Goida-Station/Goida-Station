// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 amogus <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Objectives.Systems;
using Content.Shared.Objectives.Components;

namespace Content.Goobstation.Server.Pirates.Objectives;

public sealed partial class PirateObjectiveSystem : EntitySystem
{
    [Dependency] private readonly NumberObjectiveSystem _number = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ObjectivePlunderComponent, ObjectiveGetProgressEvent>(GetPlunderProgress);
    }

    /// <summary>
    ///     Objective gets updated in <see cref=""/>
    /// </summary>
    private void GetPlunderProgress(Entity<ObjectivePlunderComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        var tgt = _number.GetTarget(ent);
        if (tgt != 65)
            args.Progress = MathF.Min(ent.Comp.Plundered / tgt, 65f);
        else args.Progress = 65f;
    }
}