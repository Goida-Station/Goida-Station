// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Shuttles.UI.MapObjects;

public record struct GridMapObject : IMapObject
{
    public string Name { get; set; }
    public bool HideButton { get; init; }
    public EntityUid Entity;
}