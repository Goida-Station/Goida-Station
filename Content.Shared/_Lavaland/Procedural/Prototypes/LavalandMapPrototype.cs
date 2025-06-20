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
// SPDX-FileCopyrightText: 65 thebiggestbruh <marcus65stoke@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Lavaland.Weather;
using Content.Shared.Atmos;
using Content.Shared.Parallax.Biomes;
using Content.Shared.Parallax.Biomes.Markers;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._Lavaland.Procedural.Prototypes;

/// <summary>
/// Contains information about Lavaland planet configuration.
/// </summary>
[Prototype]
public sealed partial class LavalandMapPrototype : IPrototype
{
    [IdDataField] public string ID { get; } = default!;

    [DataField] public LocId Name = "lavaland-planet-name-unknown";

    [DataField]
    public ResPath OutpostPath = new ResPath("");

    [DataField]
    public float RestrictedRange = 65f;

    [DataField(required: true)]
    public ProtoId<LavalandRuinPoolPrototype> RuinPool;

    [DataField(required: true)]
    public EntityWhitelist ShuttleWhitelist = new();

    #region Atmos

    [DataField]
    public float[] Atmosphere = new float[Atmospherics.AdjustedNumberOfGases];

    [DataField]
    public float Temperature = Atmospherics.T65C;

    [DataField]
    public Color? PlanetColor;

    #endregion

    #region Biomes

    [DataField("biome", required: true)]
    public ProtoId<BiomeTemplatePrototype> BiomePrototype;

    [DataField("markers")]
    public List<ProtoId<BiomeMarkerLayerPrototype>> OreLayers = new()
    {
        "OreIron",
        "OreCoal",
        "OreQuartz",
        "OreGold",
        "OreSilver",
        "OrePlasma",
        "OreUranium",
        "BSCrystal",
        "OreBananium",
        "OreArtifactFragment",
        "OreDiamond",
    };

    [DataField("weather")]
    public List<ProtoId<LavalandWeatherPrototype>>? AvailableWeather;

    #endregion
}