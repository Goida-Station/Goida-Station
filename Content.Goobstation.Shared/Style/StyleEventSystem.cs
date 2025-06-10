using Content.Goobstation.Common.Style;
using Content.Shared._Shitmed.Weapons.Ranged.Events;
using Content.Shared.Slippery;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Timing;

// i hate hardcode, but there is no other way of making this work otherwise.
namespace Content.Goobstation.Shared.Style
{
    public sealed class StyleEventSystem : EntitySystem
    {
        [Dependency] private readonly StyleSystem _styleSystem = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;

        private readonly Dictionary<EntityUid, TimeSpan> _lastEventTimes = new();

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<MeleeHitEvent>(OnMeleeHit);
            SubscribeLocalEvent<StyleCounterComponent, GunShotBodyEvent>(OnGunShot);
            SubscribeLocalEvent<StyleCounterComponent, SlipAttemptEvent>(OnSlipAttempt);
        }

        private bool CanRegisterEvent(EntityUid uid)
        {
            if (!_lastEventTimes.TryGetValue(uid, out var lastTime))
                return true;

            var currentTime = _gameTiming.CurTime;
            return (currentTime - lastTime).TotalSeconds >= 0.1; // cooldown between detecting events because predictions.
        }

        private void OnMeleeHit(MeleeHitEvent args)
        {
            if (_gameTiming.ApplyingState) return;

            if (!TryComp<StyleCounterComponent>(args.User, out var styleComp))
                return;

            if (!CanRegisterEvent(args.User))
                return;

            _lastEventTimes[args.User] = _gameTiming.CurTime;
            _styleSystem.AddStyleEvent(args.User, "+HIT", styleComp);
            styleComp.CurrentPoints += 50;
        }

        private void OnGunShot(EntityUid uid, StyleCounterComponent styleComp, GunShotBodyEvent args)
        {
            if (!CanRegisterEvent(uid))
                return;

            _lastEventTimes[uid] = _gameTiming.CurTime;
            _styleSystem.AddStyleEvent(uid, "+BULLET HIT", styleComp);
            styleComp.CurrentPoints += 100;
        }

        private void OnSlipAttempt(EntityUid uid, StyleCounterComponent styleComp, SlipAttemptEvent args)
        {
            if (!CanRegisterEvent(uid))
                return;

            _lastEventTimes[uid] = _gameTiming.CurTime;
            styleComp.CurrentPoints = Math.Max(0, styleComp.CurrentPoints - 50);
            _styleSystem.AddStyleEvent(uid, "-SLIP", styleComp);
        }
    }
}
