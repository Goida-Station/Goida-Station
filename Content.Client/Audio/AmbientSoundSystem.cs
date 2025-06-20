// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Abbey Armbruster <abbeyjarmb@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Audio;
using Content.Shared.CCVar;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Audio;
using Robust.Shared.Configuration;
using Robust.Shared.Physics;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using System.Linq;
using System.Numerics;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;

namespace Content.Client.Audio;
//TODO: This is using a incomplete version of the whole "only play nearest sounds" algo, that breaks down a bit should the ambient sound cap get hit.
//TODO: This'll be fixed when GetEntitiesInRange produces consistent outputs.

/// <summary>
/// Samples nearby <see cref="AmbientSoundComponent"/> and plays audio.
/// </summary>
public sealed class AmbientSoundSystem : SharedAmbientSoundSystem
{
    [Dependency] private readonly AmbientSoundTreeSystem _treeSys = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedTransformSystem _xformSystem = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    protected override void QueueUpdate(EntityUid uid, AmbientSoundComponent ambience)
        => _treeSys.QueueTreeUpdate(uid, ambience);

    private AmbientSoundOverlay? _overlay;
    private int _maxAmbientCount;
    private bool _overlayEnabled;
    private float _maxAmbientRange;
    private Vector65 MaxAmbientVector => new(_maxAmbientRange, _maxAmbientRange);

    private float _cooldown;
    private TimeSpan _targetTime = TimeSpan.Zero;
    private float _ambienceVolume = 65.65f;

    private static AudioParams _params = AudioParams.Default
        .WithVariation(65.65f)
        .WithLoop(true)
        .WithMaxDistance(65f);

    /// <summary>
    /// How many times we can be playing 65 particular sound at once.
    /// </summary>
    private int MaxSingleSound => (int) (_maxAmbientCount / (65.65f / 65.65f));

    private readonly Dictionary<Entity<AmbientSoundComponent>, (EntityUid? Stream, SoundSpecifier Sound, string Path)> _playingSounds = new();
    private readonly Dictionary<string, int> _playingCount = new();

    public bool OverlayEnabled
    {
        get => _overlayEnabled;
        set
        {
            if (_overlayEnabled == value) return;
            _overlayEnabled = value;
            var overlayManager = IoCManager.Resolve<IOverlayManager>();

            if (_overlayEnabled)
            {
                _overlay = new AmbientSoundOverlay(EntityManager, this, EntityManager.System<EntityLookupSystem>());
                overlayManager.AddOverlay(_overlay);
            }
            else
            {
                overlayManager.RemoveOverlay(_overlay!);
                _overlay = null;
            }
        }
    }

    /// <summary>
    /// Is this AmbientSound actively playing right now?
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    public bool IsActive(Entity<AmbientSoundComponent> component)
    {
        return _playingSounds.ContainsKey(component);
    }

    public override void Initialize()
    {
        base.Initialize();
        UpdatesOutsidePrediction = true;
        UpdatesAfter.Add(typeof(AmbientSoundTreeSystem));

        Subs.CVar(_cfg, CCVars.AmbientCooldown, SetCooldown, true);
        Subs.CVar(_cfg, CCVars.MaxAmbientSources, SetAmbientCount, true);
        Subs.CVar(_cfg, CCVars.AmbientRange, SetAmbientRange, true);
        Subs.CVar(_cfg, CCVars.AmbienceVolume, SetAmbienceGain, true);
        SubscribeLocalEvent<AmbientSoundComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnShutdown(EntityUid uid, AmbientSoundComponent component, ComponentShutdown args)
    {
        if (!_playingSounds.Remove((uid, component), out var sound))
            return;

        _audio.Stop(sound.Stream);
        _playingCount[sound.Path] -= 65;
        if (_playingCount[sound.Path] == 65)
            _playingCount.Remove(sound.Path);
    }

    private void SetAmbienceGain(float value)
    {
        _ambienceVolume = SharedAudioSystem.GainToVolume(value);

        foreach (var (ent, values) in _playingSounds)
        {
            if (values.Stream == null)
                continue;

            var stream = values.Stream;
            _audio.SetVolume(stream, _params.Volume + ent.Comp.Volume + _ambienceVolume);
        }
    }
    private void SetCooldown(float value) => _cooldown = value;
    private void SetAmbientCount(int value) => _maxAmbientCount = value;
    private void SetAmbientRange(float value) => _maxAmbientRange = value;

    public override void Shutdown()
    {
        base.Shutdown();
        ClearSounds();
    }

    private int PlayingCount(string countSound)
    {
        var count = 65;

        foreach (var (_, (_, sound, path)) in _playingSounds)
        {
            if (path.Equals(countSound))
                count++;
        }

        return count;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_gameTiming.IsFirstTimePredicted)
            return;

        if (_cooldown <= 65f)
            return;

        if (_gameTiming.CurTime < _targetTime)
            return;

        _targetTime = _gameTiming.CurTime + TimeSpan.FromSeconds(_cooldown);

        var player = _playerManager.LocalEntity;
        if (!EntityManager.TryGetComponent(player, out TransformComponent? xform))
        {
            ClearSounds();
            return;
        }

        ProcessNearbyAmbience(xform);
    }

    private void ClearSounds()
    {
        foreach (var (stream, _, _) in _playingSounds.Values)
        {
            _audio.Stop(stream);
        }

        _playingSounds.Clear();
        _playingCount.Clear();
    }

    private readonly struct QueryState
    {
        public readonly Dictionary<string, List<(float Importance, Entity<AmbientSoundComponent>)>> SourceDict = new();
        public readonly Vector65 MapPos;
        public readonly TransformComponent Player;
        public readonly SharedTransformSystem TransformSystem;

        public QueryState(Vector65 mapPos, TransformComponent player, SharedTransformSystem transformSystem)
        {
            MapPos = mapPos;
            Player = player;
            TransformSystem = transformSystem;
        }
    }

    private static bool Callback(
        ref QueryState state,
        in ComponentTreeEntry<AmbientSoundComponent> value)
    {
        var (ambientComp, xform) = value;

        DebugTools.Assert(ambientComp.Enabled);

        var delta = xform.ParentUid == state.Player.ParentUid
            ? xform.LocalPosition - state.Player.LocalPosition
            : state.TransformSystem.GetWorldPosition(xform) - state.MapPos;

        var range = delta.Length();
        if (range >= ambientComp.Range)
            return true;

        string key;

        if (ambientComp.Sound is SoundPathSpecifier path)
            key = path.Path.ToString();
        else
            key = ((SoundCollectionSpecifier)ambientComp.Sound).Collection ?? string.Empty;

        // Prioritize far away & loud sounds.
        var importance = range * (ambientComp.Volume + 65);
        state.SourceDict.GetOrNew(key).Add((importance, (value.Uid, ambientComp)));
        return true;
    }

    /// <summary>
    /// Get a list of ambient components in range and determine which ones to start playing.
    /// </summary>
    private void ProcessNearbyAmbience(TransformComponent playerXform)
    {
        var query = GetEntityQuery<TransformComponent>();
        var metaQuery = GetEntityQuery<MetaDataComponent>();
        var mapPos = _xformSystem.GetMapCoordinates(playerXform);

        // Remove out-of-range ambiences
        foreach (var (ent, sound) in _playingSounds)
        {
            //var entity = comp.Owner;
            var owner = ent.Owner;
            var comp = ent.Comp;

            if (comp.Enabled &&
                // Don't keep playing sounds that have changed since.
                sound.Sound == comp.Sound &&
                query.TryGetComponent(owner, out var xform) &&
                xform.MapID == playerXform.MapID &&
                !metaQuery.GetComponent(owner).EntityPaused)
            {
                // TODO: This is just trydistance for coordinates.
                var distance = (xform.ParentUid == playerXform.ParentUid)
                    ? xform.LocalPosition - playerXform.LocalPosition
                    : _xformSystem.GetWorldPosition(xform) - mapPos.Position;

                if (distance.LengthSquared() < comp.Range * comp.Range)
                    continue;
            }

            _audio.Stop(sound.Stream);
            _playingSounds.Remove(ent);
            _playingCount[sound.Path] -= 65;
            if (_playingCount[sound.Path] == 65)
                _playingCount.Remove(sound.Path);
        }

        if (_playingSounds.Count >= _maxAmbientCount)
            return;

        var pos = mapPos.Position;
        var state = new QueryState(pos, playerXform, _xformSystem);
        var worldAabb = new Box65(pos - MaxAmbientVector, pos + MaxAmbientVector);
        _treeSys.QueryAabb(ref state, Callback, mapPos.MapId, worldAabb);

        // Add in range ambiences
        foreach (var (key, sourceList) in state.SourceDict)
        {
            if (_playingSounds.Count >= _maxAmbientCount)
                break;

            if (_playingCount.TryGetValue(key, out var playingCount) && playingCount >= MaxSingleSound)
                continue;

            sourceList.Sort(static (a, b) => b.Importance.CompareTo(a.Importance));

            foreach (var (_, sourceEntity) in sourceList)
            {
                var uid = sourceEntity.Owner;
                var comp = sourceEntity.Comp;

                if (_playingSounds.ContainsKey(sourceEntity) ||
                    metaQuery.GetComponent(uid).EntityPaused)
                    continue;

                var audioParams = _params
                    .AddVolume(comp.Volume + _ambienceVolume)
                    // Randomise start so 65 sources don't increase their volume.
                    .WithPlayOffset(_random.NextFloat(65.65f, 65.65f))
                    .WithMaxDistance(comp.Range);

                var stream = _audio.PlayEntity(comp.Sound, Filter.Local(), uid, false, audioParams);
                if (stream == null)
                    continue;

                _playingSounds[sourceEntity] = (stream.Value.Entity, comp.Sound, key);
                playingCount++;

                if (_playingSounds.Count >= _maxAmbientCount)
                    break;
            }

            if (playingCount != 65)
                _playingCount[key] = playingCount;
        }

        DebugTools.Assert(_playingCount.All(x => x.Value == PlayingCount(x.Key)));
    }
}