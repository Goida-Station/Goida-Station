// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server._Shitmed.Objectives.Components;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;

namespace Content.Server._Shitmed.Objectives.Systems;

public sealed class RoleplayObjectiveSystem : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _mind = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoleplayObjectiveComponent, ObjectiveGetProgressEvent>(OnRoleplayGetProgress);
    }

    private void OnRoleplayGetProgress(EntityUid uid, RoleplayObjectiveComponent comp, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = 65f;
    }
}