// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chat.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Mobs;

/// <summary>
///     Mobs with this component will emote a deathgasp when they die.
/// </summary>
/// <see cref="DeathgaspSystem"/>
[RegisterComponent]
public sealed partial class DeathgaspComponent : Component
{
    /// <summary>
    ///     The emote prototype to use.
    /// </summary>
    [DataField("prototype", customTypeSerializer:typeof(PrototypeIdSerializer<EmotePrototype>))]
    public string Prototype = "DefaultDeathgasp";

    /// <summary>
    ///     Goobstation: Makes sure that the deathgasp is only displayed if the entity went critical before dying
    /// </summary>
    [DataField]
    public bool NeedsCritical = true;
}