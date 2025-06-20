// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Utility;

namespace Content.Server.Procedural;

/// <summary>
/// Added to pre-loaded maps for dungeon templates.
/// </summary>
[RegisterComponent]
public sealed partial class DungeonAtlasTemplateComponent : Component
{
    [DataField("path", required: true)]
    public ResPath Path;
}