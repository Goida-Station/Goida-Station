// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Administration;
using Content.Server.Polymorph.Systems;
using Content.Shared.Administration;
using Content.Shared.Polymorph;
using Robust.Shared.Prototypes;
using Robust.Shared.Toolshed;

namespace Content.Server.Polymorph.Toolshed;

/// <summary>
///     Polymorphs the given entity(s) into the target morph.
/// </summary>
[ToolshedCommand, AdminCommand(AdminFlags.Fun)]
public sealed class PolymorphCommand : ToolshedCommand
{
    private PolymorphSystem? _system;
    [Dependency] private IPrototypeManager _proto = default!;

    [CommandImplementation]
    public EntityUid? Polymorph(
            [PipedArgument] EntityUid input,
            ProtoId<PolymorphPrototype> protoId
        )
    {
        _system ??= GetSys<PolymorphSystem>();

        if (!_proto.TryIndex(protoId, out var prototype))
            return null;

        return _system.PolymorphEntity(input, prototype.Configuration);
    }

    [CommandImplementation]
    public IEnumerable<EntityUid> Polymorph(
            [PipedArgument] IEnumerable<EntityUid> input,
            ProtoId<PolymorphPrototype> protoId
        )
        => input.Select(x => Polymorph(x, protoId)).Where(x => x is not null).Select(x => (EntityUid)x!);
}