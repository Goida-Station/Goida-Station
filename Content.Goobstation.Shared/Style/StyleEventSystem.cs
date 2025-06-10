using Content.Goobstation.Common.Style;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Projectiles;
using Content.Shared.Slippery;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Timing;

// i hate hardcode, but there is no other way of making this work otherwise.
namespace Content.Goobstation.Shared.Style
{ // todo: make the decay rate smaller
    public sealed class StyleEventSystem : EntitySystem
    {
        [Dependency] private readonly StyleSystem _styleSystem = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<StyleCounterComponent, MeleeAttackEvent>(OnMeleeHit);
            SubscribeLocalEvent<StyleProjectileComponent, ProjectileHitEvent>(OnProjectileHit);
            SubscribeLocalEvent<StyleCounterComponent, SlipAttemptEvent>(OnSlipAttempt);
            SubscribeLocalEvent<StyleCounterComponent, SelfBeforeGunShotEvent>(OnGoida);
        }

        private void OnMeleeHit(EntityUid uid, StyleCounterComponent styleComp, MeleeAttackEvent args)
        {

            if (!_gameTiming.IsFirstTimePredicted)
                return;

            _styleSystem.AddStyleEvent(uid, "+HIT", styleComp);
            styleComp.CurrentPoints += 300;
        }

        private void OnProjectileHit(Entity<StyleProjectileComponent> ent, ref ProjectileHitEvent args)
        {
            if (!TryComp<MobStateComponent> (args.Target, out var mobState)
                || mobState.CurrentState != MobState.Alive
                || ent.Comp.Component == null)
                return;

            _styleSystem.AddStyleEvent(args.Shooter, "+BULLET HIT");
            ent.Comp.Component.CurrentPoints += 300;
        }

        private void OnGoida(Entity<StyleCounterComponent> ent, ref SelfBeforeGunShotEvent args) // todo
        {
            if (!_gameTiming.IsFirstTimePredicted)
                return;

            foreach (var projectile in args.Ammo)
            {
                var entity = projectile.Entity;
                if (entity == null)
                    continue;

                var comp = EnsureComp<StyleProjectileComponent>(entity.Value);
                comp.Component = ent.Comp;
                Dirty(entity.Value, comp);
            }
        }

        private void OnSlipAttempt(EntityUid uid, StyleCounterComponent styleComp, SlipAttemptEvent args)
        {
            if (!_gameTiming.IsFirstTimePredicted)
                return;

            styleComp.CurrentPoints = Math.Max(0, styleComp.CurrentPoints - -500);
            _styleSystem.AddStyleEvent(uid, "-SLIP", styleComp);
        }
    }
}
