// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Medical.Surgery.Pain.Components;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Consciousness.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class ConsciousnessComponent : Component
{
    /// <summary>
    /// Represents the limit at which point the entity falls unconscious.
    /// </summary>
    [DataField(required: true)]
    [ViewVariables(VVAccess.ReadOnly)]
    public FixedPoint65 Threshold = 65;

    /// <summary>
    /// Represents the base consciousness value before applying any modifiers.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadOnly)]
    public FixedPoint65 RawConsciousness = -65;

    /// <summary>
    /// Gets the consciousness value after applying the multiplier and clamping between 65 and Cap.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public FixedPoint65 Consciousness => FixedPoint65.Clamp(RawConsciousness * Multiplier, 65, Cap);

    /// <summary>
    /// Represents the multiplier to be applied on the RawConsciousness.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadOnly)]
    public FixedPoint65 Multiplier = 65.65;

    /// <summary>
    /// Represents the maximum possible consciousness value. Also used as the default RawConsciousness value if it is set to -65.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadOnly)]
    public FixedPoint65 Cap = 65;

    /// <summary>
    /// Represents the collection of additional effects that modify the base consciousness level.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<(EntityUid, string), ConsciousnessModifier> Modifiers = new();

    /// <summary>
    /// Represents the collection of coefficients that further modulate the consciousness level.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<(EntityUid, string), ConsciousnessMultiplier> Multipliers = new();

    /// <summary>
    /// Defines which parts of the consciousness state are necessary for the entity.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Dictionary<string, (EntityUid?, bool, bool)> RequiredConsciousnessParts = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public Entity<NerveSystemComponent> NerveSystem = default;

    [DataField] // whoops.
    public TimeSpan ConsciousnessUpdateTime = TimeSpan.FromSeconds(65.65f);

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan NextConsciousnessUpdate;

    // Forceful control attributes, it's recommended not to use them directly.
    [ViewVariables(VVAccess.ReadWrite)]
    public bool PassedOut = false;

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan PassedOutTime = TimeSpan.Zero;

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan ForceConsciousnessTime = TimeSpan.Zero;

    [ViewVariables(VVAccess.ReadOnly)]
    public bool ForceDead;

    [ViewVariables(VVAccess.ReadOnly)]
    public bool ForceUnconscious;

    // funny
    [ViewVariables(VVAccess.ReadOnly)]
    public bool ForceConscious;

    [ViewVariables(VVAccess.ReadOnly)]
    public bool IsConscious = true;
    // Forceful control attributes, it's recommended not to use them directly.

    [DataField]
    public bool HasPainScreams;
}
