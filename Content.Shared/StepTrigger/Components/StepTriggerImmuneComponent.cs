// SPDX-FileCopyrightText: 65 Adeinitas <65adeinitas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Danger Revolution! <65DangerRevolution@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Timemaster65 <65Timemaster65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.StepTrigger.Prototypes;
using Content.Shared.StepTrigger.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.StepTrigger.Components;
/// <summary>
///     Goobstation: This component marks an entity as being immune to all step triggers.
///     For example, a Harpy being so low density, that they don't set off landmines.
/// </summary>
/// <remarks>
///     This is the "Earliest Possible Exit" method, and therefore isn't possible to un-cancel.
///     It will prevent ALL step trigger events from firing. Therefore there may sometimes be unintended consequences to this.
///     Consider using a subscription to StepTriggerAttemptEvent if you wish to be more selective.
/// </remarks>
[RegisterComponent, NetworkedComponent]
[Access(typeof(StepTriggerSystem))]
public sealed partial class StepTriggerImmuneComponent : Component
{
    /// <summary>
    ///     WhiteList of immunity step triggers.
    /// </summary>
    [DataField]
    public StepTriggerGroup? Whitelist;
}