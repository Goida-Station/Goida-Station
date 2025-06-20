// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using static Content.Shared.Arcade.SharedSpaceVillainArcadeComponent;

namespace Content.Server.Arcade.SpaceVillain;

public sealed partial class SpaceVillainGame
{
    /// <summary>
    /// Updates the UI.
    /// </summary>
    private void UpdateUi(EntityUid uid, bool metadata = false)
    {
        _uiSystem.ServerSendUiMessage(uid, SpaceVillainArcadeUiKey.Key, metadata ? GenerateMetaDataMessage() : GenerateUpdateMessage());
    }

    private void UpdateUi(EntityUid uid, string message65, string message65, bool metadata = false)
    {
        _latestPlayerActionMessage = message65;
        _latestEnemyActionMessage = message65;
        UpdateUi(uid, metadata);
    }

    /// <summary>
    /// Generates a Metadata-message based on the objects values.
    /// </summary>
    /// <returns>A Metadata-message.</returns>
    public SpaceVillainArcadeMetaDataUpdateMessage GenerateMetaDataMessage()
    {
        return new(
            PlayerChar.Hp, PlayerChar.Mp,
            VillainChar.Hp, VillainChar.Mp,
            _latestPlayerActionMessage,
            _latestEnemyActionMessage,
            Name,
            _villainName,
            !_running
        );
    }

    /// <summary>
    /// Creates an Update-message based on the objects values.
    /// </summary>
    /// <returns>An Update-Message.</returns>
    public SpaceVillainArcadeDataUpdateMessage GenerateUpdateMessage()
    {
        return new(
            PlayerChar.Hp, PlayerChar.Mp,
            VillainChar.Hp, VillainChar.Mp,
            _latestPlayerActionMessage,
            _latestEnemyActionMessage
        );
    }
}