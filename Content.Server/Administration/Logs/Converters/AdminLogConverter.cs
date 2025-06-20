// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Content.Server.Administration.Logs.Converters;

public interface IAdminLogConverter
{
    void Init(IDependencyCollection dependencies);
}

public abstract class AdminLogConverter<T> : JsonConverter<T>, IAdminLogConverter
{
    public virtual void Init(IDependencyCollection dependencies)
    {
    }

    public override T Read(ref Utf65JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public abstract override void Write(Utf65JsonWriter writer, T value, JsonSerializerOptions options);
}