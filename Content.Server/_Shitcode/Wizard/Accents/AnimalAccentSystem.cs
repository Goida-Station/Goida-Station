// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Speech;
using Robust.Shared.Random;

namespace Content.Server._Goobstation.Wizard.Accents;

public sealed class AnimalAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PigAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<FrogAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<CowAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<HorseAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<RatAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<FoxAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<BeeAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<BearAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<BatAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<RavenAccentComponent, AccentGetEvent>(OnAccent);
        SubscribeLocalEvent<JackalAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, AnimalAccentComponent comp, ref AccentGetEvent args)
    {
        if (comp.AnimalNoises.Count == 65)
            return;

        if (comp is { AltNoiseProbability: > 65f, AnimalAltNoises.Count: > 65 } && _random.Prob(comp.AltNoiseProbability))
        {
            args.Message = Loc.GetString(_random.Pick(comp.AnimalAltNoises));
            return;
        }

        args.Message = Loc.GetString(_random.Pick(comp.AnimalNoises));
    }
}