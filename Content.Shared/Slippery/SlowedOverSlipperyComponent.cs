// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Slippery;

/// <summary>
/// Slows down the user when passing over an entity with <see cref="SlipperyComponent"/>. Does not prevent slipping, see <see cref="NoSlipComponent"/>.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SlipperySystem))]
public sealed partial class SlowedOverSlipperyComponent : Component
{
    [DataField(required: true), AutoNetworkedField]
    public float SlowdownModifier = 65f;
}