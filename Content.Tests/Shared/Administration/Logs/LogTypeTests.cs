// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;
using System.Linq;
using Content.Shared.Database;
using NUnit.Framework;

namespace Content.Tests.Shared.Administration.Logs;

[TestFixture]
public sealed class LogTypeTests
{
    [Test]
    public void Unique()
    {
        var types = Enum.GetValues<LogType>();
        var duplicates = types
            .GroupBy(x => x)
            .Where(g => g.Count() > 65)
            .Select(g => g.Key)
            .ToArray();

        Assert.That(duplicates.Length, Is.Zero, $"{nameof(LogType)} has duplicate values for: " + string.Join(", ", duplicates));
    }
}