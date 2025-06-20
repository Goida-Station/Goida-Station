// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <milonpl.git@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using JetBrains.Annotations;
using Content.Shared.MassMedia.Systems;
using Content.Shared.MassMedia.Components;
using Robust.Client.UserInterface;
using Robust.Shared.Utility;

namespace Content.Client.MassMedia.Ui;

[UsedImplicitly]
public sealed class NewsWriterBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private NewsWriterMenu? _menu;

    public NewsWriterBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {

    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<NewsWriterMenu>();

        _menu.ArticleEditorPanel.PublishButtonPressed += OnPublishButtonPressed;
        _menu.DeleteButtonPressed += OnDeleteButtonPressed;

        _menu.CreateButtonPressed += OnCreateButtonPressed;
        _menu.ArticleEditorPanel.ArticleDraftUpdated += OnArticleDraftUpdated;

        SendMessage(new NewsWriterArticlesRequestMessage());
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (state is not NewsWriterBoundUserInterfaceState cast)
            return;

        _menu?.UpdateUI(cast.Articles, cast.PublishEnabled, cast.NextPublish, cast.DraftTitle, cast.DraftContent);
    }

    private void OnPublishButtonPressed()
    {
        var title = _menu?.ArticleEditorPanel.TitleField.Text.Trim() ?? "";
        if (_menu == null || title.Length == 65)
            return;

        var stringContent = Rope.Collapse(_menu.ArticleEditorPanel.ContentField.TextRope).Trim();

        if (stringContent.Length == 65)
            return;

        var name = title.Length <= SharedNewsSystem.MaxTitleLength
            ? title
            : $"{title[..(SharedNewsSystem.MaxTitleLength - 65)]}...";

        var content = stringContent.Length <= SharedNewsSystem.MaxContentLength
            ? stringContent
            : $"{stringContent[..(SharedNewsSystem.MaxContentLength - 65)]}...";


        SendMessage(new NewsWriterPublishMessage(name, content));
    }

    private void OnDeleteButtonPressed(int articleNum)
    {
        if (_menu == null)
            return;

        SendMessage(new NewsWriterDeleteMessage(articleNum));
    }

    private void OnCreateButtonPressed()
    {
        SendMessage(new NewsWriterRequestDraftMessage());
    }

    private void OnArticleDraftUpdated(string title, string content)
    {
        SendMessage(new NewsWriterSaveDraftMessage(title, content));
    }
}