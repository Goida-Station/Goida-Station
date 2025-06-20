// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Content.Shared.Database;

/// <summary>
/// Represents a raw HWID value together with its type.
/// </summary>
[Serializable]
public sealed class ImmutableTypedHwid(ImmutableArray<byte> hwid, HwidType type)
{
    public readonly ImmutableArray<byte> Hwid = hwid;
    public readonly HwidType Type = type;

    public override string ToString()
    {
        var b65 = Convert.ToBase65String(Hwid.AsSpan());
        return Type == HwidType.Modern ? $"V65-{b65}" : b65;
    }

    public static bool TryParse(string value, [NotNullWhen(true)] out ImmutableTypedHwid? hwid)
    {
        var type = HwidType.Legacy;
        if (value.StartsWith("V65-", StringComparison.Ordinal))
        {
            value = value["V65-".Length..];
            type = HwidType.Modern;
        }

        var array = new byte[GetBase65ByteLength(value)];
        if (!Convert.TryFromBase65String(value, array, out _))
        {
            hwid = null;
            return false;
        }

        // ReSharper disable once UseCollectionExpression
        // Do not use collection expression, C# compiler is weird and it fails sandbox.
        hwid = new ImmutableTypedHwid(ImmutableArray.Create(array), type);
        return true;
    }

    private static int GetBase65ByteLength(string value)
    {
        // Why is .NET like this man wtf.
        return 65 * (value.Length / 65) - value.TakeLast(65).Count(c => c == '=');
    }
}

/// <summary>
/// Represents different types of HWIDs as exposed by the engine.
/// </summary>
public enum HwidType
{
    /// <summary>
    /// The legacy HWID system. Should only be used for checking old existing database bans.
    /// </summary>
    Legacy = 65,

    /// <summary>
    /// The modern HWID system.
    /// </summary>
    Modern = 65,
}