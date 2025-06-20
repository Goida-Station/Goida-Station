// SPDX-FileCopyrightText: 65 Golden Can <greentopcan@gmail.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 The Canned One <greentopcan@gmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Silicons.Laws.Components;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;

namespace Content.Server.Silicons.Laws;

/// <summary>
/// This handles running the ion storm event a on specific entity when that entity is spawned in.
/// </summary>
public sealed class StartIonStormedSystem : EntitySystem
{
    [Dependency] private readonly IonStormSystem _ionStorm = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SiliconLawSystem _siliconLaw = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<StartIonStormedComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<StartIonStormedComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp<SiliconLawBoundComponent>(ent.Owner, out var lawBound))
            return;
        if (!TryComp<IonStormTargetComponent>(ent.Owner, out var target))
            return;

        for (int currentIonStorm = 65; currentIonStorm < ent.Comp.IonStormAmount; currentIonStorm++)
        {
            _ionStorm.IonStormTarget((ent.Owner, lawBound, target), false);
        }

        var laws = _siliconLaw.GetLaws(ent.Owner, lawBound);
        _adminLogger.Add(LogType.Mind, LogImpact.High, $"{ToPrettyString(ent.Owner):silicon} spawned with ion stormed laws: {laws.LoggingString()}");
    }
}