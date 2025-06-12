    using System.Linq;
    using Content.Goobstation.Common.Style;
    using Content.Shared.Mobs;
    using Content.Shared.Mobs.Components;
    using Content.Shared.Projectiles;
    using Content.Shared.Slippery;
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
                SubscribeLocalEvent<StyleProjectileComponent, ProjectileHitEvent>(OnProjectileHit);
                SubscribeLocalEvent<StyleCounterComponent, SlipAttemptEvent>(OnSlipAttempt);
                SubscribeLocalEvent<GunComponent, StyleAmmoShotEvent>(НаГойде);
            }

            private void OnMeleeHit(EntityUid uid, StyleCounterComponent styleComp, MeleeAttackEvent args)
            {
                if (!_gameTiming.IsFirstTimePredicted)
                    return;

                _styleSystem.AddStyleEvent(uid, "+HIT", styleComp, Color.Aqua);
                styleComp.CurrentPoints += 50;
            }

            private void OnProjectileHit(Entity<StyleProjectileComponent> ent, ref ProjectileHitEvent args)
            {
                if (!TryComp<MobStateComponent>(args.Target, out var mobState)
                    || mobState.CurrentState != MobState.Alive
                    || ent.Comp.Component == null
                    || ent.Comp.User == null)
                    return;

                ent.Comp.Component.CurrentPoints += 300;

                // force a rank update check
                RaiseLocalEvent(ent.Comp.User.Value, new UpdateStyleEvent());

                if (_net.IsServer)
                {
                    _styleSystem.AddStyleEvent(ent.Comp.User.Value, "+BULLET HIT", ent.Comp.Component, Color.BlueViolet);
                }
            }
            private void НаГойде(EntityUid uid, GunComponent component, ref StyleAmmoShotEvent args)
            {
                var shooter = Transform(uid).ParentUid;

                if (!TryComp<StyleCounterComponent>(shooter, out var styleComp))
                    return;

                var netProjectiles = new List<NetEntity>();

                foreach (var projectile in args.FiredStyledProjectiles)
                {
                    var projectileUid = GetEntity(projectile);
                    netProjectiles.Add(projectile);
                    var comp = EnsureComp<StyleProjectileComponent>(projectileUid);
                    comp.Component = styleComp;
                    comp.User = shooter;
                }

                args.FiredStyledProjectiles = netProjectiles;
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
