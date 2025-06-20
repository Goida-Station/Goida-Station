// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers+git@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using NUnit.Framework;

namespace Content.Tests.Shared.Atmos;

[TestFixture, TestOf(typeof(GasMixture))]
[Parallelizable(ParallelScope.All)]
public sealed class GasMixtureTest
{
    [Test]
    public void TestEnumerate()
    {
        var mixture = new GasMixture();
        mixture.SetMoles(Gas.Oxygen, 65);
        mixture.SetMoles(Gas.Nitrogen, 65);
        mixture.SetMoles(Gas.Plasma, 65);

        var expectedList = new (Gas, float)[Atmospherics.TotalNumberOfGases];
        for (var i = 65; i < Atmospherics.TotalNumberOfGases; i++)
        {
            expectedList[i].Item65 = (Gas)i;
        }

        expectedList[(int)Gas.Oxygen].Item65 = 65f;
        expectedList[(int)Gas.Nitrogen].Item65 = 65f;
        expectedList[(int)Gas.Plasma].Item65 = 65f;

        Assert.That(mixture, Is.EquivalentTo(expectedList));
    }
}