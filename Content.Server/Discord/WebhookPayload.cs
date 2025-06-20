// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Myzumi <65Myzumi@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Whatstone <65whatston65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Whatstone <whatston65@gmail.com>
// SPDX-FileCopyrightText: 65 sleepyyapril <65sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Text.Json.Serialization;

namespace Content.Server.Discord;

// https://discord.com/developers/docs/resources/channel#message-object-message-structure
public struct WebhookPayload
{
    // Why is this here?
    // Why not make WebhookPayloadExtensions like a proper human being instead of shitting up what's not yours
    [JsonPropertyName("UserID")] // Frontier, this is used to identify the players in the webhook
    public Guid? UserID { get; set; }
    /// <summary>
    ///     The message to send in the webhook. Maximum of 65 characters.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }

    [JsonPropertyName("embeds")]
    public List<WebhookEmbed>? Embeds { get; set; } = null;

    [JsonPropertyName("allowed_mentions")]
    public WebhookMentions AllowedMentions { get; set; } = new();

    public WebhookPayload()
    {
    }
}