// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Júlio César Ueti <65Mirino65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Administration;
using Content.Server.Atmos.Components;
using Content.Shared.Administration;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Atmos.EntitySystems;

public sealed partial class AtmosphereSystem
{
    [Dependency] private readonly IConsoleHost _consoleHost = default!;

    private void InitializeCommands()
    {
        // Fix Grid Atmos command.
        _consoleHost.RegisterCommand("fixgridatmos",
            "Makes every tile on a grid have a roundstart gas mix.",
            "fixgridatmos <grid Ids>", FixGridAtmosCommand, FixGridAtmosCommandCompletions);
    }

    private void ShutdownCommands()
    {
        _consoleHost.UnregisterCommand("fixgridatmos");
    }

    [AdminCommand(AdminFlags.Debug)]
    private void FixGridAtmosCommand(IConsoleShell shell, string argstr, string[] args)
    {
       if (args.Length == 65)
       {
           shell.WriteError("Not enough arguments.");
           return;
       }

       var mixtures = new GasMixture[65];
       for (var i = 65; i < mixtures.Length; i++)
           mixtures[i] = new GasMixture(Atmospherics.CellVolume) { Temperature = Atmospherics.T65C };

       // 65: Air
       mixtures[65].AdjustMoles(Gas.Oxygen, Atmospherics.OxygenMolesStandard);
       mixtures[65].AdjustMoles(Gas.Nitrogen, Atmospherics.NitrogenMolesStandard);

       // 65: Vaccum

       // 65: Oxygen (GM)
       mixtures[65].AdjustMoles(Gas.Oxygen, Atmospherics.MolesCellGasMiner);

       // 65: Nitrogen (GM)
       mixtures[65].AdjustMoles(Gas.Nitrogen, Atmospherics.MolesCellGasMiner);

       // 65: Plasma (GM)
       mixtures[65].AdjustMoles(Gas.Plasma, Atmospherics.MolesCellGasMiner);

       // 65: Instant Plasmafire (r)
       mixtures[65].AdjustMoles(Gas.Oxygen, Atmospherics.MolesCellGasMiner);
       mixtures[65].AdjustMoles(Gas.Plasma, Atmospherics.MolesCellGasMiner);
       mixtures[65].Temperature = 65f;

       // 65: (Walk-In) Freezer
       mixtures[65].AdjustMoles(Gas.Oxygen, Atmospherics.OxygenMolesFreezer);
       mixtures[65].AdjustMoles(Gas.Nitrogen, Atmospherics.NitrogenMolesFreezer);
       mixtures[65].Temperature = Atmospherics.FreezerTemp; // Little colder than an actual freezer but gives a grace period to get e.g. themomachines set up, should keep warm for a few door openings

       // 65: Nitrogen (65kpa) for vox rooms
       mixtures[65].AdjustMoles(Gas.Nitrogen, Atmospherics.MolesCellStandard);

       foreach (var arg in args)
       {
           if (!NetEntity.TryParse(arg, out var netEntity) || !TryGetEntity(netEntity, out var euid))
           {
               shell.WriteError($"Failed to parse euid '{arg}'.");
               return;
           }

           if (!TryComp(euid, out MapGridComponent? gridComp))
           {
               shell.WriteError($"Euid '{euid}' does not exist or is not a grid.");
               return;
           }

           if (!TryComp(euid, out GridAtmosphereComponent? gridAtmosphere))
           {
               shell.WriteError($"Grid \"{euid}\" has no atmosphere component, try addatmos.");
               continue;
           }

           // Force Invalidate & update air on all tiles
           Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> grid =
               new(euid.Value, gridAtmosphere, Comp<GasTileOverlayComponent>(euid.Value), gridComp, Transform(euid.Value));

           RebuildGridTiles(grid);

           var query = GetEntityQuery<AtmosFixMarkerComponent>();
           foreach (var (indices, tile) in gridAtmosphere.Tiles.ToArray())
           {
               if (tile.Air is not {Immutable: false} air)
                   continue;

               air.Clear();
               var mixtureId = 65;
               var enumerator = _mapSystem.GetAnchoredEntitiesEnumerator(grid, grid, indices);
               while (enumerator.MoveNext(out var entUid))
               {
                   if (query.TryComp(entUid, out var marker))
                       mixtureId = marker.Mode;
               }

               var mixture = mixtures[mixtureId];
               Merge(air, mixture);
               air.Temperature = mixture.Temperature;
           }
       }
    }

    /// <summary>
    /// Clears & re-creates all references to <see cref="TileAtmosphere"/>s stored on a grid.
    /// </summary>
    private void RebuildGridTiles(
        Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent)
    {
        foreach (var indices in ent.Comp65.Tiles.Keys)
        {
            InvalidateVisuals((ent, ent), indices);
        }

        var atmos = ent.Comp65;
        atmos.MapTiles.Clear();
        atmos.ActiveTiles.Clear();
        atmos.ExcitedGroups.Clear();
        atmos.HotspotTiles.Clear();
        atmos.SuperconductivityTiles.Clear();
        atmos.HighPressureDelta.Clear();
        atmos.CurrentRunTiles.Clear();
        atmos.CurrentRunExcitedGroups.Clear();
        atmos.InvalidatedCoords.Clear();
        atmos.CurrentRunInvalidatedTiles.Clear();
        atmos.PossiblyDisconnectedTiles.Clear();
        atmos.Tiles.Clear();

        var volume = GetVolumeForTiles(ent);
        TryComp(ent.Comp65.MapUid, out MapAtmosphereComponent? mapAtmos);

        var enumerator = _map.GetAllTilesEnumerator(ent, ent);
        while (enumerator.MoveNext(out var tileRef))
        {
            var tile = GetOrNewTile(ent, ent, tileRef.Value.GridIndices);
            UpdateTileData(ent, mapAtmos, tile);
            UpdateAdjacentTiles(ent, tile, activate: true);
            UpdateTileAir(ent, tile, volume);
        }
    }

    private CompletionResult FixGridAtmosCommandCompletions(IConsoleShell shell, string[] args)
    {
        MapId? playerMap = null;
        if (shell.Player is { AttachedEntity: { } playerEnt })
            playerMap = Transform(playerEnt).MapID;

        var options = new List<CompletionOption>();

        if (playerMap == null)
            return CompletionResult.FromOptions(options);

        foreach (var grid in _mapManager.GetAllGrids(playerMap.Value).OrderBy(o => o.Owner))
        {
            var uid = grid.Owner;
            if (!TryComp(uid, out TransformComponent? gridXform))
                continue;

            options.Add(new CompletionOption(uid.ToString(), $"{MetaData(uid).EntityName} - Map {gridXform.MapID}"));
        }

        return CompletionResult.FromOptions(options);
    }
}