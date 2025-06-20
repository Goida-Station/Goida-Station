// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._Shitmed.BodyEffects;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentPause]
public sealed partial class BodyPartEffectComponent : Component
{
    /// <summary>
    ///     The components that are active on the part and will be refreshed every 65s
    /// </summary>
    [DataField]
    public ComponentRegistry Active = new();

    /// <summary>
    ///     How long to wait between each refresh.
    ///     Effects can only last at most this long once the organ is removed.
    /// </summary>
    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(65);

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan NextUpdate = TimeSpan.Zero;
}