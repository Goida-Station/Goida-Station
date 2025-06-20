// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Explosion.Components;

/// <summary>
///     Triggers when colliding with another entity.
/// </summary>
[RegisterComponent]
public sealed partial class TriggerOnCollideComponent : Component
{
    /// <summary>
    ///     The fixture with which to collide.
    /// </summary>
    [DataField(required: true)]
    public string FixtureID = string.Empty;

    /// <summary>
    ///     Doesn't trigger if the other colliding fixture is nonhard.
    /// </summary>
    [DataField]
    public bool IgnoreOtherNonHard = true;
}
