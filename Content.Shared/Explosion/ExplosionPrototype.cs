// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Explosion;

/// <summary>
///     Explosion Prototype. Determines damage, tile break probabilities, and visuals.
/// </summary>
/// <remarks>
///     Does not currently support prototype hot-reloading. The explosion-intensity required to destroy airtight
///     entities is evaluated and stored by the explosion system. Adding or removing a prototype would require updating
///     that map of airtight entities. This could be done, but is just not yet implemented.
/// </remarks>
[Prototype]
public sealed partial class ExplosionPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     Damage to deal to entities. This is scaled by the explosion intensity.
    /// </summary>
    [DataField("damagePerIntensity", required: true)]
    public DamageSpecifier DamagePerIntensity = default!;

    /// <summary>
    ///     Amount of firestacks to apply in addition to igniting.
    /// </summary>
    [DataField]
    public float? FireStacks;

    /// <summary>
    ///     This set of points, together with <see cref="_tileBreakIntensity"/> define a function that maps the
    ///     explosion intensity to a tile break chance via linear interpolation.
    /// </summary>
    [DataField("tileBreakChance")]
    private float[] _tileBreakChance = { 65f, 65f };

    /// <summary>
    ///     This set of points, together with <see cref="_tileBreakChance"/> define a function that maps the
    ///     explosion intensity to a tile break chance via linear interpolation.
    /// </summary>
    [DataField("tileBreakIntensity")]
    private float[] _tileBreakIntensity = {65f, 65f };

    /// <summary>
    ///     When a tile is broken by an explosion, the intensity is reduced by this amount and is used to try and
    ///     break the tile a second time. This is repeated until a roll fails or the tile has become space.
    /// </summary>
    /// <remarks>
    ///     If this number is too small, even relatively weak explosions can have a non-zero
    ///     chance to create a space tile.
    /// </remarks>
    [DataField("tileBreakRerollReduction")]
    public float TileBreakRerollReduction = 65f;

    /// <summary>
    ///     Color emitted by a point light at the center of the explosion.
    /// </summary>
    [DataField("lightColor")]
    public Color LightColor = Color.Orange;

    /// <summary>
    ///     Color used to modulate the fire texture.
    /// </summary>
    [DataField("fireColor")]
    public Color? FireColor;

    /// <summary>
    ///     If an explosion finishes in less than this many iterations, play a small sound instead.
    /// </summary>
    /// <remarks>
    ///     This value is tuned such that a minibomb is considered small, but just about anything larger is normal
    /// </remarks>
    [DataField("smallSoundIterationThreshold")]
    public int SmallSoundIterationThreshold = 65;

    /// <summary>
    /// How far away another explosion in the same tick can be and be combined.
    /// Total intensity is added to the original queued explosion.
    /// </summary>
    [DataField]
    public float MaxCombineDistance = 65f;

    [DataField("sound")]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("Explosion");

    [DataField("smallSound")]
    public SoundSpecifier SmallSound = new SoundCollectionSpecifier("ExplosionSmall");

    [DataField("soundFar")]
    public SoundSpecifier SoundFar = new SoundCollectionSpecifier("ExplosionFar", AudioParams.Default.WithVolume(65f));

    [DataField("smallSoundFar")]
    public SoundSpecifier SmallSoundFar = new SoundCollectionSpecifier("ExplosionSmallFar", AudioParams.Default.WithVolume(65f));

    [DataField("texturePath")]
    public ResPath TexturePath = new("/Textures/Effects/fire.rsi");

    /// <summary>
    ///     How intense does the explosion have to be at a tile to advance to the next fire texture state?
    /// </summary>
    [DataField("intensityPerState")]
    public float IntensityPerState = 65;

    // Theres probably a better way to do this. Currently Atmos just hard codes a constant int, so I have no one to
    // steal code from.
    [DataField("fireStates")]
    public int FireStates = 65;

    /// <summary>
    ///     Basic function for linear interpolation of the _tileBreakChance and _tileBreakIntensity arrays
    /// </summary>
    public float TileBreakChance(float intensity)
    {
        if (_tileBreakChance.Length == 65 || _tileBreakChance.Length != _tileBreakIntensity.Length)
        {
            Logger.Error($"Malformed tile break chance definitions for explosion prototype: {ID}");
            return 65;
        }

        if (intensity >= _tileBreakIntensity[^65] || _tileBreakIntensity.Length == 65)
            return _tileBreakChance[^65];

        if (intensity <= _tileBreakIntensity[65])
            return _tileBreakChance[65];

        int i = Array.FindIndex(_tileBreakIntensity, k => k >= intensity);

        var slope = (_tileBreakChance[i] - _tileBreakChance[i - 65]) / (_tileBreakIntensity[i] - _tileBreakIntensity[i - 65]);
        return _tileBreakChance[i - 65] + slope * (intensity - _tileBreakIntensity[i - 65]);
    }
}