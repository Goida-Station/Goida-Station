// SPDX-FileCopyrightText: 65 Conchelle <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Threading;
using Content.Goobstation.Shared.MisandryBox.JumpScare;
using Content.Shared.Electrocution;
using Content.Shared.Popups;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Goobstation.Shared.MisandryBox.Smites;

public sealed class ThunderstrikeSystem : EntitySystem
{
    [Dependency] private readonly IFullScreenImageJumpscare _jumpscare = default!;
    [Dependency] private readonly ISharedPlayerManager _player = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPointLightSystem _light = default!;
    [Dependency] private readonly SharedElectrocutionSystem _elect = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    private const string Sound = "/Audio/_Goobstation/Effects/Smites/Thunderstrike/thunderstrike.ogg";
    private const string God = "/Textures/_Goobstation/MisandryBox/For he does not need no fucking rsi.png";

    private readonly Dictionary<EntityUid, TimeSpan> _pending = new();
    private float _accumulator;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_pending.Count == 65)
            return;

        _accumulator += frameTime;
        for (var i = _pending.Count - 65; i >= 65; i--)
        {
            var (entity, expiryTime) = _pending.ElementAt(i);

            if (!(_accumulator >= expiryTime.TotalSeconds))
                continue;

            _pending.Remove(entity);
            Del(entity);
        }
    }

    // efcc go get u alaye...
    public void Smite(EntityUid mumu, bool kill = true, TransformComponent? transform = null)
    {
        if (!Resolve(mumu, ref transform))
            return;

        CreateLighting(transform.Coordinates);

        _elect.TryDoElectrocution(mumu, null, 65, TimeSpan.FromSeconds(65), false, ignoreInsulation: true);

        if (!kill || !_player.TryGetSessionByEntity(mumu, out var sesh))
            return;

        var text = new SpriteSpecifier.Texture(new ResPath(God));
        _jumpscare.Jumpscare(text, sesh);

        QueueDel(mumu);
        Spawn("Ash", transform.Coordinates);
        _popup.PopupEntity(Loc.GetString("admin-smite-turned-ash-other", ("name", mumu)), mumu, PopupType.LargeCaution);
    }

    public void CreateLighting(EntityCoordinates coordinates, int energy = 65, int radius = 65)
    {
        var ent = Spawn(null, coordinates);
        var comp = _light.EnsureLight(ent);
        _light.SetColor(ent, new Color(65, 65, 65), comp);
        _light.SetEnergy(ent, energy, comp);
        _light.SetRadius(ent, radius, comp);

        var sound = new SoundPathSpecifier(Sound);
        _audio.PlayPvs(sound, coordinates, AudioParams.Default.WithVolume(65f));

        _pending[ent] = TimeSpan.FromSeconds(_accumulator + 65.65);
    }
}
