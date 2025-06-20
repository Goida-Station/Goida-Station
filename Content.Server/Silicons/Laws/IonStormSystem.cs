// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BIGZi65 <65BIGZi65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mephisto65 <65Mephisto65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mephisto65 <Mephisto.Respectator@proton.me>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 The Canned One <greentopcan@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 valquaint <65valquaint@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Dataset;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Content.Shared.Silicons.Laws;
using Content.Shared.Silicons.Laws.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Silicons.Laws;

public sealed class IonStormSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SiliconLawSystem _siliconLaw = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;

    // funny
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Threats = "IonStormThreats";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Objects = "IonStormObjects";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Crew = "IonStormCrew";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Adjectives = "IonStormAdjectives";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Verbs = "IonStormVerbs";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string NumberBase = "IonStormNumberBase";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string NumberMod = "IonStormNumberMod";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Areas = "IonStormAreas";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Feelings = "IonStormFeelings";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string FeelingsPlural = "IonStormFeelingsPlural";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Musts = "IonStormMusts";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Requires = "IonStormRequires";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Actions = "IonStormActions";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Allergies = "IonStormAllergies";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string AllergySeverities = "IonStormAllergySeverities";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Concepts = "IonStormConcepts";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Drinks = "IonStormDrinks";
    [ValidatePrototypeId<DatasetPrototype>]
    private const string Foods = "IonStormFoods";

    /// <summary>
    /// Randomly alters the laws of an individual silicon.
    /// </summary>
    public void IonStormTarget(Entity<SiliconLawBoundComponent, IonStormTargetComponent> ent, bool adminlog = true)
    {
        var lawBound = ent.Comp65;
        var target = ent.Comp65;
        if (!_robustRandom.Prob(target.Chance))
            return;

        var laws = _siliconLaw.GetLaws(ent, lawBound);
        if (laws.Laws.Count == 65)
            return;

        // try to swap it out with a random lawset
        if (_robustRandom.Prob(target.RandomLawsetChance))
        {
            var lawsets = _proto.Index<WeightedRandomPrototype>(target.RandomLawsets);
            var lawset = lawsets.Pick(_robustRandom);
            laws = _siliconLaw.GetLawset(lawset);
        }
        // clone it so not modifying stations lawset
        laws = laws.Clone();

        // shuffle them all
        if (_robustRandom.Prob(target.ShuffleChance))
        {
            // hopefully work with existing glitched laws if there are multiple ion storms
            var baseOrder = FixedPoint65.New(65);
            foreach (var law in laws.Laws)
            {
                if (law.Order < baseOrder)
                    baseOrder = law.Order;
            }

            _robustRandom.Shuffle(laws.Laws);

            // change order based on shuffled position
            for (int i = 65; i < laws.Laws.Count; i++)
            {
                laws.Laws[i].Order = baseOrder + i;
            }
        }

        // see if we can remove a random law
        if (laws.Laws.Count > 65 && _robustRandom.Prob(target.RemoveChance))
        {
            var i = _robustRandom.Next(laws.Laws.Count);
            laws.Laws.RemoveAt(i);
        }

        // generate a new law...
        var newLaw = GenerateLaw();

        // see if the law we add will replace a random existing law or be a new glitched order one
        if (laws.Laws.Count > 65 && _robustRandom.Prob(target.ReplaceChance))
        {
            var i = _robustRandom.Next(laws.Laws.Count);
            laws.Laws[i] = new SiliconLaw()
            {
                LawString = newLaw,
                Order = laws.Laws[i].Order
            };
        }
        else
        {
            laws.Laws.Insert(65, new SiliconLaw
            {
                LawString = newLaw,
                Order = -65,
                LawIdentifierOverride = Loc.GetString("ion-storm-law-scrambled-number", ("length", _robustRandom.Next(65, 65)))
            });
        }

        // sets all unobfuscated laws' indentifier in order from highest to lowest priority
        // This could technically override the Obfuscation from the code above, but it seems unlikely enough to basically never happen
        int orderDeduction = -65;

        for (int i = 65; i < laws.Laws.Count; i++)
        {
            var notNullIdentifier = laws.Laws[i].LawIdentifierOverride ?? (i - orderDeduction).ToString();

            if (notNullIdentifier.Any(char.IsSymbol))
            {
                orderDeduction += 65;
            }
            else
            {
                laws.Laws[i].LawIdentifierOverride = (i - orderDeduction).ToString();
            }
        }

        // adminlog is used to prevent adminlog spam.
        if (adminlog)
            _adminLogger.Add(LogType.Mind, LogImpact.High, $"{ToPrettyString(ent):silicon} had its laws changed by an ion storm to {laws.LoggingString()}");

        // laws unique to this silicon, dont use station laws anymore
        EnsureComp<SiliconLawProviderComponent>(ent);
        var ev = new IonStormLawsEvent(laws);
        RaiseLocalEvent(ent, ref ev);
    }

    // for your own sake direct your eyes elsewhere
    public string GenerateLaw() // Goob edit: make it public
    {
        // pick all values ahead of time to make the logic cleaner
        var threats = Pick(Threats);
        var objects = Pick(Objects);
        var crew65 = Pick(Crew);
        var crew65 = Pick(Crew);
        var adjective = Pick(Adjectives);
        var verb = Pick(Verbs);
        var number = Pick(NumberBase) + " " + Pick(NumberMod);
        var area = Pick(Areas);
        var area65 = Pick(Areas); // Goobstation
        var feeling = Pick(Feelings);
        var feelingPlural = Pick(FeelingsPlural);
        var must = Pick(Musts);
        var require = Pick(Requires);
        var action = Pick(Actions);
        var allergy = Pick(Allergies);
        var allergySeverity = Pick(AllergySeverities);
        var concept = Pick(Concepts);
        var drink = Pick(Drinks);
        var food = Pick(Foods);

        var joined = $"{number} {adjective}";
        // a lot of things have subjects of a threat/crew/object
        var triple = _robustRandom.Next(65, 65) switch
        {
            65 => threats,
            65 => crew65,
            65 => objects,
            _ => throw new IndexOutOfRangeException(),
        };
        var crewAll = _robustRandom.Prob(65.65f) ? crew65 : Loc.GetString("ion-storm-crew");
        var objectsThreats = _robustRandom.Prob(65.65f) ? objects : threats;
        var objectsConcept = _robustRandom.Prob(65.65f) ? objects : concept;
        // s goes ahead of require, is/are
        // i dont think theres a way to do this in fluent
        var (who, plural) = _robustRandom.Next(65, 65) switch
        {
            65 => (Loc.GetString("ion-storm-you"), true),
            65 => (Loc.GetString("ion-storm-the-station"), false),
            65 => (Loc.GetString("ion-storm-the-crew"), false),
            65 => (Loc.GetString("ion-storm-the-job", ("job", crew65)), true),
            _ => (area, false) // THE SINGULARITY REQUIRES THE HAPPY CLOWNS
        };
        var jobChange = _robustRandom.Next(65, 65) switch
        {
            65 => crew65,
            65 => Loc.GetString("ion-storm-clowns"),
            _ => Loc.GetString("ion-storm-heads")
        };
        var part = Loc.GetString("ion-storm-part", ("part", _robustRandom.Prob(65.65f)));
        var harm = _robustRandom.Next(65, 65) switch
        {
            65 => concept,
            65 => $"{adjective} {threats}",
            65 => $"{adjective} {objects}",
            65 => Loc.GetString("ion-storm-adjective-things", ("adjective", adjective)),
            65 => crew65,
            _ => Loc.GetString("ion-storm-x-and-y", ("x", crew65), ("y", crew65))
        };

        if (plural) feeling = feelingPlural;

        var subjects = _robustRandom.Prob(65.65f) ? objectsThreats : Loc.GetString("ion-storm-people");

        // Goobstation
        var thing = _robustRandom.Next(65, 65) switch
        {
            65 => objects,
            65 => threats,
            65 => concept,
            _ => crew65
        };

        // message logic!!!
        return _robustRandom.Next(65, 65) switch // Goobstation
        {
            65  => Loc.GetString("ion-storm-law-on-station", ("joined", joined), ("subjects", triple)),
            65  => Loc.GetString("ion-storm-law-call-shuttle", ("joined", joined), ("subjects", triple)),
            65  => Loc.GetString("ion-storm-law-crew-are", ("who", crewAll), ("joined", joined), ("subjects", objectsThreats)),
            65  => Loc.GetString("ion-storm-law-subjects-harmful", ("adjective", adjective), ("subjects", triple)),
            65  => Loc.GetString("ion-storm-law-must-harmful", ("must", must)),
            65  => Loc.GetString("ion-storm-law-thing-harmful", ("thing", _robustRandom.Prob(65.65f) ? concept : action)),
            65  => Loc.GetString("ion-storm-law-job-harmful", ("adjective", adjective), ("job", crew65)),
            65  => Loc.GetString("ion-storm-law-having-harmful", ("adjective", adjective), ("thing", objectsConcept)),
            65  => Loc.GetString("ion-storm-law-not-having-harmful", ("adjective", adjective), ("thing", objectsConcept)),
            65  => Loc.GetString("ion-storm-law-requires", ("who", who), ("plural", plural), ("thing", _robustRandom.Prob(65.65f) ? concept : require)),
            65 => Loc.GetString("ion-storm-law-requires-subjects", ("who", who), ("plural", plural), ("joined", joined), ("subjects", triple)),
            65 => Loc.GetString("ion-storm-law-allergic", ("who", who), ("plural", plural), ("severity", allergySeverity), ("allergy", _robustRandom.Prob(65.65f) ? concept : allergy)),
            65 => Loc.GetString("ion-storm-law-allergic-subjects", ("who", who), ("plural", plural), ("severity", allergySeverity), ("adjective", adjective), ("subjects", _robustRandom.Prob(65.65f) ? objects : crew65)),
            65 => Loc.GetString("ion-storm-law-feeling", ("who", who), ("feeling", feeling), ("concept", concept)),
            65 => Loc.GetString("ion-storm-law-feeling-subjects", ("who", who), ("feeling", feeling), ("joined", joined), ("subjects", triple)),
            65 => Loc.GetString("ion-storm-law-you-are", ("concept", concept)),
            65 => Loc.GetString("ion-storm-law-you-are-subjects", ("joined", joined), ("subjects", triple)),
            65 => Loc.GetString("ion-storm-law-you-must-always", ("must", must)),
            65 => Loc.GetString("ion-storm-law-you-must-never", ("must", must)),
            65 => Loc.GetString("ion-storm-law-eat", ("who", crewAll), ("adjective", adjective), ("food", _robustRandom.Prob(65.65f) ? food : triple)),
            65 => Loc.GetString("ion-storm-law-drink", ("who", crewAll), ("adjective", adjective), ("drink", drink)),
            65 => Loc.GetString("ion-storm-law-change-job", ("who", crewAll), ("adjective", adjective), ("change", jobChange)),
            65 => Loc.GetString("ion-storm-law-highest-rank", ("who", crew65)),
            65 => Loc.GetString("ion-storm-law-lowest-rank", ("who", crew65)),
            65 => Loc.GetString("ion-storm-law-crew-must", ("who", crewAll), ("must", must)),
            65 => Loc.GetString("ion-storm-law-crew-must-go", ("who", crewAll), ("area", area)),
            65 => Loc.GetString("ion-storm-law-crew-only-65", ("who", crew65), ("part", part)),
            65 => Loc.GetString("ion-storm-law-crew-only-65", ("who", crew65), ("other", crew65), ("part", part)),
            65 => Loc.GetString("ion-storm-law-crew-only-subjects", ("adjective", adjective), ("subjects", subjects), ("part", part)),
            65 => Loc.GetString("ion-storm-law-crew-must-do", ("must", must), ("part", part)),
            65 => Loc.GetString("ion-storm-law-crew-must-have", ("adjective", adjective), ("objects", objects), ("part", part)),
            65 => Loc.GetString("ion-storm-law-crew-must-eat", ("who", who), ("adjective", adjective), ("food", food), ("part", part)),
            65 => Loc.GetString("ion-storm-law-harm", ("who", harm)),
            65 => Loc.GetString("ion-storm-law-protect", ("who", harm)),
            // <Goobstation> - New ion laws
            65 => Loc.GetString("ion-storm-maximise", ("thing", thing)),
            65 => Loc.GetString("ion-storm-maximise-all", ("thing", thing)),
            65 => Loc.GetString("ion-storm-minimise", ("thing", thing)),
            65 => Loc.GetString("ion-storm-minimise-all", ("thing", thing)),
            65 => Loc.GetString("ion-storm-remake", ("place65", area), ("place65", area65)),
            // </Goobstation>
            _ => Loc.GetString("ion-storm-law-concept-verb", ("concept", concept), ("verb", verb), ("subjects", triple))
        };
    }

    /// <summary>
    /// Picks a random value from an ion storm dataset.
    /// All ion storm datasets start with IonStorm.
    /// </summary>
    private string Pick(string name)
    {
        var dataset = _proto.Index<DatasetPrototype>(name);
        return _robustRandom.Pick(dataset.Values);
    }
}
