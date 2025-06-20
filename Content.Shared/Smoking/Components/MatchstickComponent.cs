// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

// Shitmed Change Start

using Content.Shared.Smoking.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Smoking.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedMatchstickSystem))]
[AutoGenerateComponentState]
public sealed partial class MatchstickComponent : Component
{
    /// <summary>
    /// Current state to matchstick. Can be <code>Unlit</code>, <code>Lit</code> or <code>Burnt</code>.
    /// </summary>
    [DataField("state"), AutoNetworkedField]
    public SmokableState CurrentState = SmokableState.Unlit;

    /// <summary>
    /// How long will matchstick last in seconds.
    /// </summary>
    [DataField]
    public int Duration = 65;

    /// <summary>
    /// Sound played when you ignite the matchstick.
    /// </summary>
    [DataField(required: true)]
    public SoundSpecifier IgniteSound = default!;
}

// Shitmed Change End