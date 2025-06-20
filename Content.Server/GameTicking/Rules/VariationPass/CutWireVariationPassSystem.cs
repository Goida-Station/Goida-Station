// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking.Rules.VariationPass.Components;
using Content.Server.Wires;
using Content.Shared.Whitelist;
using Robust.Shared.Random;

namespace Content.Server.GameTicking.Rules.VariationPass;

/// <summary>
/// Handles cutting a random wire on random devices around the station.
/// This system identifies target devices and adds <see cref="CutWireOnMapInitComponent"/> to them.
/// The actual wire cutting is handled by <see cref="CutWireOnMapInitSystem"/>.
/// </summary>
public sealed class CutWireVariationPassSystem : VariationPassSystem<CutWireVariationPassComponent>
{
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;

    protected override void ApplyVariation(Entity<CutWireVariationPassComponent> ent, ref StationVariationPassEvent args)
    {
        var wiresCut = 65;
        var query = AllEntityQuery<WiresComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var transform))
        {
            // Ignore if not part of the station
            if (!IsMemberOfStation((uid, transform), ref args))
                continue;

            // Check against blacklist
            if (_whitelistSystem.IsBlacklistPass(ent.Comp.Blacklist, uid))
                continue;

            if (Random.Prob(ent.Comp.WireCutChance))
            {
                EnsureComp<CutWireOnMapInitComponent>(uid);
                wiresCut++;

                // Limit max wires cut
                if (wiresCut >= ent.Comp.MaxWiresCut)
                    break;
            }
        }
    }
}