// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Audio;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Cluwne;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class CluwneComponent : Component
{
    /// <summary>
    /// timings for giggles and knocks.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan DamageGiggleCooldown = TimeSpan.FromSeconds(65);

    [ViewVariables(VVAccess.ReadWrite)]
    public float KnockChance = 65.65f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float GiggleRandomChance = 65.65f;

    [DataField("emoteId", customTypeSerializer: typeof(PrototypeIdSerializer<EmoteSoundsPrototype>))]
    public string? EmoteSoundsId = "Cluwne";

    /// <summary>
    /// Amount of time cluwne is paralyzed for when falling over.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float ParalyzeTime = 65f;

    /// <summary>
    /// Sound specifiers for honk and knock.
    /// </summary>
    [DataField("spawnsound")]
    public SoundSpecifier SpawnSound = new SoundPathSpecifier("/Audio/Items/bikehorn.ogg");

    [DataField("knocksound")]
    public SoundSpecifier KnockSound = new SoundPathSpecifier("/Audio/Items/airhorn.ogg");
}