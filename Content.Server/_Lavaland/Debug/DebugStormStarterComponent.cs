using Content.Shared._Lavaland.Weather;
using Robust.Shared.Prototypes;

namespace Content.Server._Lavaland.Debug;

[RegisterComponent]
public sealed partial class DebugStormStarterComponent : Component
{
    [DataField]
    public ProtoId<LavalandWeatherPrototype> WeatherId = "AshfallLigh";
}
