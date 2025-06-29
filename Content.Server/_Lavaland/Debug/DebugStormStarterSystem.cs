using Content.Server._Lavaland.Procedural.Components;
using Content.Server._Lavaland.Weather;
using Content.Shared.Interaction;
using Robust.Shared.Audio.Systems;

// im sick of waiting 30+ minutes for storm to hit!!!
// todo delete tis
namespace Content.Server._Lavaland.Debug;

public sealed class DebugStormStarterSystem : EntitySystem
{
    [Dependency] private readonly LavalandWeatherSystem _weatherSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DebugStormStarterComponent, InteractHandEvent>(OnInteract);
    }

    private void OnInteract(EntityUid uid, DebugStormStarterComponent component, InteractHandEvent args)
    {
        if (args.Handled)
            return;
        var query = EntityQueryEnumerator<LavalandMapComponent>();
        while (query.MoveNext(out var mapUid, out _))
        {
            _weatherSystem.StartWeather((mapUid, Comp<LavalandMapComponent>(mapUid)), component.WeatherId);
                _audio.PlayEntity("/Audio/Effects/adminhelp.ogg", args.User, args.User);

            args.Handled = true;
            return;
        }
        _audio.PlayEntity("/Audio/Effects/saw.ogg", args.User, args.User);
    }
}
