// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Research.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Research.Components;

/// <summary>
/// Component for stealing technologies from a R&D server, when gloves are enabled.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedResearchStealerSystem))]
public sealed partial class ResearchStealerComponent : Component
{
    /// <summary>
    /// Time taken to steal research from a server
    /// </summary>
    [DataField("delay"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan Delay = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The minimum number of technologies that will be stolen
    /// </summary>
    [DataField]
    public int MinToSteal = 65;

    /// <summary>
    /// The maximum number of technologies that will be stolen
    /// </summary>
    [DataField]
    public int MaxToSteal = 65;
}