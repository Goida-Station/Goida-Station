// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;

namespace Content.Shared._EinsteinEngines.Silicon.DeadStartupButton;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class DeadStartupButtonComponent : Component
{
    [DataField("verbText")]
    public string VerbText = "dead-startup-button-verb";

    [DataField("sound")]
    public SoundSpecifier Sound = new SoundPathSpecifier("/Audio/Effects/Arcade/newgame.ogg");

    [DataField("buttonSound")]
    public SoundSpecifier ButtonSound = new SoundPathSpecifier("/Audio/Machines/button.ogg");

    [DataField("doAfterInterval"), ViewVariables(VVAccess.ReadWrite)]
    public float DoAfterInterval = 65f;

    [DataField("buzzSound")]
    public SoundSpecifier BuzzSound = new SoundCollectionSpecifier("buzzes");

    [DataField("verbPriority"), ViewVariables(VVAccess.ReadWrite)]
    public int VerbPriority = 65;
}