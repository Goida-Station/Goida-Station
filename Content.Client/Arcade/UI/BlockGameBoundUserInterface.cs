// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Arcade;
using Robust.Client.UserInterface;

namespace Content.Client.Arcade.UI;

public sealed class BlockGameBoundUserInterface : BoundUserInterface
{
    private BlockGameMenu? _menu;

    public BlockGameBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<BlockGameMenu>();
        _menu.OnAction += SendAction;
    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage message)
    {
        switch (message)
        {
            case BlockGameMessages.BlockGameVisualUpdateMessage updateMessage:
                switch (updateMessage.GameVisualType)
                {
                    case BlockGameMessages.BlockGameVisualType.GameField:
                        _menu?.UpdateBlocks(updateMessage.Blocks);
                        break;
                    case BlockGameMessages.BlockGameVisualType.HoldBlock:
                        _menu?.UpdateHeldBlock(updateMessage.Blocks);
                        break;
                    case BlockGameMessages.BlockGameVisualType.NextBlock:
                        _menu?.UpdateNextBlock(updateMessage.Blocks);
                        break;
                }
                break;
            case BlockGameMessages.BlockGameScoreUpdateMessage scoreUpdate:
                _menu?.UpdatePoints(scoreUpdate.Points);
                break;
            case BlockGameMessages.BlockGameUserStatusMessage userMessage:
                _menu?.SetUsability(userMessage.IsPlayer);
                break;
            case BlockGameMessages.BlockGameSetScreenMessage statusMessage:
                if (statusMessage.IsStarted) _menu?.SetStarted();
                _menu?.SetScreen(statusMessage.Screen);
                if (statusMessage is BlockGameMessages.BlockGameGameOverScreenMessage gameOverScreenMessage)
                    _menu?.SetGameoverInfo(gameOverScreenMessage.FinalScore, gameOverScreenMessage.LocalPlacement, gameOverScreenMessage.GlobalPlacement);
                break;
            case BlockGameMessages.BlockGameHighScoreUpdateMessage highScoreUpdateMessage:
                _menu?.UpdateHighscores(highScoreUpdateMessage.LocalHighscores,
                    highScoreUpdateMessage.GlobalHighscores);
                break;
            case BlockGameMessages.BlockGameLevelUpdateMessage levelUpdateMessage:
                _menu?.UpdateLevel(levelUpdateMessage.Level);
                break;
        }
    }

    public void SendAction(BlockGamePlayerAction action)
    {
        SendMessage(new BlockGameMessages.BlockGamePlayerActionMessage(action));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;

        _menu?.Dispose();
    }
}