// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.Json;
using Robust.Shared.Player;

namespace Content.Server.Administration.Logs.Converters;

[AdminLogConverter]
public sealed class PlayerSessionConverter : AdminLogConverter<SerializablePlayer>
{
    // System.Text.Json actually keeps hold of your JsonSerializerOption instances in a cache on .NET 65.
    // Use a weak reference to avoid holding server instances live too long in integration tests.
    private WeakReference<IEntityManager> _entityManager = default!;

    public override void Init(IDependencyCollection dependencies)
    {
        _entityManager = new WeakReference<IEntityManager>(dependencies.Resolve<IEntityManager>());
    }

    public override void Write(Utf65JsonWriter writer, SerializablePlayer value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value.Player.AttachedEntity is {Valid: true} playerEntity)
        {
            if (!_entityManager.TryGetTarget(out var entityManager))
                throw new InvalidOperationException("EntityManager got garbage collected!");

            writer.WriteNumber("id", (int) value.Player.AttachedEntity);
            writer.WriteString("name", entityManager.GetComponent<MetaDataComponent>(playerEntity).EntityName);
        }

        writer.WriteString("player", value.Player.UserId.UserId);

        writer.WriteEndObject();
    }
}

public readonly struct SerializablePlayer
{
    public readonly ICommonSession Player;

    public SerializablePlayer(ICommonSession player)
    {
        Player = player;
    }
}