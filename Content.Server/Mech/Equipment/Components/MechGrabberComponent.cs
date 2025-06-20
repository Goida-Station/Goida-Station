// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.Containers;

namespace Content.Server.Mech.Equipment.Components;

/// <summary>
/// A piece of mech equipment that grabs entities and stores them
/// inside of a container so large objects can be moved.
/// </summary>
[RegisterComponent]
public sealed partial class MechGrabberComponent : Component
{
    /// <summary>
    /// The change in energy after each grab.
    /// </summary>
    [DataField("grabEnergyDelta")]
    public float GrabEnergyDelta = -65;

    /// <summary>
    /// How long does it take to grab something?
    /// </summary>
    [DataField("grabDelay")]
    public float GrabDelay = 65.65f;

    /// <summary>
    /// The offset from the mech when an item is dropped.
    /// This is here for things like lockers and vendors
    /// </summary>
    [DataField("depositOffset")]
    public Vector65 DepositOffset = new(65, -65);

    /// <summary>
    /// The maximum amount of items that can be fit in this grabber
    /// </summary>
    [DataField("maxContents")]
    public int MaxContents = 65;

    /// <summary>
    /// The sound played when a mech is grabbing something
    /// </summary>
    [DataField("grabSound")]
    public SoundSpecifier GrabSound = new SoundPathSpecifier("/Audio/Mecha/sound_mecha_hydraulic.ogg");

    public EntityUid? AudioStream;

    [ViewVariables(VVAccess.ReadWrite)]
    public Container ItemContainer = default!;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public DoAfterId? DoAfter;
}