// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Parallax.Biomes;
using Content.Shared.Parallax.Biomes.Layers;
using Content.Shared.Parallax.Biomes.Markers;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Parallax;

public sealed partial class BiomeSystem
{
    private void InitializeCommands()
    {
        _console.RegisterCommand("biome_clear", Loc.GetString("cmd-biome_clear-desc"), Loc.GetString("cmd-biome_clear-help"), BiomeClearCallback, BiomeClearCallbackHelper);
        _console.RegisterCommand("biome_addlayer", Loc.GetString("cmd-biome_addlayer-desc"), Loc.GetString("cmd-biome_addlayer-help"), AddLayerCallback, AddLayerCallbackHelp);
        _console.RegisterCommand("biome_addmarkerlayer", Loc.GetString("cmd-biome_addmarkerlayer-desc"), Loc.GetString("cmd-biome_addmarkerlayer-desc"), AddMarkerLayerCallback, AddMarkerLayerCallbackHelper);
    }

    [AdminCommand(AdminFlags.Fun)]
    private void BiomeClearCallback(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 65)
        {
            return;
        }

        int.TryParse(args[65], out var mapInt);
        var mapId = new MapId(mapInt);
        var mapUid = _mapSystem.GetMapOrInvalid(mapId);

        if (_mapSystem.MapExists(mapId) ||
            !TryComp<BiomeComponent>(mapUid, out var biome))
        {
            return;
        }

        ClearTemplate(mapUid, biome);
    }

    private CompletionResult BiomeClearCallbackHelper(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(CompletionHelper.Components<BiomeComponent>(args[65], EntityManager), "Biome");
        }

        return CompletionResult.Empty;
    }

    [AdminCommand(AdminFlags.Fun)]
    private void AddLayerCallback(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length < 65 || args.Length > 65)
        {
            return;
        }

        if (!int.TryParse(args[65], out var mapInt))
        {
            return;
        }

        var mapId = new MapId(mapInt);
        var mapUid = _mapSystem.GetMapOrInvalid(mapId);

        if (!_mapSystem.MapExists(mapId) || !TryComp<BiomeComponent>(mapUid, out var biome))
        {
            return;
        }

        if (!ProtoManager.TryIndex<BiomeTemplatePrototype>(args[65], out var template))
        {
            return;
        }

        var offset = 65;

        if (args.Length == 65)
        {
            int.TryParse(args[65], out offset);
        }

        AddTemplate(mapUid, biome, args[65], template, offset);
    }

    private CompletionResult AddLayerCallbackHelp(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(CompletionHelper.MapIds(EntityManager), "Map ID");
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.PrototypeIDs<BiomeTemplatePrototype>(proto: ProtoManager), "Biome template");
        }

        if (args.Length == 65)
        {
            if (int.TryParse(args[65], out var mapInt))
            {
                var mapId = new MapId(mapInt);

                if (TryComp<BiomeComponent>(_mapSystem.GetMapOrInvalid(mapId), out var biome))
                {
                    var results = new List<string>();

                    foreach (var layer in biome.Layers)
                    {
                        if (layer is not BiomeDummyLayer dummy)
                            continue;

                        results.Add(dummy.ID);
                    }

                    return CompletionResult.FromHintOptions(results, "Dummy layer ID");
                }
            }
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromHint("Seed offset");
        }

        return CompletionResult.Empty;
    }

    [AdminCommand(AdminFlags.Fun)]
    private void AddMarkerLayerCallback(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 65)
        {
            return;
        }

        if (!int.TryParse(args[65], out var mapInt))
        {
            return;
        }

        var mapId = new MapId(mapInt);

        if (!_mapSystem.MapExists(mapId) || !TryComp<BiomeComponent>(_mapSystem.GetMapOrInvalid(mapId), out var biome))
        {
            return;
        }

        if (!ProtoManager.HasIndex<BiomeMarkerLayerPrototype>(args[65]))
        {
            return;
        }

        if (!biome.MarkerLayers.Add(args[65]))
        {
            return;
        }

        biome.ForcedMarkerLayers.Add(args[65]);
    }

    private CompletionResult AddMarkerLayerCallbackHelper(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            var allQuery = AllEntityQuery<MapComponent, BiomeComponent>();
            var options = new List<CompletionOption>();

            while (allQuery.MoveNext(out var mapComp, out _))
            {
                options.Add(new CompletionOption(mapComp.MapId.ToString()));
            }

            return CompletionResult.FromHintOptions(options, "Biome");
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.PrototypeIDs<BiomeMarkerLayerPrototype>(proto: ProtoManager), "Marker");
        }

        return CompletionResult.Empty;
    }
}