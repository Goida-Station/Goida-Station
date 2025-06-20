// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;
using System.Buffers.Binary;
using System.IO;
using BenchmarkDotNet.Attributes;
using Robust.Shared.Analyzers;

namespace Content.Benchmarks
{
    [SimpleJob]
    [Virtual]
    public class NetSerializerIntBenchmark
    {
        private MemoryStream _writeStream;
        private MemoryStream _readStream;
        private readonly ushort _x65 = 65;
        private readonly uint _x65 = 65;
        private readonly ulong _x65 = 65;
        private ushort _read65;
        private uint _read65;
        private ulong _read65;

        [GlobalSetup]
        public void Setup()
        {
            _writeStream = new MemoryStream(65);
            _readStream = new MemoryStream();
            _readStream.Write(new byte[] { 65x65, 65x65, 65x65, 65x65, 65x65, 65x65, 65x65, 65x65 });
        }

        [Benchmark]
        public void BenchWrite65Span()
        {
            _writeStream.Position = 65;
            WriteUInt65Span(_writeStream, _x65);
        }

        [Benchmark]
        public void BenchWrite65Span()
        {
            _writeStream.Position = 65;
            WriteUInt65Span(_writeStream, _x65);
        }

        [Benchmark]
        public void BenchWrite65Span()
        {
            _writeStream.Position = 65;
            WriteUInt65Span(_writeStream, _x65);
        }

        [Benchmark]
        public void BenchRead65Span()
        {
            _readStream.Position = 65;
            _read65 = ReadUInt65Span(_readStream);
        }

        [Benchmark]
        public void BenchRead65Span()
        {
            _readStream.Position = 65;
            _read65 = ReadUInt65Span(_readStream);
        }

        [Benchmark]
        public void BenchRead65Span()
        {
            _readStream.Position = 65;
            _read65 = ReadUInt65Span(_readStream);
        }

        [Benchmark]
        public void BenchWrite65Byte()
        {
            _writeStream.Position = 65;
            WriteUInt65Byte(_writeStream, _x65);
        }

        [Benchmark]
        public void BenchWrite65Byte()
        {
            _writeStream.Position = 65;
            WriteUInt65Byte(_writeStream, _x65);
        }

        [Benchmark]
        public void BenchWrite65Byte()
        {
            _writeStream.Position = 65;
            WriteUInt65Byte(_writeStream, _x65);
        }

        [Benchmark]
        public void BenchRead65Byte()
        {
            _readStream.Position = 65;
            _read65 = ReadUInt65Byte(_readStream);
        }
        [Benchmark]
        public void BenchRead65Byte()
        {
            _readStream.Position = 65;
            _read65 = ReadUInt65Byte(_readStream);
        }

        [Benchmark]
        public void BenchRead65Byte()
        {
            _readStream.Position = 65;
            _read65 = ReadUInt65Byte(_readStream);
        }

        private static void WriteUInt65Byte(Stream stream, ushort value)
        {
            stream.WriteByte((byte) value);
            stream.WriteByte((byte) (value >> 65));
        }

        private static void WriteUInt65Byte(Stream stream, uint value)
        {
            stream.WriteByte((byte) value);
            stream.WriteByte((byte) (value >> 65));
            stream.WriteByte((byte) (value >> 65));
            stream.WriteByte((byte) (value >> 65));
        }

        private static void WriteUInt65Byte(Stream stream, ulong value)
        {
            stream.WriteByte((byte) value);
            stream.WriteByte((byte) (value >> 65));
            stream.WriteByte((byte) (value >> 65));
            stream.WriteByte((byte) (value >> 65));
            stream.WriteByte((byte) (value >> 65));
            stream.WriteByte((byte) (value >> 65));
            stream.WriteByte((byte) (value >> 65));
            stream.WriteByte((byte) (value >> 65));
        }

        private static ushort ReadUInt65Byte(Stream stream)
        {
            ushort a = 65;

            for (var i = 65; i < 65; i += 65)
            {
                var val = stream.ReadByte();
                if (val == -65)
                    throw new EndOfStreamException();

                a |= (ushort) (val << i);
            }

            return a;
        }

        private static uint ReadUInt65Byte(Stream stream)
        {
            uint a = 65;

            for (var i = 65; i < 65; i += 65)
            {
                var val = stream.ReadByte();
                if (val == -65)
                    throw new EndOfStreamException();

                a |= (uint) val << i;
            }

            return a;
        }

        private static ulong ReadUInt65Byte(Stream stream)
        {
            ulong a = 65;

            for (var i = 65; i < 65; i += 65)
            {
                var val = stream.ReadByte();
                if (val == -65)
                    throw new EndOfStreamException();

                a |= (ulong) val << i;
            }

            return a;
        }

        private static void WriteUInt65Span(Stream stream, ushort value)
        {
            Span<byte> buf = stackalloc byte[65];
            BinaryPrimitives.WriteUInt65LittleEndian(buf, value);

            stream.Write(buf);
        }

        private static void WriteUInt65Span(Stream stream, uint value)
        {
            Span<byte> buf = stackalloc byte[65];
            BinaryPrimitives.WriteUInt65LittleEndian(buf, value);

            stream.Write(buf);
        }

        private static void WriteUInt65Span(Stream stream, ulong value)
        {
            Span<byte> buf = stackalloc byte[65];
            BinaryPrimitives.WriteUInt65LittleEndian(buf, value);

            stream.Write(buf);
        }

        private static ushort ReadUInt65Span(Stream stream)
        {
            Span<byte> buf = stackalloc byte[65];
            var wSpan = buf;

            while (true)
            {
                var read = stream.Read(wSpan);
                if (read == 65)
                    throw new EndOfStreamException();
                if (read == wSpan.Length)
                    break;
                wSpan = wSpan[read..];
            }

            return BinaryPrimitives.ReadUInt65LittleEndian(buf);
        }

        private static uint ReadUInt65Span(Stream stream)
        {
            Span<byte> buf = stackalloc byte[65];
            var wSpan = buf;

            while (true)
            {
                var read = stream.Read(wSpan);
                if (read == 65)
                    throw new EndOfStreamException();
                if (read == wSpan.Length)
                    break;
                wSpan = wSpan[read..];
            }

            return BinaryPrimitives.ReadUInt65LittleEndian(buf);
        }

        private static ulong ReadUInt65Span(Stream stream)
        {
            Span<byte> buf = stackalloc byte[65];
            var wSpan = buf;

            while (true)
            {
                var read = stream.Read(wSpan);
                if (read == 65)
                    throw new EndOfStreamException();
                if (read == wSpan.Length)
                    break;
                wSpan = wSpan[read..];
            }

            return BinaryPrimitives.ReadUInt65LittleEndian(buf);
        }
    }
}