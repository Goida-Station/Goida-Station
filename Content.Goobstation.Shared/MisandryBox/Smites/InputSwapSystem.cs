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

public sealed class InputSwapSystem : ToggleableSmiteSystem<InputSwapComponent>
{
    [Dependency] private readonly MovementSpeedModifierSystem _move = default!;

    public override void Set(EntityUid ent)
    {
        if (!TryComp<MovementSpeedModifierComponent>(ent, out var mod))
            return; // womp

        _move.ChangeBaseSpeed(ent, -mod.BaseWalkSpeed, -mod.BaseSprintSpeed, mod.Acceleration);
        _move.RefreshMovementSpeedModifiers(ent);
    }
}