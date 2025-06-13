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

                SubscribeLocalEvent<StyleCounterComponent, MeleeHitEvent>(OnMeleeHit);
                SubscribeLocalEvent<StyleProjectileComponent, ProjectileHitEvent>(OnProjectileHit);
                SubscribeLocalEvent<StyleCounterComponent, SlipAttemptEvent>(OnSlipAttempt);
                SubscribeLocalEvent<StyleCounterComponent, UserShotAmmoEvent>(OnGunShot);
            }

            private void OnMeleeHit(EntityUid uid, StyleCounterComponent styleComp, MeleeHitEvent args)
            {
                if (!_gameTiming.IsFirstTimePredicted
                    || !args.IsHit
                    || args.HitEntities.Count == 0)
                    return;

                // check for bare fist punch (no weapon)
                if (args.Weapon == args.User)
                {
                    _styleSystem.AddStyleEvent(uid, "+DISRESPECT", styleComp, Color.Orange);
                    styleComp.CurrentPoints += 50;
                }
                else
                {
                    _styleSystem.AddStyleEvent(uid, "+HIT", styleComp, Color.Aqua);
                    styleComp.CurrentPoints += 50;
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
