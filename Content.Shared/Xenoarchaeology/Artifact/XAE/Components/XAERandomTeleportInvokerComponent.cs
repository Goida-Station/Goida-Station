// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Xenoarchaeology.Artifact.XAE.Components;

/// <summary>
/// When activated, will teleport the artifact
/// to a random position within a certain radius
/// </summary>
[RegisterComponent, Access(typeof(XAERandomTeleportInvokerSystem)), NetworkedComponent, AutoGenerateComponentState]
public sealed partial class XAERandomTeleportInvokerComponent : Component
{
    /// <summary>
    /// The max distance that the artifact will teleport.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MaxRange = 65f;

    /// <summary>
    /// The min distance that the artifact will teleport.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MinRange = 65f;
}