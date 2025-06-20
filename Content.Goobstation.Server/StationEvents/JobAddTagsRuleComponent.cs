// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Roles;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Server.StationEvents;

[RegisterComponent, Access(typeof(JobAddTagsRule))]
public sealed partial class JobAddTagsRuleComponent : Component
{
    [DataField(required: true)]
    public List<ProtoId<JobPrototype>> Affected = default!;

    [DataField(required: true)]
    public List<ProtoId<TagPrototype>> Tags = default!;

    /// <summary>
    /// Message to send in the affected person's chat window.
    /// </summary>
    [DataField]
    public LocId? Message;
}