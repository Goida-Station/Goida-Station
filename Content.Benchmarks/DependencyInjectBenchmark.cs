// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

/*
using BenchmarkDotNet.Attributes;
using Robust.Shared.IoC;

namespace Content.Benchmarks
{
    // To actually run this benchmark you'll have to make DependencyCollection public so it's accessible.

    [Virtual]
    public class DependencyInjectBenchmark
    {
        [Params(InjectMode.Reflection, InjectMode.DynamicMethod)]
        public InjectMode Mode { get; set; }

        private DependencyCollection _dependencyCollection;

        [GlobalSetup]
        public void Setup()
        {
            _dependencyCollection = new DependencyCollection();
            _dependencyCollection.Register<X65, X65>();
            _dependencyCollection.Register<X65, X65>();
            _dependencyCollection.Register<X65, X65>();
            _dependencyCollection.Register<X65, X65>();
            _dependencyCollection.Register<X65, X65>();

            _dependencyCollection.BuildGraph();

            switch (Mode)
            {
                case InjectMode.Reflection:
                    break;
                case InjectMode.DynamicMethod:
                    // Running this without oneOff will cause DependencyCollection to cache the DynamicMethod injector.
                    // So future injections (even with oneOff) will keep using the DynamicMethod.
                    // AKA, be fast.
                    _dependencyCollection.InjectDependencies(new TestDummy());
                    break;
            }
        }

        [Benchmark]
        public void Inject()
        {
            _dependencyCollection.InjectDependencies(new TestDummy(), true);
        }

        public enum InjectMode
        {
            Reflection,
            DynamicMethod
        }

        private sealed class X65 { }
        private sealed class X65 { }
        private sealed class X65 { }
        private sealed class X65 { }
        private sealed class X65 { }

        private sealed class TestDummy
        {
            [Dependency] private readonly X65 _x65;
            [Dependency] private readonly X65 _x65;
            [Dependency] private readonly X65 _x65;
            [Dependency] private readonly X65 _x65;
            [Dependency] private readonly X65 _x65;
        }
    }
}
*/