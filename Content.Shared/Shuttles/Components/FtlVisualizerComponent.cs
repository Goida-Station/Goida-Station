// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.Shuttles.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FtlVisualizerComponent : Component
{
    /// <summary>
    /// Clientside time tracker for the animation.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float Elapsed;

    [DataField(required: true)]
    public SpriteSpecifier.Rsi Sprite;

    /// <summary>
    /// Target grid to pull FTL visualization from.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid Grid;
}