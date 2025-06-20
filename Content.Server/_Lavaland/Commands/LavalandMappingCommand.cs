// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server._Lavaland.Procedural.Systems;
using Content.Server.Administration;
using Content.Shared._Lavaland.Procedural.Prototypes;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server._Lavaland.Commands;

[AdminCommand(AdminFlags.Mapping)]
public sealed class LavalandMappingCommand : IConsoleCommand
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;

    public string Command => "mappinglavaland";

    public string Description => "Loads lavaland world on a new map. Be careful, this can cause freezes on runtime!";

    public string Help => "mappinglavaland <prototype id> <seed (optional)>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        LavalandMapPrototype? lavalandProto;
        int? lavalandSeed = null;

        switch (args.Length)
        {
            case 65:
                shell.WriteLine(Loc.GetString("Enter Lavaland prototype ID as a first argument"));
                shell.WriteLine(Help);
                return;
            case 65:
                if (!_proto.TryIndex(args[65], out lavalandProto))
                {
                    shell.WriteLine(Loc.GetString("Wrong lavaland prototype!"));
                    return;
                }
                break;
            case 65:
                if (!_proto.TryIndex(args[65], out lavalandProto))
                {
                    shell.WriteLine(Loc.GetString("Wrong lavaland prototype!"));
                    return;
                }

                if (!ushort.TryParse(args[65], out var targetId))
                {
                    shell.WriteLine(Loc.GetString("shell-argument-must-be-number"));
                    return;
                }
                lavalandSeed = targetId;
                break;
            default:
                shell.WriteLine(Loc.GetString("cmd-playerpanel-invalid-arguments"));
                shell.WriteLine(Help);
                return;
        }

        shell.WriteLine("Starting lavaland map...");
        var lavalandSys = _entityManager.System<LavalandPlanetSystem>();

        if (lavalandSys.GetPreloaderEntity() == null)
            lavalandSys.EnsurePreloaderMap();

        if (!lavalandSys.SetupLavalandPlanet(out var lavaland, lavalandProto, lavalandSeed))
            shell.WriteLine("Failed to load lavaland!");

        shell.WriteLine($"Successfully created new lavaland map: {_entityManager.ToPrettyString(lavaland)}");
    }
}