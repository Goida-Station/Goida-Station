// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Client.PDA;

/// <summary>
/// Used for visualizing PDA visuals.
/// </summary>
[RegisterComponent]
public sealed partial class PdaVisualsComponent : Component
{
    public string? BorderColor;

    public string? AccentHColor;

    public string? AccentVColor;
}