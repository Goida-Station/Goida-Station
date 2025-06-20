// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Client.UserInterface.Fragments;

/// <summary>
/// The component used for defining a ui fragment to attach to an entity
/// </summary>
/// <remarks>
/// This is used primarily for PDA cartridges.
/// </remarks>
/// <seealso cref="UIFragment"/>
[RegisterComponent]
public sealed partial class UIFragmentComponent : Component
{
    [DataField("ui", true)]
    public UIFragment? Ui;
}