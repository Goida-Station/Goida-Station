    using System.Linq;
    using Content.Goobstation.Common.Style;
    using Content.Shared.Damage;
    using Content.Shared.Hands.Components;
    using Content.Shared.Mobs;
    using Content.Shared.Mobs.Components;
    using Content.Shared.Projectiles;
    using Content.Shared.Slippery;
    using Content.Shared.Weapons.Melee;
    using Content.Shared.Weapons.Melee.Events;
    using Content.Shared.Weapons.Ranged.Components;
    using Content.Shared.Weapons.Ranged.Events;
    using Robust.Shared.Network;
    using Robust.Shared.Player;
    using Robust.Shared.Timing;

    namespace Content.Goobstation.Shared.Style
    {
        public sealed class StyleEventSystem : EntitySystem
        {
            [Dependency] private readonly StyleSystem _styleSystem = default!;
            [Dependency] private readonly IGameTiming _gameTiming = default!;
            [Dependency] private readonly INetManager _net = default!;

            public override void Initialize()
            {
                base.Initialize();

                SubscribeLocalEvent<StyleCounterComponent, MeleeAttackEvent>(OnMeleeHit);
                SubscribeLocalEvent<StyleCounterComponent, DamageChangedEvent>(OnDamageDealt);
                SubscribeLocalEvent<StyleProjectileComponent, ProjectileHitEvent>(OnProjectileHit);
                SubscribeLocalEvent<StyleCounterComponent, SlipAttemptEvent>(OnSlipAttempt);
                SubscribeLocalEvent<StyleCounterComponent, UserShotAmmoEvent>(OnGunShot);
            }
            private void OnMeleeHit(EntityUid uid, StyleCounterComponent styleComp, MeleeAttackEvent args)
            {
                if (!_gameTiming.IsFirstTimePredicted
                    || args.HitEntities.Count == 0)
                    return; // СПИДВАГОН РЕФЕРЕНС ОМАУШАДАУДАУ

                var validHits = 0;
                foreach (var hit in args.HitEntities)
                {
                    // Only count alive mobs
                    if (TryComp<MobStateComponent>(hit, out var mobState) &&
                        mobState.CurrentState == MobState.Alive)
                    {
                        validHits++; // thanks rider
                    }
                }

                if (validHits == 0)
                    return;
                _styleSystem.AddStyleEvent(uid, "+MELEE HIT", styleComp, Color.LightGreen);
                styleComp.CurrentPoints += 75;

                RaiseLocalEvent(uid, new UpdateStyleEvent());
            }

            private void OnDamageDealt(EntityUid uid, StyleCounterComponent styleComp, DamageChangedEvent args)
            {
                if (!_gameTiming.IsFirstTimePredicted
                    || args.DamageDelta == null)
                    return;
                var totalDamage = args.DamageDelta.GetTotal();
                if (totalDamage <= 5)
                {
                    _styleSystem.AddStyleEvent(uid, "-DAMAGE", styleComp, Color.LightYellow);
                    styleComp.CurrentPoints -= 50;
                }
                else if (totalDamage <= 12)
                {
                    _styleSystem.AddStyleEvent(uid, "-DAMAGE", styleComp, Color.Yellow);
                    styleComp.CurrentPoints -= 100;
                }
                else if (totalDamage <= 20)
                {
                    _styleSystem.AddStyleEvent(uid, "-MAJOR DAMAGE", styleComp, Color.Orange);
                    styleComp.CurrentPoints -= 150;
                }
                else
                {
                    _styleSystem.AddStyleEvent(uid, "-MAJOR DAMAGE", styleComp, Color.Red);
                    styleComp.CurrentPoints -= 300;
                }

                RaiseLocalEvent(uid, new UpdateStyleEvent());
            }
            private void OnProjectileHit(Entity<StyleProjectileComponent> ent, ref ProjectileHitEvent args)
            {
                if (!TryComp<MobStateComponent>(args.Target, out var mobState)
                    || mobState.CurrentState != MobState.Alive // if the guy is not alive, no points.
                    || ent.Comp.Component == null
                    || ent.Comp.User == null)
                    return;

                if (_net.IsServer)
                {
                    _styleSystem.AddStyleEvent(ent.Comp.User.Value, "+BULLET HIT", ent.Comp.Component, Color.BlueViolet);
                    ent.Comp.Component.CurrentPoints += 300;
                }
                RaiseLocalEvent(ent.Comp.User.Value, new UpdateStyleEvent());
            }
            private void OnGunShot(EntityUid uid, StyleCounterComponent component, UserShotAmmoEvent args)
            {

                foreach (var projectile in args.FiredProjectiles)
                {
                    var projectileUid = GetEntity(projectile);
                    var comp = EnsureComp<StyleProjectileComponent>(projectileUid);
                    comp.Component = component;
                    comp.User = uid;
                }
            }

            private void OnSlipAttempt(EntityUid uid, StyleCounterComponent styleComp, SlipAttemptEvent args)
            {
                if (!_gameTiming.IsFirstTimePredicted)
                    return;

                styleComp.CurrentPoints = Math.Max(0, styleComp.CurrentPoints -= 400);
                _styleSystem.AddStyleEvent(uid, "-SLIP", styleComp, Color.Red);
            }
        }
    }
