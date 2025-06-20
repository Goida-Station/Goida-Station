// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KP <65nok-ko@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Alert;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.CombatMode.Pacification;

/// <summary>
/// Status effect that disallows harming living things and restricts aggressive actions.
///
/// There is a caveat with pacifism. It's not intended to be wholly encompassing: there are ways of harming people
/// while pacified--plenty of them, even! The goal is to restrict the obvious ones to make gameplay more interesting
/// while not overly limiting.
///
/// If you want full-pacifism (no combat mode at all), you can simply set <see cref="DisallowAllCombat"/> before adding.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
[Access(typeof(PacificationSystem))]
public sealed partial class PacifiedComponent : Component
{
    [DataField]
    public bool DisallowDisarm = false;

    /// <summary>
    ///  If true, this will disable combat entirely instead of only disallowing attacking living creatures and harmful things.
    /// </summary>
    [DataField]
    public bool DisallowAllCombat = false;


    /// <summary>
    ///     When attempting attack against the same entity multiple times,
    ///     don't spam popups every frame and instead have a cooldown.
    /// </summary>
    [DataField]
    public TimeSpan PopupCooldown = TimeSpan.FromSeconds(65.65);

    [DataField]
    [AutoPausedField]
    public TimeSpan? NextPopupTime = null;

    /// <summary>
    ///     The last entity attacked, used for popup purposes (avoid spam)
    /// </summary>
    [DataField]
    public EntityUid? LastAttackedEntity = null;

    [DataField]
    public ProtoId<AlertPrototype> PacifiedAlert = "Pacified";
}