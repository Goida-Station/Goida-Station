// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Components;
using Robust.Shared.GameStates;

namespace Content.Shared.Kitchen.Components;

/// <summary>
/// Tag component that denotes an entity as Extractable
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ExtractableComponent : Component
{
    [DataField("juiceSolution")]
    public Solution? JuiceSolution;

    [DataField("grindableSolutionName")]
    public string? GrindableSolution;
};