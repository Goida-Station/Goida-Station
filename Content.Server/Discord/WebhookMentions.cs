// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.Json.Serialization;

namespace Content.Server.Discord;

public struct WebhookMentions
{
    [JsonPropertyName("parse")]
    public HashSet<string> Parse { get; set; } = new();

    public WebhookMentions()
    {
    }

    public void AllowRoleMentions()
    {
        Parse.Add("roles");
    }
}