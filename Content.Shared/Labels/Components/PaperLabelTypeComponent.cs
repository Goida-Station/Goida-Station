// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Labels.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Labels.Components;

/// <summary>
/// Specifies the paper type (see textures/storage/crates/labels.rsi to see currently supported paper types)  to show on crates this label is attached to.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(LabelSystem))]
public sealed partial class PaperLabelTypeComponent : Component
{
    /// <summary>
    /// The type of label to show.
    /// </summary>
    [DataField]
    public string PaperType = "Paper";
}