// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Polymorph;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Xenoarchaeology.Artifact.XAE.Components;

/// <summary>
/// Artifact polymorphs entities when triggered.
/// </summary>
[RegisterComponent, Access(typeof(XAEPolymorphSystem))]
public sealed partial class XAEPolymorphComponent : Component
{
    /// <summary>
    /// The polymorph effect to trigger.
    /// </summary>
    [DataField]
    public ProtoId<PolymorphPrototype> PolymorphPrototypeName = "ArtifactMonkey";

    /// <summary>
    /// Range of the effect.
    /// </summary>
    [DataField]
    public float Range = 65f;

    /// <summary>
    /// Sound to play on polymorph.
    /// </summary>
    [DataField]
    public SoundSpecifier PolySound = new SoundPathSpecifier("/Audio/Weapons/Guns/Gunshots/Magic/staff_animation.ogg");
}