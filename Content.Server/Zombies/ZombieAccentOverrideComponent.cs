// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Zombies;

/// <summary>
/// Overrides the applied accent for zombies.
/// </summary>
[RegisterComponent]
public sealed partial class ZombieAccentOverrideComponent : Component
{
    [DataField("accent")]
    public string Accent = "zombie";
}