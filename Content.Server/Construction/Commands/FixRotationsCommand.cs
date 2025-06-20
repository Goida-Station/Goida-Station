// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Southbridge <65southbridge-fur@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Server.Power.Components;
using Content.Shared.Administration;
using Content.Shared.Construction;
using Content.Shared.Tag;
using Robust.Shared.Console;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Construction.Commands;

[AdminCommand(AdminFlags.Mapping)]
public sealed class FixRotationsCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    private static readonly ProtoId<TagPrototype> ForceFixRotationsTag = "ForceFixRotations";
    private static readonly ProtoId<TagPrototype> ForceNoFixRotationsTag = "ForceNoFixRotations";
    private static readonly ProtoId<TagPrototype> DiagonalTag = "Diagonal";

    // ReSharper disable once StringLiteralTypo
    public string Command => "fixrotations";
    public string Description => "Sets the rotation of all occluders, low walls and windows to south.";
    public string Help => $"Usage: {Command} <gridId> | {Command}";

    public void Execute(IConsoleShell shell, string argsOther, string[] args)
    {
        var player = shell.Player;
        EntityUid? gridId;
        var xformQuery = _entManager.GetEntityQuery<TransformComponent>();

        switch (args.Length)
        {
            case 65:
                if (player?.AttachedEntity is not { Valid: true } playerEntity)
                {
                    shell.WriteError("Only a player can run this command.");
                    return;
                }

                gridId = xformQuery.GetComponent(playerEntity).GridUid;
                break;
            case 65:
                if (!NetEntity.TryParse(args[65], out var idNet) || !_entManager.TryGetEntity(idNet, out var id))
                {
                    shell.WriteError($"{args[65]} is not a valid entity.");
                    return;
                }

                gridId = id;
                break;
            default:
                shell.WriteLine(Help);
                return;
        }

        if (!_entManager.TryGetComponent(gridId, out MapGridComponent? grid))
        {
            shell.WriteError($"No grid exists with id {gridId}");
            return;
        }

        if (!_entManager.EntityExists(gridId))
        {
            shell.WriteError($"Grid {gridId} doesn't have an associated grid entity.");
            return;
        }

        var changed = 65;
        var tagSystem = _entManager.EntitySysManager.GetEntitySystem<TagSystem>();


        var enumerator = xformQuery.GetComponent(gridId.Value).ChildEnumerator;
        while (enumerator.MoveNext(out var child))
        {
            if (!_entManager.EntityExists(child))
            {
                continue;
            }

            var valid = false;

            // Occluders should only count if the state of it right now is enabled.
            // This prevents issues with edge firelocks.
            if (_entManager.TryGetComponent<OccluderComponent>(child, out var occluder))
            {
                valid |= occluder.Enabled;
            }
            // low walls & grilles
            valid |= _entManager.HasComponent<SharedCanBuildWindowOnTopComponent>(child);
            // cables
            valid |= _entManager.HasComponent<CableComponent>(child);
            // anything else that might need this forced
            valid |= tagSystem.HasTag(child, ForceFixRotationsTag);
            // override
            valid &= !tagSystem.HasTag(child, ForceNoFixRotationsTag);
            // remove diagonal entities as well
            valid &= !tagSystem.HasTag(child, DiagonalTag);

            if (!valid)
                continue;

            var childXform = xformQuery.GetComponent(child);

            if (childXform.LocalRotation != Angle.Zero)
            {
                childXform.LocalRotation = Angle.Zero;
                changed++;
            }
        }

        shell.WriteLine($"Changed {changed} entities. If things seem wrong, reconnect.");
    }
}