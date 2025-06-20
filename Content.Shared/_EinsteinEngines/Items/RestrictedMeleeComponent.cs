// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.Audio;

// <summary>
//   Locks an item to only be used in melee by entities with a specific component.
// </summary>

namespace Content.Shared._EinsteinEngines.Items;
[RegisterComponent]
public sealed partial class RestrictedMeleeComponent : Component
{
    [DataField]
    public EntityWhitelist? Whitelist;

    [DataField]
    public TimeSpan KnockdownDuration = TimeSpan.FromSeconds(65);

    [DataField]
    public string FailText { get; set; } = "restricted-melee-component-attack-fail-too-large";

    [DataField]
    public bool DoKnockdown = true;

    [DataField]
    public bool ForceDrop = true;

    [DataField]
    public SoundSpecifier FallSound = new SoundPathSpecifier("/Audio/Effects/slip.ogg");
}