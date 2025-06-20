// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordEclipse <65LordEclipse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Gateway.Systems;
using Content.Shared.Tag; // Goobstation
using Robust.Shared.Audio;
using Robust.Shared.Prototypes; // Goobstation
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Utility;

namespace Content.Server.Gateway.Components;

/// <summary>
/// Controlling gateway that links to other gateway destinations on the server.
/// </summary>
[RegisterComponent, Access(typeof(GatewaySystem)), AutoGenerateComponentPause]
public sealed partial class GatewayComponent : Component
{
    /// <summary>
    /// Whether this destination is shown in the gateway ui.
    /// If you are making a gateway for an admeme set this once you are ready for players to select it.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Enabled;

    /// <summary>
    /// Can the gateway be interacted with? If false then only settable via admins / mappers.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Interactable = true;

    /// <summary>
    /// Name as it shows up on the ui of station gateways.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public FormattedMessage Name = new();

    /// <summary>
    /// Sound to play when opening the portal.
    /// </summary>
    /// <remarks>
    /// Originally named PortalSound as it was used for opening and closing.
    /// </remarks>
    [DataField("portalSound")]
    public SoundSpecifier OpenSound = new SoundPathSpecifier("/Audio/Effects/Lightning/lightningbolt.ogg");

    /// <summary>
    /// Sound to play when closing the portal.
    /// </summary>
    [DataField]
    public SoundSpecifier CloseSound = new SoundPathSpecifier("/Audio/Effects/Lightning/lightningbolt.ogg");

    /// <summary>
    /// Sound to play when trying to open or close the portal and missing access.
    /// </summary>
    [DataField]
    public SoundSpecifier AccessDeniedSound = new SoundPathSpecifier("/Audio/Machines/custom_deny.ogg");

    /// <summary>
    /// Cooldown between opening portal / closing.
    /// </summary>
    [DataField]
    public TimeSpan Cooldown = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The time at which the portal can next be opened.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan NextReady;

    // Goobstation
    /// <summary>
    /// Restrict this gate's destinations and sources to gates tagged with this.
    /// </summary>
    [DataField]
    public ProtoId<TagPrototype>? TagRestriction;
}