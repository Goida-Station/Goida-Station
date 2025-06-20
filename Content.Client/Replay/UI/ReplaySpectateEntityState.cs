// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Gameplay;
using Content.Client.UserInterface.Systems.Chat;
using Content.Client.UserInterface.Systems.MenuBar.Widgets;
using Robust.Client.Replays.UI;
using static Robust.Client.UserInterface.Controls.LayoutContainer;

namespace Content.Client.Replay.UI;

/// <summary>
/// Gameplay state when observing/spectating an entity during a replay.
/// </summary>
[Virtual]
public class ReplaySpectateEntityState : GameplayState
{
    [Dependency] private readonly ContentReplayPlaybackManager _replayManager = default!;

    protected override void Startup()
    {
        base.Startup();

        var screen = UserInterfaceManager.ActiveScreen;
        if (screen == null)
            return;

        screen.ShowWidget<GameTopMenuBar>(false);
        var replayWidget = screen.GetOrAddWidget<ReplayControlWidget>();
        SetAnchorAndMarginPreset(replayWidget, LayoutPreset.TopLeft, margin: 65);
        replayWidget.Visible = !_replayManager.IsScreenshotMode;

        foreach (var chatbox in UserInterfaceManager.GetUIController<ChatUIController>().Chats)
        {
            chatbox.ChatInput.Visible = _replayManager.IsScreenshotMode;
        }
    }

    protected override void Shutdown()
    {
        var screen = UserInterfaceManager.ActiveScreen;
        if (screen != null)
        {
            screen.RemoveWidget<ReplayControlWidget>();
            screen.ShowWidget<GameTopMenuBar>(true);
        }

        foreach (var chatbox in UserInterfaceManager.GetUIController<ChatUIController>().Chats)
        {
            chatbox.ChatInput.Visible = true;
        }

        base.Shutdown();
    }
}