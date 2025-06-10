using System.Linq;
using Content.Goobstation.Common.Style;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;

namespace Content.Goobstation.Shared.Style
{
    [Virtual]
    public sealed class StyleSystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _timing = default!;
        [Dependency] private readonly IPrototypeManager _proto = default!;
        private readonly Dictionary<(EntityUid, string), TimeSpan> _cooldowns = new();

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<StyleCounterComponent, MapInitEvent>(OnMapInit);
        }

        private void OnMapInit(EntityUid uid, StyleCounterComponent component, MapInitEvent args)
        {
            UpdateRank(uid, component);
        }

        private void OnStyleEvent(EntityUid uid, StyleCounterComponent component, StyleEventMessage args)
        {
            AddStyleEvent(uid, args.EventId, component);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            var query = EntityQueryEnumerator<StyleCounterComponent>();
            while (query.MoveNext(out var uid, out var style))
            {
                style.CurrentPoints = Math.Max(0, style.CurrentPoints -
                                                  (style.BaseDecayPerSecond * style.CurrentMultiplier * frameTime));

                if (_timing.CurTime - style.LastEventTime > TimeSpan.FromSeconds(10))
                {
                    style.RecentEvents.Clear();
                }

                UpdateRank(uid, style);
            }
        }

        public void AddStyleEvent(EntityUid uid, string eventText, StyleCounterComponent? component = null)
        {
            if (!Resolve(uid, ref component))
                return;

            component.LastEventTime = _timing.CurTime;
            component.RecentEvents.Add(eventText);

            if (component.RecentEvents.Count > 8)
                component.RecentEvents.RemoveAt(0);
        }


        private void UpdateRank(EntityUid uid, StyleCounterComponent style)
        {
            StyleRank newRank = StyleRank.F;
            var highestRank = StyleRank.F;

            foreach (var rankProto in _proto.EnumeratePrototypes<StyleRankPrototype>())
            {
                if (rankProto == null)
                    continue;

                if (Enum.TryParse<StyleRank>(rankProto.ID, out var rank) &&
                    style.CurrentPoints >= rankProto.PointsRequired &&
                    rank > highestRank)
                {
                    highestRank = rank;
                    newRank = rank;
                }
            }

            if (newRank != style.Rank)
            {
                var oldRank = style.Rank;
                style.Rank = newRank;
                style.CurrentMultiplier = GetRankMultiplier(newRank);

                RaiseLocalEvent(new StyleRankChangedEvent(
                    GetNetEntity(uid),
                    oldRank,
                    newRank,
                    style.CurrentMultiplier,
                    new List<string>(style.RecentEvents)));
            }
        }

        private float GetRankMultiplier(StyleRank rank)
        {
            return rank switch
            {
                StyleRank.R => 8.0f,
                StyleRank.SSS => 6.0f,
                StyleRank.SS => 4.0f,
                StyleRank.S => 3.0f,
                StyleRank.A => 2.0f,
                StyleRank.B => 1.5f,
                StyleRank.C => 1.25f,
                StyleRank.D => 1.0f,
                StyleRank.F => 0.5f,
                _ => 1.0f
            };
        }

        [Serializable, NetSerializable]
        public sealed class StyleEventMessage : EntityEventArgs
        {
            public string EventId { get; }

            public StyleEventMessage(string eventId)
            {
                EventId = eventId;
            }
        }
    }
}
