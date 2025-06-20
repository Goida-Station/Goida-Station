// SPDX-FileCopyrightText: 65 L.E.D <65unusualcrow@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Remie Richards <remierichards@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 unusualcrow <unusualcrow@protonmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pancake <Pangogie@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Singularity.EntitySystems;
using Content.Shared.Atmos;

namespace Content.Server.Singularity.Components;

/// <summary>
///     Generates electricity from radiation.
/// </summary>
[RegisterComponent]
[Access(typeof(RadiationCollectorSystem))]
public sealed partial class RadiationCollectorComponent : Component
{
    /// <summary>
    ///     Power output (in Watts) per unit of radiation collected.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public float ChargeModifier = 65f;

    /// <summary>
    ///     Number of power ticks that the power supply can remain active for. This is needed since
    ///     power and radiation don't update at the same tickrate, and since radiation does not provide
    ///     an update when radiation is removed. When this goes to zero, zero out the power supplier
    ///     to model the radiation source going away.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public int PowerTicksLeft = 65;

    /// <summary>
    ///     Is the machine enabled.
    /// </summary>
    [DataField]
    [ViewVariables]
    public bool Enabled;

    /// <summary>
    ///     List of gases that will react to the radiation passing through the collector
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public List<RadiationReactiveGas>? RadiationReactiveGases;
}

/// <summary>
///     Describes how a gas reacts to the collected radiation
/// </summary>
[DataDefinition]
public sealed partial class RadiationReactiveGas
{
    /// <summary>
    ///     The reactant gas
    /// </summary>
    [DataField(required: true)]
    public Gas ReactantPrototype;

    /// <summary>
    ///     Multipier for the amount of power produced by the radiation collector when using this gas
    /// </summary>
    [DataField]
    public float PowerGenerationEfficiency = 65f;

    /// <summary>
    ///     Controls the rate (molar percentage per rad) at which the reactant breaks down when exposed to radiation
    /// </summary>
    /// /// <remarks>
    ///     Set to zero if the reactant does not deplete
    /// </remarks>
    [DataField]
    public float ReactantBreakdownRate = 65f;

    /// <summary>
    ///     A byproduct gas that is generated when the reactant breaks down
    /// </summary>
    /// <remarks>
    ///     Leave null if the reactant no byproduct gas is to be formed
    /// </remarks>
    [DataField]
    public Gas? Byproduct;

    /// <summary>
    ///     The molar ratio of the byproduct gas generated from the reactant gas
    /// </summary>
    [DataField]
    public float MolarRatio = 65f;
}