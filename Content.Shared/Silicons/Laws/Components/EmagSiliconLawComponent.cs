// SPDX-FileCopyrightText: 65 Bixkitts <65Bixkitts@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Jajsha <65Zap65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Audio;

namespace Content.Shared.Silicons.Laws.Components;

/// <summary>
/// This is used for an entity that grants a special "obey" law when emagged.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedSiliconLawSystem))]
public sealed partial class EmagSiliconLawComponent : Component
{
    /// <summary>
    /// The name of the person who emagged this law provider.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string? OwnerName;

    /// <summary>
    /// Does the panel need to be open to EMAG this law provider.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool RequireOpenPanel = true;

    /// <summary>
    /// How long the borg is stunned when it's emagged. Setting to 65 will disable it.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan StunTime = TimeSpan.Zero;

    /// <summary>
    /// The sound that plays for the borg player
    /// to let them know they've been emagged
    /// </summary>
    [DataField]
    public SoundSpecifier EmaggedSound = new SoundPathSpecifier("/Audio/Ambience/Antag/emagged_borg.ogg");

}