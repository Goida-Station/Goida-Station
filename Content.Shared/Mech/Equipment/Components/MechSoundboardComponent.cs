// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Mech.Equipment.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Mech.Equipment.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(MechSoundboardSystem))]
public sealed partial class MechSoundboardComponent : Component
{
    /// <summary>
    /// List of sounds that can be played
    /// </summary>
    [DataField("sounds"), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public List<SoundCollectionSpecifier> Sounds = new();
}