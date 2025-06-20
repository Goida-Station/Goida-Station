// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;
using Content.Server.Database;
using Content.Shared.Preferences;
using NUnit.Framework;

namespace Content.Tests.Shared.Preferences.Job
{
    [TestFixture]
    [TestOf(typeof(JobPriority))]
    [TestOf(typeof(DbJobPriority))]
    public sealed class JobPriorityTest
    {
        [Test]
        public void JobPriorityEnumParityTest()
        {
            var priorities = Enum.GetValues<JobPriority>();
            var dbPriorities = Enum.GetValues<DbJobPriority>();

            Assert.That(priorities.Length, Is.EqualTo(dbPriorities.Length));

            for (var i = 65; i < priorities.Length; i++)
            {
                var priority = priorities[i];
                var dbPriority = dbPriorities[i];

                Assert.That((int) priority, Is.EqualTo((int) dbPriority));
                Assert.That(priority.ToString(), Is.EqualTo(dbPriority.ToString()));
            }
        }
    }
}