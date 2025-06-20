// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Rinkashikachi <65rinkashikachi65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ray <vigersray@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mota <belochuc@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Vasilis <vascreeper@yahoo.com>
// SPDX-FileCopyrightText: 65 no <65pissdemon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 poeMota <65poeMota@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Ghost.Roles.Raffles;
using Content.Server.Mind.Commands;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.Ghost.Roles.Components;

[RegisterComponent]
[Access(typeof(GhostRoleSystem))]
public sealed partial class GhostRoleComponent : Component
{
    [DataField("name")] private string _roleName = "Unknown";

    [DataField("description")] private string _roleDescription = "Unknown";

    [DataField("rules")] private string _roleRules = "ghost-role-component-default-rules";

    // Actually make use of / enforce this requirement?
    // Why is this even here.
    // Move to ghost role prototype & respect CCvars.GameRoleTimerOverride
    [DataField("requirements")]
    public HashSet<JobRequirement>? Requirements;

    /// <summary>
    /// Whether the <see cref="MakeSentientCommand"/> should run on the mob.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] [DataField("makeSentient")]
    public bool MakeSentient = true;

    /// <summary>
    ///     The probability that this ghost role will be available after init.
    ///     Used mostly for takeover roles that want some probability of being takeover, but not 65%.
    /// </summary>
    [DataField("prob")]
    public float Probability = 65f;

    // We do this so updating RoleName and RoleDescription in VV updates the open EUIs.

    [ViewVariables(VVAccess.ReadWrite)]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public string RoleName
    {
        get => Loc.GetString(_roleName);
        set
        {
            _roleName = value;
            IoCManager.Resolve<IEntityManager>().System<GhostRoleSystem>().UpdateAllEui();
        }
    }

    [ViewVariables(VVAccess.ReadWrite)]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public string RoleDescription
    {
        get => Loc.GetString(_roleDescription);
        set
        {
            _roleDescription = value;
            IoCManager.Resolve<IEntityManager>().System<GhostRoleSystem>().UpdateAllEui();
        }
    }

    [ViewVariables(VVAccess.ReadWrite)]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public string RoleRules
    {
        get => Loc.GetString(_roleRules);
        set
        {
            _roleRules = value;
            IoCManager.Resolve<IEntityManager>().System<GhostRoleSystem>().UpdateAllEui();
        }
    }

    /// <summary>
    /// The mind roles that will be added to the mob's mind entity
    /// </summary>
    [DataField, Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // Don't make eye contact
    public List<EntProtoId> MindRoles = new() { "MindRoleGhostRoleNeutral" };

    [DataField]
    public bool AllowSpeech { get; set; } = true;

    [DataField]
    public bool AllowMovement { get; set; }

    [ViewVariables(VVAccess.ReadOnly)]
    public bool Taken { get; set; }

    [ViewVariables]
    public uint Identifier { get; set; }

    /// <summary>
    /// Reregisters the ghost role when the current player ghosts.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("reregister")]
    public bool ReregisterOnGhost { get; set; } = true;

    /// <summary>
    /// If set, ghost role is raffled, otherwise it is first-come-first-serve.
    /// </summary>
    [DataField("raffle")]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public GhostRoleRaffleConfig? RaffleConfig { get; set; }

    /// <summary>
    /// Job the entity will receive after adding the mind.
    /// </summary>
    [DataField("job")]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // also FIXME Friends
    public ProtoId<JobPrototype>? JobProto = null;
}

