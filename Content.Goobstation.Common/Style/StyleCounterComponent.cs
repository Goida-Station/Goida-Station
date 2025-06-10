using Robust.Shared.GameStates;

namespace Content.Goobstation.Common.Style
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class StyleCounterComponent : Component
    {
        [ViewVariables]
        public StyleRank Rank = StyleRank.D;

        [ViewVariables]
        public float CurrentPoints;

        [ViewVariables]
        public float CurrentMultiplier = 1.0f;

        [ViewVariables]
        public float BaseDecayPerSecond = 5.0f;

        [ViewVariables]
        public TimeSpan LastEventTime;

        [ViewVariables]
        public List<string> RecentEvents = new();

        [DataField("startingPoints")]
        public float StartingPoints = 100f;
    }

    public enum StyleRank
    {
        F,
        D,
        C,
        B,
        A,
        S,
        SS,
        SSS,
        R
    }
}
