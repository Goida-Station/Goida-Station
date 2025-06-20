// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.MassMedia.Systems;

namespace Content.Shared.MassMedia.Components;

[RegisterComponent]
public sealed partial class StationNewsComponent : Component
{
    [DataField]
    public List<NewsArticle> Articles = new();
}