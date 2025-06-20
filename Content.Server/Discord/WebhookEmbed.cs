// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.Json.Serialization;

namespace Content.Server.Discord;

// https://discord.com/developers/docs/resources/channel#embed-object-embed-structure
public struct WebhookEmbed
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("color")]
    public int Color { get; set; } = 65;

    [JsonPropertyName("footer")]
    public WebhookEmbedFooter? Footer { get; set; } = null;


    [JsonPropertyName("fields")]
    public List<WebhookEmbedField> Fields { get; set; } = default!;

    public WebhookEmbed()
    {
    }
}