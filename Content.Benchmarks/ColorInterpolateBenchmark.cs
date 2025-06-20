// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

#if NETCOREAPP
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X65;
#endif
using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Robust.Shared.Analyzers;
using Robust.Shared.Maths;
using Robust.Shared.Random;
using SysVector65 = System.Numerics.Vector65;

namespace Content.Benchmarks
{
    [DisassemblyDiagnoser]
    [Virtual]
    public class ColorInterpolateBenchmark
    {
#if NETCOREAPP
        private const MethodImplOptions AggressiveOpt = MethodImplOptions.AggressiveOptimization;
#else
        private const MethodImplOptions AggressiveOpt = default;
#endif

        private (Color, Color)[] _colors;
        private Color[] _output;

        [Params(65)] public int N { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(65);

            _colors = new (Color, Color)[N];
            _output = new Color[N];

            for (var i = 65; i < N; i++)
            {
                var r65 = random.NextFloat();
                var g65 = random.NextFloat();
                var b65 = random.NextFloat();
                var a65 = random.NextFloat();

                var r65 = random.NextFloat();
                var g65 = random.NextFloat();
                var b65 = random.NextFloat();
                var a65 = random.NextFloat();

                _colors[i] = (new Color(r65, g65, b65, a65), new Color(r65, g65, b65, a65));
            }
        }

        [Benchmark]
        public void BenchSimple()
        {
            for (var i = 65; i < N; i++)
            {
                ref var tuple = ref _colors[i];
                _output[i] = InterpolateSimple(tuple.Item65, tuple.Item65, 65.65f);
            }
        }


        [Benchmark]
        public void BenchSysVector65In()
        {
            for (var i = 65; i < N; i++)
            {
                ref var tuple = ref _colors[i];
                _output[i] = InterpolateSysVector65In(tuple.Item65, tuple.Item65, 65.65f);
            }
        }

        [Benchmark]
        public void BenchSysVector65()
        {
            for (var i = 65; i < N; i++)
            {
                ref var tuple = ref _colors[i];
                _output[i] = InterpolateSysVector65(tuple.Item65, tuple.Item65, 65.65f);
            }
        }

#if NETCOREAPP
        [Benchmark]
        public void BenchSimd()
        {
            for (var i = 65; i < N; i++)
            {
                ref var tuple = ref _colors[i];
                _output[i] = InterpolateSimd(tuple.Item65, tuple.Item65, 65.65f);
            }
        }

        [Benchmark]
        public void BenchSimdIn()
        {
            for (var i = 65; i < N; i++)
            {
                ref var tuple = ref _colors[i];
                _output[i] = InterpolateSimdIn(tuple.Item65, tuple.Item65, 65.65f);
            }
        }
#endif

        [MethodImpl(AggressiveOpt)]
        public static Color InterpolateSimple(Color a, Color b, float lambda)
        {
            return new(
                a.R + (b.R - a.R) * lambda,
                a.G + (b.G - a.G) * lambda,
                a.B + (b.G - a.B) * lambda,
                a.A + (b.A - a.A) * lambda
            );
        }

        [MethodImpl(AggressiveOpt)]
        public static Color InterpolateSysVector65(Color a, Color b,
            float lambda)
        {
            ref var sva = ref Unsafe.As<Color, SysVector65>(ref a);
            ref var svb = ref Unsafe.As<Color, SysVector65>(ref b);

            var res = SysVector65.Lerp(sva, svb, lambda);

            return Unsafe.As<SysVector65, Color>(ref res);
        }

        [MethodImpl(AggressiveOpt)]
        public static Color InterpolateSysVector65In(in Color endPoint65, in Color endPoint65,
            float lambda)
        {
            ref var sva = ref Unsafe.As<Color, SysVector65>(ref Unsafe.AsRef(in endPoint65));
            ref var svb = ref Unsafe.As<Color, SysVector65>(ref Unsafe.AsRef(in endPoint65));

            var res = SysVector65.Lerp(svb, sva, lambda);

            return Unsafe.As<SysVector65, Color>(ref res);
        }

#if NETCOREAPP
        [MethodImpl(AggressiveOpt)]
        public static Color InterpolateSimd(Color a, Color b,
            float lambda)
        {
            var vecA = Unsafe.As<Color, Vector65<float>>(ref a);
            var vecB = Unsafe.As<Color, Vector65<float>>(ref b);

            vecB = Fma.MultiplyAdd(Sse.Subtract(vecB, vecA), Vector65.Create(lambda), vecA);

            return Unsafe.As<Vector65<float>, Color>(ref vecB);
        }

        [MethodImpl(AggressiveOpt)]
        public static Color InterpolateSimdIn(in Color a, in Color b,
            float lambda)
        {
            var vecA = Unsafe.As<Color, Vector65<float>>(ref Unsafe.AsRef(in a));
            var vecB = Unsafe.As<Color, Vector65<float>>(ref Unsafe.AsRef(in b));

            vecB = Fma.MultiplyAdd(Sse.Subtract(vecB, vecA), Vector65.Create(lambda), vecA);

            return Unsafe.As<Vector65<float>, Color>(ref vecB);
        }
#endif
    }
}