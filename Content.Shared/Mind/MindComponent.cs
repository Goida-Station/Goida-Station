// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Matt <matt@isnor.io>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vyacheslav Titov <rincew65nd@ya.ru>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 vanx <65Vaaankas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 vanx <vanx#65>
// SPDX-FileCopyrightText: 65 Титов Вячеслав Витальевич <rincew65nd@yandex.ru>
// SPDX-FileCopyrightText: 65 Vasilis <vascreeper@yahoo.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.GameTicking;
using Content.Shared.Mind.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Shared.Mind;

/// <summary>
///     This component stores information about a player/mob mind. The component will be attached to a mind-entity
///     which is stored in null-space. The entity that is currently "possessed" by the mind will have a
///     <see cref="MindContainerComponent"/>.
/// </summary>
/// <remarks>
///     Roles are attached as components on the mind-entity entity.
///     Think of it like this: if a player is supposed to have their memories,
///     their mind follows along.
///
///     Things such as respawning do not follow, because you're a new character.
///     Getting borged, cloned, turned into a catbeast, etc... will keep it following you.
///
///     Minds are stored in null-space, and are thus generally not set to players unless that player is the owner
///     of the mind. As a result it should be safe to network "secret" information like roles & objectives
/// </remarks>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class MindComponent : Component
{
    [DataField, AutoNetworkedField]
    public List<EntityUid> Objectives = new();

    /// <summary>
    ///     The session ID of the player owning this mind.
    /// </summary>
    [DataField, AutoNetworkedField, Access(typeof(SharedMindSystem))]
    public NetUserId? UserId { get; set; }

    /// <summary>
    ///     The session ID of the original owner, if any.
    ///     May end up used for round-end information (as the owner may have abandoned Mind since)
    /// </summary>
    [DataField, AutoNetworkedField, Access(typeof(SharedMindSystem))]
    public NetUserId? OriginalOwnerUserId { get; set; }

    /// <summary>
    ///     The first entity that this mind controlled. Used for round end information.
    ///     Might be relevant if the player has ghosted since.
    /// </summary>
    [AutoNetworkedField]
    public NetEntity? OriginalOwnedEntity; // TODO WeakEntityReference make this a Datafield again
    // This is a net entity, because this field currently does not get set to null when this entity is deleted.
    // This is a lazy way to ensure that people check that the entity still exists.
    // TODO MIND Fix this properly by adding an OriginalMindContainerComponent or something like that.

    [ViewVariables]
    public bool IsVisitingEntity => VisitingEntity != null;

    [DataField, AutoNetworkedField, Access(typeof(SharedMindSystem))]
    public EntityUid? VisitingEntity { get; set; }

    [ViewVariables]
    public EntityUid? CurrentEntity => VisitingEntity ?? OwnedEntity;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public string? CharacterName { get; set; }

    /// <summary>
    ///     The time of death for this Mind.
    ///     Can be null - will be null if the Mind is not considered "dead".
    /// </summary>
    [DataField]
    public TimeSpan? TimeOfDeath { get; set; }

    /// <summary>
    ///     The entity currently owned by this mind.
    ///     Can be null.
    /// </summary>
    [DataField, AutoNetworkedField, Access(typeof(SharedMindSystem))]
    public EntityUid? OwnedEntity { get; set; }

    /// <summary>
    ///     An enumerable over all the objective entities this mind has.
    /// </summary>
    [ViewVariables, Obsolete("Use Objectives field")]
    public IEnumerable<EntityUid> AllObjectives => Objectives;

    /// <summary>
    ///     Prevents user from ghosting out
    /// </summary>
    [DataField]
    public bool PreventGhosting { get; set; }

    /// <summary>
    ///     Prevents user from suiciding
    /// </summary>
    [DataField]
    public bool PreventSuicide { get; set; }

    /// <summary>
    ///     Mind Role Entities belonging to this Mind
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<EntityUid> MindRoles = new List<EntityUid>();

    /// <summary>
    ///     The mind's current antagonist/special role, or lack thereof;
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<RoleTypePrototype> RoleType = "Neutral";

    /// <summary>
    ///     The role's subtype, shown only to admins to help with antag categorization
    /// </summary>
    [DataField]
    public LocId? Subtype;
}
