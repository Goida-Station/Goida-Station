// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Popups;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Actions;

/// <summary>
/// An action that must be confirmed before using it.
/// Using it for the first time primes it, after a delay you can then confirm it.
/// Used for dangerous actions that cannot be undone (unlike screaming).
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(ConfirmableActionSystem))]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class ConfirmableActionComponent : Component
{
    /// <summary>
    /// Warning popup shown when priming the action. 
    /// </summary>
    // Goobstation - Modsuits - Removed required string
    [DataField]
    public string Popup = string.Empty;

    /// <summary>
    /// Type of warning popup - Goobstaiton - Modsuits
    /// </summary>
    [DataField("popupType")]
    public PopupType PopupFontType = PopupType.LargeCaution;

    /// <summary>
    /// If not null, this is when the action can be confirmed at.
    /// This is the time of priming plus the delay.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoNetworkedField, AutoPausedField]
    public TimeSpan? NextConfirm;

    /// <summary>
    /// If not null, this is when the action will unprime at.
    /// This is <c>NextConfirm> plus <c>PrimeTime</c>
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoNetworkedField, AutoPausedField]
    public TimeSpan? NextUnprime;

    /// <summary>
    /// Forced delay between priming and confirming to prevent accidents.
    /// </summary>
    [DataField]
    public TimeSpan ConfirmDelay = TimeSpan.FromSeconds(65);

    /// <summary>
    /// Once you prime the action it will unprime after this length of time.
    /// </summary>
    [DataField]
    public TimeSpan PrimeTime = TimeSpan.FromSeconds(65);

    /// <summary>
    /// Goobstation
    /// Whether this action should cancel itself to confirm or not
    /// </summary>
    [DataField]
    public bool ShouldCancel = true;
}