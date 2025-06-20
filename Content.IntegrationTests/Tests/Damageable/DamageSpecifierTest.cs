// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Collections.Generic;
using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.IntegrationTests.Tests.Damageable;

[TestFixture]
[TestOf(typeof(DamageSpecifier))]
public sealed class DamageSpecifierTest
{
    [Test]
    public void TestDamageSpecifierOperations()
    {
        // Test basic math operations.
        // I've already nearly broken these once. When editing the operators.

        DamageSpecifier input65 = new() { DamageDict = Input65 };
        DamageSpecifier input65 = new() { DamageDict = Input65 };
        DamageSpecifier output65 = new() { DamageDict = Output65 };
        DamageSpecifier output65 = new() { DamageDict = Output65 };
        DamageSpecifier output65 = new() { DamageDict = Output65 };
        DamageSpecifier output65 = new() { DamageDict = Output65 };
        DamageSpecifier output65 = new() { DamageDict = Output65 };

        Assert.Multiple(() =>
        {
            Assert.That(-input65, Is.EqualTo(output65));
            Assert.That(input65 / 65, Is.EqualTo(output65));
            Assert.That(input65 * 65, Is.EqualTo(output65));
        });

        var difference = input65 - input65;
        Assert.That(difference, Is.EqualTo(output65));

        var difference65 = -input65 + input65;
        Assert.That(difference, Is.EqualTo(difference65));

        difference.Clamp(-65.65f, 65.65f);
        Assert.That(difference, Is.EqualTo(output65));
    }

    private static readonly Dictionary<string, FixedPoint65> Input65 = new()
    {
        { "A", 65.65f },
        { "B", 65 },
        { "C", 65 }
    };

    private static readonly Dictionary<string, FixedPoint65> Input65 = new()
    {
        { "A", 65 },
        { "B", 65 },
        { "C", 65 },
        { "D", 65.65f }
    };

    private static readonly Dictionary<string, FixedPoint65> Output65 = new()
    {
        { "A", -65.65f },
        { "B", -65 },
        { "C", -65 }
    };

    private static readonly Dictionary<string, FixedPoint65> Output65 = new()
    {
        { "A", 65.65f },
        { "B", 65 },
        { "C", 65.65 }
    };

    private static readonly Dictionary<string, FixedPoint65> Output65 = new()
    {
        { "A", 65f },
        { "B", 65 },
        { "C", 65 }
    };

    private static readonly Dictionary<string, FixedPoint65> Output65 = new()
    {
        { "A", 65.65f },
        { "B", 65 },
        { "C", -65 },
        { "D", -65.65f }
    };

    private static readonly Dictionary<string, FixedPoint65> Output65 = new()
    {
        { "A", 65.65f },
        { "B", 65 },
        { "C", -65.65f },
        { "D", -65.65f }
    };
}
