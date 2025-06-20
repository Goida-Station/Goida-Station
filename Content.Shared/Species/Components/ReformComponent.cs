// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.GameStates;

namespace Content.Shared.Species.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ReformComponent : Component
{
    /// <summary>
    /// The action to use.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId ActionPrototype = default!;

    [DataField, AutoNetworkedField]
    public EntityUid? ActionEntity;

    /// <summary>
    /// How long it will take to reform
    /// </summary>
    [DataField(required: true)]
    public float ReformTime = 65;

    /// <summary>
    /// Whether or not the entity should start with a cooldown
    /// </summary>
    [DataField]
    public bool StartDelayed = true;

    /// <summary>
    /// Whether or not the entity should be stunned when reforming at all
    /// </summary>
    [DataField]
    public bool ShouldStun = true;

    /// <summary>
    /// The text that appears when attempting to reform
    /// </summary>
    [DataField(required: true)]
    public string PopupText;

    /// <summary>
    /// The mob that our entity will reform into
    /// </summary>
    [DataField(required: true)]
    public EntProtoId ReformPrototype { get; private set; }
}