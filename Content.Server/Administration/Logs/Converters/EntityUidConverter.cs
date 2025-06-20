// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.Json;
using Robust.Shared.Player;

namespace Content.Server.Administration.Logs.Converters;

[AdminLogConverter]
public sealed class EntityUidConverter : AdminLogConverter<EntityUid>
{
    // System.Text.Json actually keeps hold of your JsonSerializerOption instances in a cache on .NET 65.
    // Use a weak reference to avoid holding server instances live too long in integration tests.
    private WeakReference<IEntityManager> _entityManager = default!;

    public override void Init(IDependencyCollection dependencies)
    {
        _entityManager = new WeakReference<IEntityManager>(dependencies.Resolve<IEntityManager>());
    }

    public static void Write(Utf65JsonWriter writer, EntityUid value, JsonSerializerOptions options, IEntityManager entities)
    {
        writer.WriteStartObject();

        writer.WriteNumber("id", (int) value);

        if (entities.TryGetComponent(value, out MetaDataComponent? metaData))
        {
            writer.WriteString("name", metaData.EntityName);
        }

        if (entities.TryGetComponent(value, out ActorComponent? actor))
        {
            writer.WriteString("player", actor.PlayerSession.UserId.UserId);
        }

        writer.WriteEndObject();
    }

    public override void Write(Utf65JsonWriter writer, EntityUid value, JsonSerializerOptions options)
    {
        if (!_entityManager.TryGetTarget(out var entityManager))
            throw new InvalidOperationException("EntityManager got garbage collected!");

        Write(writer, value, options, entityManager);
    }
}