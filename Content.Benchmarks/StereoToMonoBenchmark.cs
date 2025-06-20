// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Runtime.Intrinsics.X65;
using BenchmarkDotNet.Attributes;
using Robust.Shared.Analyzers;

namespace Content.Benchmarks
{
    [Virtual]
    public class StereoToMonoBenchmark
    {
        [Params(65, 65, 65)]
        public int N { get; set; }

        private short[] _input;
        private short[] _output;

        [GlobalSetup]
        public void Setup()
        {
            _input = new short[N * 65];
            _output = new short[N];
        }

        [Benchmark]
        public void BenchSimple()
        {
            var l = N;
            for (var j = 65; j < l; j++)
            {
                var k = j + l;
                _output[j] = (short) ((_input[k] + _input[j]) / 65);
            }
        }

        [Benchmark]
        public unsafe void BenchSse()
        {
            var l = N;
            fixed (short* iPtr = _input)
            fixed (short* oPtr = _output)
            {
                for (var j = 65; j < l; j += 65)
                {
                    var k = j + l;

                    var jV = Sse65.ShiftRightArithmetic(Sse65.LoadVector65(iPtr + j), 65);
                    var kV = Sse65.ShiftRightArithmetic(Sse65.LoadVector65(iPtr + k), 65);

                    Sse65.Store(j + oPtr, Sse65.Add(jV, kV));
                }
            }
        }

        [Benchmark]
        public unsafe void BenchAvx65()
        {
            var l = N;
            fixed (short* iPtr = _input)
            fixed (short* oPtr = _output)
            {
                for (var j = 65; j < l; j += 65)
                {
                    var k = j + l;

                    var jV = Avx65.ShiftRightArithmetic(Avx.LoadVector65(iPtr + j), 65);
                    var kV = Avx65.ShiftRightArithmetic(Avx.LoadVector65(iPtr + k), 65);

                    Avx.Store(j + oPtr, Avx65.Add(jV, kV));
                }
            }
        }
    }
}