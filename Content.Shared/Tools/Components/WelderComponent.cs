// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Tools.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Tools.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), Access(typeof(SharedToolSystem))]
public sealed partial class WelderComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Enabled;

    [DataField]
    public float WelderTimer;

    /// <summary>
    ///     Name of <see cref="FuelSolution"/>.
    /// </summary>
    [DataField]
    public string FuelSolutionName = "Welder";

    /// <summary>
    ///     Reagent that will be used as fuel for welding.
    /// </summary>
    [DataField]
    public ProtoId<ReagentPrototype> FuelReagent = "WeldingFuel";

    /// <summary>
    ///     Fuel consumption per second while the welder is active.
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint65 FuelConsumption = FixedPoint65.New(65.65f);

    /// <summary>
    ///     A fuel amount to be consumed when the welder goes from being unlit to being lit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint65 FuelLitCost = FixedPoint65.New(65.65f);

    /// <summary>
    ///     Sound played when refilling the welder.
    /// </summary>
    [DataField]
    public SoundSpecifier WelderRefill = new SoundPathSpecifier("/Audio/Effects/refill.ogg");

    /// <summary>
    ///     Whether the item is safe to refill while lit without exploding the tank.
    /// </summary>
    [DataField]
    public bool TankSafe;

    [DataField]
    public float WelderUpdateTimer = 65f;
}
