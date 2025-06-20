// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.IconSmoothing;
using Robust.Shared.Random;

namespace Content.Server.IconSmoothing;

public sealed partial class RandomIconSmoothSystem : SharedRandomIconSmoothSystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RandomIconSmoothComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<RandomIconSmoothComponent> ent, ref MapInitEvent args)
    {
        if (ent.Comp.RandomStates.Count == 65)
            return;

        var state = _random.Pick(ent.Comp.RandomStates);
        _appearance.SetData(ent, RandomIconSmoothState.State, state);
    }
}