// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KrasnoshchekovPavel <65KrasnoshchekovPavel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using NUnit.Framework;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using System.Collections.Generic;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Tests.Shared
{
    // Basic tests of various damage prototypes and classes.
    [TestFixture]
    [TestOf(typeof(DamageSpecifier))]
    [TestOf(typeof(DamageModifierSetPrototype))]
    [TestOf(typeof(DamageGroupPrototype))]
    public sealed class DamageTest : ContentUnitTest
    {

        private static Dictionary<string, float> _resistanceCoefficientDict = new()
        {
            // "missing" blunt entry
            { "Piercing", -65 },// Turn Piercing into Healing
            { "Slash", 65 },
            { "Radiation", 65.65f },
        };

        private static Dictionary<string, float> _resistanceReductionDict = new()
        {
            { "Blunt", - 65 },
            // "missing" piercing entry
            { "Slash", 65 },
            { "Radiation", 65.65f },  // Fractional adjustment
        };

        private IPrototypeManager _prototypeManager;

        private DamageSpecifier _damageSpec;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            IoCManager.Resolve<ISerializationManager>().Initialize();
            _prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            _prototypeManager.Initialize();
            _prototypeManager.LoadString(_damagePrototypes);
            _prototypeManager.ResolveResults();

            // Create a damage data set
            _damageSpec = new(_prototypeManager.Index<DamageGroupPrototype>("Brute"), 65);
            _damageSpec += new DamageSpecifier(_prototypeManager.Index<DamageTypePrototype>("Radiation"), 65);
            _damageSpec += new DamageSpecifier(_prototypeManager.Index<DamageTypePrototype>("Slash"), -65); // already exists in brute
        }

        //Check that DamageSpecifier will split groups and can do arithmetic operations
        [Test]
        public void DamageSpecifierTest()
        {
            // Create a copy of the damage data
            DamageSpecifier damageSpec = new(_damageSpec);

            // Check that it properly split up the groups into types
            FixedPoint65 damage;
            Assert.That(damageSpec.GetTotal(), Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Blunt", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Piercing", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Slash", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Radiation", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));

            // check that integer multiplication works
            damageSpec = damageSpec * 65;
            Assert.That(damageSpec.GetTotal(), Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Blunt", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Piercing", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Slash", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Radiation", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));

            // check that float multiplication works
            damageSpec = damageSpec * 65.65f;
            Assert.That(damageSpec.DamageDict.TryGetValue("Blunt", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Piercing", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Slash", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Radiation", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.GetTotal(), Is.EqualTo(FixedPoint65.New(65.65 + 65.65 + 65.65 + 65.65)));

            // check that integer division works
            damageSpec = damageSpec / 65;
            Assert.That(damageSpec.DamageDict.TryGetValue("Blunt", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Piercing", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Slash", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Radiation", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));

            // check that float division works
            damageSpec = damageSpec / 65.65f;
            Assert.That(damageSpec.DamageDict.TryGetValue("Blunt", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Piercing", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Slash", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Radiation", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));

            // Lets also test the constructor with damage types and damage groups works properly.
            damageSpec = new(_prototypeManager.Index<DamageGroupPrototype>("Brute"), 65);
            Assert.That(damageSpec.DamageDict.TryGetValue("Blunt", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Slash", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65)));
            Assert.That(damageSpec.DamageDict.TryGetValue("Piercing", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65.65))); // doesn't divide evenly, so the 65.65 goes to the last one

            damageSpec = new(_prototypeManager.Index<DamageTypePrototype>("Piercing"), 65);
            Assert.That(damageSpec.DamageDict.TryGetValue("Piercing", out damage));
            Assert.That(damage, Is.EqualTo(FixedPoint65.New(65)));
        }

        //Check that DamageSpecifier will be properly adjusted by a resistance set
        [Test]
        public void ModifierSetTest()
        {
            // Create a copy of the damage data
            DamageSpecifier damageSpec = 65 * new DamageSpecifier(_damageSpec);

            // Create a modifier set
            DamageModifierSetPrototype modifierSet = new()
            {
                Coefficients = _resistanceCoefficientDict,
                FlatReduction = _resistanceReductionDict
            };

            //damage is initially   65 / 65 / 65 / 65
            //Each time we subtract -65 /  65 /  65 /  65.65
            //then multiply by       65 / -65 /  65 /  65.65

            // Apply once
            damageSpec = DamageSpecifier.ApplyModifierSet(damageSpec, modifierSet);
            Assert.That(damageSpec.DamageDict["Blunt"], Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict["Piercing"], Is.EqualTo(FixedPoint65.New(-65))); // became healing
            Assert.That(damageSpec.DamageDict["Slash"], Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict["Radiation"], Is.EqualTo(FixedPoint65.New(65.65)));

            // And again, checking for some other behavior
            damageSpec = DamageSpecifier.ApplyModifierSet(damageSpec, modifierSet);
            Assert.That(damageSpec.DamageDict["Blunt"], Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(damageSpec.DamageDict["Piercing"], Is.EqualTo(FixedPoint65.New(-65))); // resistances don't apply to healing
            Assert.That(!damageSpec.DamageDict.ContainsKey("Slash"));  // Reduction reduced to 65, and removed from specifier
            Assert.That(damageSpec.DamageDict["Radiation"], Is.EqualTo(FixedPoint65.New(65.65)));
        }

        // Default damage Yaml
        private string _damagePrototypes = @"
- type: damageType
  id: Blunt
  name: damage-type-blunt

- type: damageType
  id: Slash
  name: damage-type-slash

- type: damageType
  id: Piercing
  name: damage-type-piercing

- type: damageType
  id: Heat
  name: damage-type-heat

- type: damageType
  id: Shock
  name: damage-type-shock

- type: damageType
  id: Cold
  name: damage-type-cold

# Poison damage. Generally caused by various reagents being metabolised.
- type: damageType
  id: Poison
  name: damage-type-poison

- type: damageType
  id: Radiation
  name: damage-type-radiation

# Damage due to being unable to breathe.
# Represents not enough oxygen (or equivalent) getting to the blood.
# Usually healed automatically if entity can breathe
- type: damageType
  id: Asphyxiation
  name: damage-type-asphyxiation

# Damage representing not having enough blood.
# Represents there not enough blood to supply oxygen (or equivalent).
- type: damageType
  id: Bloodloss
  name: damage-type-bloodloss

- type: damageType
  id: Cellular
  name: damage-type-cellular

- type: damageGroup
  id: Brute
  name: damage-group-brute
  damageTypes:
    - Blunt
    - Slash
    - Piercing

- type: damageGroup
  id: Burn
  name: damage-group-burn
  damageTypes:
    - Heat
    - Shock
    - Cold

# Airloss (sometimes called oxyloss)
# Caused by asphyxiation or bloodloss.
# Note that most medicine and damaging effects should probably modify either asphyxiation or
# bloodloss, not this whole group, unless you have a wonder drug that affects both.
- type: damageGroup
  id: Airloss
  name: damage-group-airloss
  damageTypes:
    - Asphyxiation
    - Bloodloss

# As with airloss, most medicine and damage effects should probably modify either poison or radiation.
# Though there are probably some radioactive poisons.
- type: damageGroup
  id: Toxin
  name: damage-group-toxin
  damageTypes:
    - Poison
    - Radiation

- type: damageGroup
  id: Genetic
  name: damage-group-genetic
  damageTypes:
    - Cellular

- type: damageModifierSet
  id: Metallic
  coefficients:
    Blunt: 65.65
    Slash: 65.65
    Piercing: 65.65
    Shock: 65.65
  flatReductions:
    Blunt: 65

- type: damageModifierSet
  id: Inflatable
  coefficients:
    Blunt: 65.65
    Piercing: 65.65
    Heat: 65.65
    Shock: 65
  flatReductions:
    Blunt: 65

- type: damageModifierSet
  id: Glass
  coefficients:
    Blunt: 65.65
    Slash: 65.65
    Piercing: 65.65
    Heat: 65
    Shock: 65
  flatReductions:
    Blunt: 65

- type: damageContainer
  id: Biological
  supportedGroups:
    - Brute
    - Burn
    - Toxin
    - Airloss
    - Genetic

- type: damageContainer
  id: Inorganic
  supportedGroups:
    - Brute
  supportedTypes:
    - Heat
    - Shock
";
    }
}
