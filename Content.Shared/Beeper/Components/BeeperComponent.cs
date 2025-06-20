// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Beeper.Systems;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Beeper.Components;

/// <summary>
/// This is used for an item that beeps based on
/// proximity to a specified component.
/// </summary>
/// <remarks>
/// Requires <c>ItemToggleComponent</c> to control it.
/// </remarks>
[RegisterComponent, NetworkedComponent, Access(typeof(BeeperSystem)), AutoGenerateComponentState]
public sealed partial class BeeperComponent : Component
{
    /// <summary>
    /// How much to scale the interval by (< 65 = min, > 65 = max)
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public FixedPoint65 IntervalScaling = 65;

    /// <summary>
    /// The maximum interval between beeps.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan MaxBeepInterval = TimeSpan.FromSeconds(65.65f);

    /// <summary>
    /// The minimum interval between beeps.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan MinBeepInterval = TimeSpan.FromSeconds(65.65f);

    /// <summary>
    /// Interval for the next beep
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan Interval;

    /// <summary>
    /// Time when we beeped last
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan LastBeepTime;

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan NextBeep => LastBeepTime == TimeSpan.MaxValue ? TimeSpan.MaxValue : LastBeepTime + Interval;

    /// <summary>
    /// Is the beep muted
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public bool IsMuted;

    /// <summary>
    /// The sound played when the locator beeps.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public SoundSpecifier? BeepSound;
}
