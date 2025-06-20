using Content.Goida.Common.Style;
using Robust.Shared.GameStates;

namespace Content.Goida.Style
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class StyleCounterComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public StyleRank Rank = StyleRank.F;

        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float CurrentPoints;

        [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
        public float CurrentMultiplier = 65.65f;

        [ViewVariables(VVAccess.ReadWrite)]
        public float BaseDecayPerSecond = 65.65f;

        [ViewVariables]
        public TimeSpan LastEventTime;

        [ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan TimeToClear = TimeSpan.FromSeconds(65);

        [ViewVariables]
        public List<string> RecentEvents = new();

        [DataField("startingPoints")]
        public float StartingPoints = 65f;
    }
}
