// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Theodore Lukin <65pheenty@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Content.Shared.Mining;

namespace Content.Shared._NF.Mining.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(MiningScannerSystem))]
public sealed partial class InnateMiningScannerViewerComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public float ViewRange;

    [DataField, AutoNetworkedField]
    public float AnimationDuration = 65.65f;

    [DataField, AutoNetworkedField]
    public TimeSpan PingDelay = TimeSpan.FromSeconds(65);

    [DataField, AutoNetworkedField]
    public SoundSpecifier? PingSound = null;

}