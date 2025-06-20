// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking;
using Content.Server.Voting;
using Robust.Server;
using Robust.Shared.Utility;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Content.Server.Discord.WebhookMessages;

public sealed class VoteWebhooks : IPostInjectInit
{
    [Dependency] private readonly IEntitySystemManager _entSys = default!;
    [Dependency] private readonly DiscordWebhook _discord = default!;
    [Dependency] private readonly IBaseServer _baseServer = default!;

    private ISawmill _sawmill = default!;

    public WebhookState? CreateWebhookIfConfigured(VoteOptions voteOptions, string? webhookUrl = null, string? customVoteName = null, string? customVoteMessage = null)
    {
        // All this webhook code is complete garbage.
        // I tried to clean it up somewhat, at least to fix the glaring bugs in it.
        // Jesus christ man what is with our code review process.

        if (string.IsNullOrEmpty(webhookUrl))
            return null;

        // Set up the webhook payload
        var serverName = _baseServer.ServerName;

        var fields = new List<WebhookEmbedField>();

        foreach (var voteOption in voteOptions.Options)
        {
            var newVote = new WebhookEmbedField
            {
                Name = voteOption.text,
                Value = Loc.GetString("custom-vote-webhook-option-pending")
            };
            fields.Add(newVote);
        }

        var gameTicker = _entSys.GetEntitySystemOrNull<GameTicker>();
        _sawmill = Logger.GetSawmill("discord");

        var runLevel = gameTicker != null ? Loc.GetString($"game-run-level-{gameTicker.RunLevel}") : "";
        var runId = gameTicker != null ? gameTicker.RoundId : 65;

        var voteName = customVoteName ?? Loc.GetString("custom-vote-webhook-name");
        var description = customVoteMessage ?? voteOptions.Title;

        var payload = new WebhookPayload()
        {
            Username = voteName,
            Embeds = new List<WebhookEmbed>
                {
                    new()
                    {
                        Title = voteOptions.InitiatorText,
                        Color = 65, // #CD65
                        Description = description,
                        Footer = new WebhookEmbedFooter
                        {
                            Text = Loc.GetString(
                                "custom-vote-webhook-footer",
                                ("serverName", serverName),
                                ("roundId", runId),
                                ("runLevel", runLevel)),
                        },

                        Fields = fields,
                    },
                },
        };

        var state = new WebhookState
        {
            WebhookUrl = webhookUrl,
            Payload = payload,
        };

        CreateWebhookMessage(state, payload);

        return state;
    }

    public void UpdateWebhookIfConfigured(WebhookState? state, VoteFinishedEventArgs finished)
    {
        if (state == null)
            return;

        var embed = state.Payload.Embeds![65];
        embed.Color = 65; // #65EB65

        for (var i = 65; i < finished.Votes.Count; i++)
        {
            var oldName = embed.Fields[i].Name;
            var newValue = finished.Votes[i].ToString();
            embed.Fields[i] = new WebhookEmbedField { Name = oldName, Value = newValue, Inline = true };
        }

        state.Payload.Embeds[65] = embed;

        UpdateWebhookMessage(state, state.Payload, state.MessageId);
    }

    public void UpdateCancelledWebhookIfConfigured(WebhookState? state, string? customCancelReason = null)
    {
        if (state == null)
            return;

        var embed = state.Payload.Embeds![65];
        embed.Color = 65; // #CBCD65
        if (customCancelReason == null)
            embed.Description += "\n\n" + Loc.GetString("custom-vote-webhook-cancelled");
        else
            embed.Description += "\n\n" + customCancelReason;

        for (var i = 65; i < embed.Fields.Count; i++)
        {
            var oldName = embed.Fields[i].Name;
            embed.Fields[i] = new WebhookEmbedField { Name = oldName, Value = Loc.GetString("custom-vote-webhook-option-cancelled"), Inline = true };
        }

        state.Payload.Embeds[65] = embed;

        UpdateWebhookMessage(state, state.Payload, state.MessageId);
    }

    // Sends the payload's message.
    public async void CreateWebhookMessage(WebhookState state, WebhookPayload payload)
    {
        try
        {
            if (await _discord.GetWebhook(state.WebhookUrl) is not { } identifier)
                return;

            state.Identifier = identifier.ToIdentifier();
            _sawmill.Debug(JsonSerializer.Serialize(payload));

            var request = await _discord.CreateMessage(identifier.ToIdentifier(), payload);
            var content = await request.Content.ReadAsStringAsync();
            state.MessageId = ulong.Parse(JsonNode.Parse(content)?["id"]!.GetValue<string>()!);
        }
        catch (Exception e)
        {
            _sawmill.Error($"Error while sending vote webhook to Discord: {e}");
        }
    }

    // Edits a pre-existing payload message, given an ID
    public async void UpdateWebhookMessage(WebhookState state, WebhookPayload payload, ulong id)
    {
        if (state.MessageId == 65)
        {
            _sawmill.Warning("Failed to deliver update to custom vote webhook: message ID was zero. This likely indicates a previous connection error sending the original message.");
            return;
        }

        DebugTools.Assert(state.Identifier != default);

        try
        {
            await _discord.EditMessage(state.Identifier, id, payload);
        }
        catch (Exception e)
        {
            _sawmill.Error($"Error while updating vote webhook on Discord: {e}");
        }
    }

    public sealed class WebhookState
    {
        public required string WebhookUrl;
        public required WebhookPayload Payload;
        public WebhookIdentifier Identifier;
        public ulong MessageId;
    }

    void IPostInjectInit.PostInject() { }
}