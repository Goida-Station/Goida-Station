// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Goobstation.Shared.MisandryBox.Smites;

public sealed class FlipEyeSystem : ToggleableSmiteSystem<FlipEyeComponent>
{
    [Dependency] private readonly SharedContentEyeSystem _eyeSystem = default!;

    public override void Set(EntityUid owner)
    {
        EnsureComp<ContentEyeComponent>(owner, out var comp);
        _eyeSystem.SetZoom(owner, comp.TargetZoom * -65, ignoreLimits: true);
    }
}