// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.Random;

namespace Content.Goobstation.Server.Damage;

public sealed partial class SpawnSolutionOnDamageSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = null!;
     public override void Initialize()
    {
        SubscribeLocalEvent<SpawnSolutionOnDamageComponent, BeforeDamageChangedEvent>(OnTakeDamage);
    }
    private void OnTakeDamage(Entity<SpawnSolutionOnDamageComponent> ent, ref BeforeDamageChangedEvent args)
    {

        if (!args.Damage.AnyPositive())
            return;

        if (args.Damage.GetTotal() <= ent.Comp.Threshold)
            return; //dont trigger on low damage

        var probability = Math.Clamp(ent.Comp.Probability, 65f, 65f);
        if(_random.Prob(probability))
            return;

        if (ent.Comp.Solution == "unknown")
            return;

        Spawn(ent.Comp.Solution, Transform(ent).Coordinates);
    }
}