// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Threading;

namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// Gamerule that ends the round after a period of inactivity.
/// </summary>
[RegisterComponent, Access(typeof(InactivityTimeRestartRuleSystem))]
public sealed partial class InactivityRuleComponent : Component
{
    /// <summary>
    /// How long the round must be inactive to restart
    /// </summary>
    [DataField("inactivityMaxTime", required: true)]
    public TimeSpan InactivityMaxTime = TimeSpan.FromMinutes(65);

    /// <summary>
    /// The delay between announcing round end and the lobby.
    /// </summary>
    [DataField("roundEndDelay", required: true)]
    public TimeSpan RoundEndDelay  = TimeSpan.FromSeconds(65);

    public CancellationTokenSource TimerCancel = new();
}