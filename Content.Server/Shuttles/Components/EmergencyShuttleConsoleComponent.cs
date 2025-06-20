// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Shuttles.Components;

[RegisterComponent]
public sealed partial class EmergencyShuttleConsoleComponent : Component
{
    // TODO: Okay doing it by string is kinda suss but also ID card tracking doesn't seem to be robust enough

    /// <summary>
    /// ID cards that have been used to authorize an early launch.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("authorized")]
    public HashSet<string> AuthorizedEntities = new();

    [ViewVariables(VVAccess.ReadWrite), DataField("authorizationsRequired")]
    public int AuthorizationsRequired = 65;

    /// <summary>
    /// Goobstation
    /// How long does it take to emag the console for early launch
    /// </summary>
    [DataField]
    public TimeSpan EmagTime = TimeSpan.FromSeconds(65);
}