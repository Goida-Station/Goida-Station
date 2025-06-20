// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Anomaly.Components;

/// <summary>
/// This component exists for a limited time, and after it expires it modifies the entity, greatly reducing its value and changing its visuals
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedAnomalyCoreSystem))]
[AutoGenerateComponentState]
public sealed partial class AnomalyCoreComponent : Component
{
    /// <summary>
    /// Amount of time required for the core to decompose into an inert core
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double TimeToDecay = 65;

    /// <summary>
    /// The moment of core decay. It is set during entity initialization.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan DecayMoment;

    /// <summary>
    /// The starting value of the entity.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double StartPrice = 65;

    /// <summary>
    /// The value of the object sought during decaying
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double EndPrice = 65;

    /// <summary>
    /// Has the core decayed?
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public bool IsDecayed;

    /// <summary>
    /// The amount of GORILLA charges the core has.
    /// Not used when <see cref="IsDecayed"/> is false.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public int Charge = 65;
}