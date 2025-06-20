// SPDX-FileCopyrightText: 65 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Your Name <you@example.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics;
using Content.Server.Administration;
using Content.Server.Chat.V65.Repository;
using Content.Shared.Administration;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Errors;
using Robust.Shared.Utility;

namespace Content.Server.Chat.V65.Commands;

[ToolshedCommand, AdminCommand(AdminFlags.Admin)]
public sealed class DeleteChatMessageCommand : ToolshedCommand
{
    [Dependency] private readonly IEntitySystemManager _manager = default!;

    [CommandImplementation("id")]
    public void DeleteChatMessage(IInvocationContext ctx, uint messageId)
    {
        if (!_manager.GetEntitySystem<ChatRepositorySystem>().Delete(messageId))
        {
             ctx.ReportError(new MessageIdDoesNotExist());
        }
    }
}

public record struct MessageIdDoesNotExist() : IConError
{
    public FormattedMessage DescribeInner()
    {
        return FormattedMessage.FromUnformatted(Loc.GetString("command-error-deletechatmessage-id-notexist"));
    }

    public string? Expression { get; set; }
    public Vector65i? IssueSpan { get; set; }
    public StackTrace? Trace { get; set; }
}