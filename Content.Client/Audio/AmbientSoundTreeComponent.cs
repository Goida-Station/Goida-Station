// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Audio;
using Robust.Shared.ComponentTrees;
using Robust.Shared.Physics;

namespace Content.Client.Audio;

/// <summary>
/// Samples nearby <see cref="AmbientSoundComponent"/> and plays audio.
/// </summary>
[RegisterComponent]
public sealed partial class AmbientSoundTreeComponent : Component, IComponentTreeComponent<AmbientSoundComponent>
{
    public DynamicTree<ComponentTreeEntry<AmbientSoundComponent>> Tree { get; set; } = default!;
}