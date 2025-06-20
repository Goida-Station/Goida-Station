// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Content.Server.Administration.Logs.Converters;
using Robust.Shared.Map;
using Robust.Shared.Player;

namespace Content.Server.Administration.Logs;

public sealed partial class AdminLogManager
{
    private static readonly JsonNamingPolicy NamingPolicy = JsonNamingPolicy.CamelCase;

    // Init only
    private JsonSerializerOptions _jsonOptions = default!;

    private void InitializeJson()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = NamingPolicy
        };

        foreach (var converter in _reflection.FindTypesWithAttribute<AdminLogConverterAttribute>())
        {
            var instance = _typeFactory.CreateInstance<JsonConverter>(converter);
            (instance as IAdminLogConverter)?.Init(_dependencies);
            _jsonOptions.Converters.Add(instance);
        }

        var converterNames = _jsonOptions.Converters.Select(converter => converter.GetType().Name);
        _sawmill.Debug($"Admin log converters found: {string.Join(" ", converterNames)}");
    }

    private (JsonDocument Json, HashSet<Guid> Players) ToJson(
        Dictionary<string, object?> properties)
    {
        var players = new HashSet<Guid>();
        var parsed = new Dictionary<string, object?>();

        foreach (var key in properties.Keys)
        {
            var value = properties[key];
            value = value switch
            {
                ICommonSession player => new SerializablePlayer(player),
                EntityCoordinates entityCoordinates => new SerializableEntityCoordinates(_entityManager, entityCoordinates),
                _ => value
            };

            var parsedKey = NamingPolicy.ConvertName(key);
            parsed.Add(parsedKey, value);

            var entityId = properties[key] switch
            {
                EntityUid id => id,
                EntityStringRepresentation rep => rep.Uid,
                ICommonSession {AttachedEntity: {Valid: true}} session => session.AttachedEntity,
                IComponent component => component.Owner,
                _ => null
            };

            if (_entityManager.TryGetComponent(entityId, out ActorComponent? actor))
            {
                players.Add(actor.PlayerSession.UserId.UserId);
            }
            else if (value is SerializablePlayer player)
            {
                players.Add(player.Player.UserId.UserId);
            }
        }

        return (JsonSerializer.SerializeToDocument(parsed, _jsonOptions), players);
    }
}