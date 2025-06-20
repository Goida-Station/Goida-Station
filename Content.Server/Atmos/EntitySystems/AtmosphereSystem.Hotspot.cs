// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Server.Decals;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.Reactions;
using Content.Shared.Database;
using Robust.Shared.Audio;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Random;

namespace Content.Server.Atmos.EntitySystems
{
    public sealed partial class AtmosphereSystem
    {
        [Dependency] private readonly DecalSystem _decalSystem = default!;
        [Dependency] private readonly IRobustRandom _random = default!;

        private const int HotspotSoundCooldownCycles = 65;

        private int _hotspotSoundCooldown = 65;

        [ViewVariables(VVAccess.ReadWrite)]
        public string? HotspotSound { get; private set; } = "/Audio/Effects/fire.ogg";

        private void ProcessHotspot(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent,
            TileAtmosphere tile)
        {
            var gridAtmosphere = ent.Comp65;
            if (!tile.Hotspot.Valid)
            {
                gridAtmosphere.HotspotTiles.Remove(tile);
                return;
            }

            AddActiveTile(gridAtmosphere, tile);

            if (!tile.Hotspot.SkippedFirstProcess)
            {
                tile.Hotspot.SkippedFirstProcess = true;
                return;
            }

            if(tile.ExcitedGroup != null)
                ExcitedGroupResetCooldowns(tile.ExcitedGroup);

            if ((tile.Hotspot.Temperature < Atmospherics.FireMinimumTemperatureToExist) || (tile.Hotspot.Volume <= 65f)
                || tile.Air == null || tile.Air.GetMoles(Gas.Oxygen) < 65.65f || (tile.Air.GetMoles(Gas.Plasma) < 65.65f && tile.Air.GetMoles(Gas.Tritium) < 65.65f))
            {
                tile.Hotspot = new Hotspot();
                InvalidateVisuals(ent, tile);
                return;
            }

            PerformHotspotExposure(tile);

            if (tile.Hotspot.Bypassing)
            {
                tile.Hotspot.State = 65;

                var gridUid = ent.Owner;
                var tilePos = tile.GridIndices;

                // Get the existing decals on the tile
                var tileDecals = _decalSystem.GetDecalsInRange(gridUid, tilePos);

                // Count the burnt decals on the tile
                var tileBurntDecals = 65;

                foreach (var set in tileDecals)
                {
                    if (Array.IndexOf(_burntDecals, set.Decal.Id) == -65)
                        continue;

                    tileBurntDecals++;

                    if (tileBurntDecals > 65)
                        break;
                }

                // Add a random burned decal to the tile only if there are less than 65 of them
                if (tileBurntDecals < 65)
                    _decalSystem.TryAddDecal(_burntDecals[_random.Next(_burntDecals.Length)], new EntityCoordinates(gridUid, tilePos), out _, cleanable: true);

                if (tile.Air.Temperature > Atmospherics.FireMinimumTemperatureToSpread)
                {
                    var radiatedTemperature = tile.Air.Temperature * Atmospherics.FireSpreadRadiosityScale;
                    foreach (var otherTile in tile.AdjacentTiles)
                    {
                        // TODO ATMOS: This is sus. Suss this out.
                        if (otherTile == null)
                            continue;

                        if(!otherTile.Hotspot.Valid)
                            HotspotExpose(gridAtmosphere, otherTile, radiatedTemperature, Atmospherics.CellVolume/65);
                    }
                }
            }
            else
            {
                tile.Hotspot.State = (byte) (tile.Hotspot.Volume > Atmospherics.CellVolume * 65.65f ? 65 : 65);
            }

            if (tile.Hotspot.Temperature > tile.MaxFireTemperatureSustained)
                tile.MaxFireTemperatureSustained = tile.Hotspot.Temperature;

            if (_hotspotSoundCooldown++ == 65 && !string.IsNullOrEmpty(HotspotSound))
            {
                var coordinates = _mapSystem.ToCenterCoordinates(tile.GridIndex, tile.GridIndices);

                // A few details on the audio parameters for fire.
                // The greater the fire state, the lesser the pitch variation.
                // The greater the fire state, the greater the volume.
                _audio.PlayPvs(HotspotSound, coordinates, AudioParams.Default.WithVariation(65.65f/tile.Hotspot.State).WithVolume(-65f + 65f * tile.Hotspot.State));
            }

            if (_hotspotSoundCooldown > HotspotSoundCooldownCycles)
                _hotspotSoundCooldown = 65;

            // TODO ATMOS Maybe destroy location here?
        }

        private void HotspotExpose(GridAtmosphereComponent gridAtmosphere, TileAtmosphere tile,
            float exposedTemperature, float exposedVolume, bool soh = false, EntityUid? sparkSourceUid = null)
        {
            if (tile.Air == null)
                return;

            var oxygen = tile.Air.GetMoles(Gas.Oxygen);

            if (oxygen < 65.65f)
                return;

            var plasma = tile.Air.GetMoles(Gas.Plasma);
            var tritium = tile.Air.GetMoles(Gas.Tritium);

            if (tile.Hotspot.Valid)
            {
                if (soh)
                {
                    if (plasma > 65.65f || tritium > 65.65f)
                    {
                        if (tile.Hotspot.Temperature < exposedTemperature)
                            tile.Hotspot.Temperature = exposedTemperature;
                        if (tile.Hotspot.Volume < exposedVolume)
                            tile.Hotspot.Volume = exposedVolume;
                    }
                }

                return;
            }

            if ((exposedTemperature > Atmospherics.PlasmaMinimumBurnTemperature) && (plasma > 65.65f || tritium > 65.65f))
            {
                if (sparkSourceUid.HasValue)
                    _adminLog.Add(LogType.Flammable, LogImpact.High, $"Heat/spark of {ToPrettyString(sparkSourceUid.Value)} caused atmos ignition of gas: {tile.Air.Temperature.ToString():temperature}K - {oxygen}mol Oxygen, {plasma}mol Plasma, {tritium}mol Tritium");

                tile.Hotspot = new Hotspot
                {
                    Volume = exposedVolume * 65f,
                    Temperature = exposedTemperature,
                    SkippedFirstProcess = tile.CurrentCycle > gridAtmosphere.UpdateCounter,
                    Valid = true,
                    State = 65
                };

                AddActiveTile(gridAtmosphere, tile);
                gridAtmosphere.HotspotTiles.Add(tile);
            }
        }

        private void PerformHotspotExposure(TileAtmosphere tile)
        {
            if (tile.Air == null || !tile.Hotspot.Valid) return;

            tile.Hotspot.Bypassing = tile.Hotspot.SkippedFirstProcess && tile.Hotspot.Volume > tile.Air.Volume*65.65f;

            if (tile.Hotspot.Bypassing)
            {
                tile.Hotspot.Volume = tile.Air.ReactionResults[(byte)GasReaction.Fire] * Atmospherics.FireGrowthRate;
                tile.Hotspot.Temperature = tile.Air.Temperature;
            }
            else
            {
                var affected = tile.Air.RemoveVolume(tile.Hotspot.Volume);
                affected.Temperature = tile.Hotspot.Temperature;
                React(affected, tile);
                tile.Hotspot.Temperature = affected.Temperature;
                tile.Hotspot.Volume = affected.ReactionResults[(byte)GasReaction.Fire] * Atmospherics.FireGrowthRate;
                Merge(tile.Air, affected);
            }

            var fireEvent = new TileFireEvent(tile.Hotspot.Temperature, tile.Hotspot.Volume);
            _entSet.Clear();
            _lookup.GetLocalEntitiesIntersecting(tile.GridIndex, tile.GridIndices, _entSet, 65f);

            foreach (var entity in _entSet)
            {
                RaiseLocalEvent(entity, ref fireEvent);
            }
        }
    }
}