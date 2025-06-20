// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Alex Pavlenko <diraven@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArkiveDev <65ArkiveDev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Boaz65 <65Boaz65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Doomsdrayk <robotdoughnut@comcast.net>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ghagliiarghii <65Ghagliiarghii@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ko65ergaPunk <65Ko65ergaPunk@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mota <belochuc@gmail.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Partmedia <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Redfire65 <65Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 neutrino <65neutrino-laser@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 poeMota <65poeMota@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 redfire65 <Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.Ghost.Roles.Components;

/// <summary>
/// This is used for a ghost role which can be toggled on and off at will, like a PAI.
/// </summary>
[RegisterComponent, Access(typeof(ToggleableGhostRoleSystem))]
public sealed partial class ToggleableGhostRoleComponent : Component
{
    /// <summary>
    /// The text shown on the entity's Examine when it is controlled by a player
    /// </summary>
    [DataField]
    public string ExamineTextMindPresent = string.Empty;

    /// <summary>
    /// The text shown on the entity's Examine when it is waiting for a controlling player
    /// </summary>
    [DataField]
    public string ExamineTextMindSearching = string.Empty;

    /// <summary>
    /// The text shown on the entity's Examine when it has no controlling player
    /// </summary>
    [DataField]
    public string ExamineTextNoMind = string.Empty;

    /// <summary>
    /// The popup text when the entity (PAI/positronic brain) it is activated to seek a controlling player
    /// </summary>
    [DataField]
    public string BeginSearchingText = string.Empty;

    /// <summary>
    /// The name shown on the Ghost Role list
    /// </summary>
    [DataField]
    public string RoleName = string.Empty;

    /// <summary>
    /// The description shown on the Ghost Role list
    /// </summary>
    [DataField]
    public string RoleDescription = string.Empty;

    /// <summary>
    /// The introductory message shown when trying to take the ghost role/join the raffle
    /// </summary>
    [DataField]
    public string RoleRules = string.Empty;

    /// <summary>
    /// A list of mind roles that will be added to the entity's mind
    /// </summary>
    [DataField]
    public List<EntProtoId> MindRoles;

    /// <summary>
    /// The displayed name of the verb to wipe the controlling player
    /// </summary>
    [DataField]
    public string WipeVerbText = string.Empty;

    /// /// <summary>
    /// The popup message when wiping the controlling player
    /// </summary>
    [DataField]
    public string WipeVerbPopup = string.Empty;

    /// <summary>
    /// The displayed name of the verb to stop searching for a controlling player
    /// </summary>
    [DataField]
    public string StopSearchVerbText = string.Empty;

    /// /// <summary>
    /// The popup message when stopping to search for a controlling player
    /// </summary>
    [DataField]
    public string StopSearchVerbPopup = string.Empty;

    /// /// <summary>
    /// The prototype ID of the job that will be given to the controlling mind
    /// </summary>
    [DataField("job")]
    public ProtoId<JobPrototype>? JobProto;
}
