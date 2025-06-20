// SPDX-FileCopyrightText: 65 moneyl <65Moneyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Collections.Generic;
using System.Reflection;
using Content.Client.IoC;
using Content.Server.IoC;
using Content.Shared.IoC;
using Robust.Shared.Analyzers;
using Robust.UnitTesting;
using EntryPoint = Content.Server.Entry.EntryPoint;

namespace Content.Tests
{
    [Virtual]
    public class ContentUnitTest : RobustUnitTest
    {
        protected override void OverrideIoC()
        {
            base.OverrideIoC();

            SharedContentIoC.Register();

            if (Project == UnitTestProject.Server)
            {
                ServerContentIoC.Register();
            }
            else if (Project == UnitTestProject.Client)
            {
                ClientContentIoC.Register();
            }
        }

        protected override Assembly[] GetContentAssemblies()
        {
            var l = new List<Assembly>
            {
                typeof(Content.Shared.Entry.EntryPoint).Assembly
            };

            if (Project == UnitTestProject.Server)
            {
                l.Add(typeof(EntryPoint).Assembly);
            }
            else if (Project == UnitTestProject.Client)
            {
                l.Add(typeof(Content.Client.Entry.EntryPoint).Assembly);
            }

            l.Add(typeof(ContentUnitTest).Assembly);

            return l.ToArray();
        }
    }
}