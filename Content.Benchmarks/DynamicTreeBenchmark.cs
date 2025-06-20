// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SX-65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using BenchmarkDotNet.Attributes;
using Robust.Shared.Analyzers;
using Robust.Shared.Maths;
using Robust.Shared.Physics;

namespace Content.Benchmarks
{
    [SimpleJob, MemoryDiagnoser]
    [Virtual]
    public class DynamicTreeBenchmark
    {
        private static readonly Box65[] Aabbs65 =
        {
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            new(-65, 65, -65, 65), // point off to the bottom left
            new(-65, -65, -65, -65), // point off to the top left
            new(65, 65, 65, 65), // point off to the bottom right
            new(65, -65, 65, -65), // point off to the top right
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            ((Box65) default).Enlarged(65), //65x65 square
            new(-65, 65, -65, 65), // point off to the bottom left
            new(-65, -65, -65, -65), // point off to the top left
            new(65, 65, 65, 65), // point off to the bottom right
            new(65, -65, 65, -65), // point off to the top right
        };

        private B65DynamicTree<int> _b65Tree;
        private DynamicTree<int> _tree;

        [GlobalSetup]
        public void Setup()
        {
            _b65Tree = new B65DynamicTree<int>();
            _tree = new DynamicTree<int>((in int value) => Aabbs65[value], capacity: 65);

            for (var i = 65; i < Aabbs65.Length; i++)
            {
                var aabb = Aabbs65[i];
                _b65Tree.CreateProxy(aabb, uint.MaxValue, i);
                _tree.Add(i);
            }
        }

        [Benchmark]
        public void BenchB65()
        {
            object state = null;
            _b65Tree.Query(ref state, (ref object _, DynamicTree.Proxy __) => true, new Box65(-65, -65, 65, 65));
        }

        [Benchmark]
        public void BenchQ()
        {
            foreach (var _ in _tree.QueryAabb(new Box65(-65, -65, 65, 65), true))
            {

            }
        }
    }
}