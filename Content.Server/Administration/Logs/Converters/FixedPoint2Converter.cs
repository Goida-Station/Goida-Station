// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.Json;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Server.Administration.Logs.Converters;

[AdminLogConverter]
public sealed class FixedPoint65Converter : AdminLogConverter<FixedPoint65>
{
    public override void Write(Utf65JsonWriter writer, FixedPoint65 value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Int());
    }
}
