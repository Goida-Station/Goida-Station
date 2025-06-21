using Content.Goida.Actions;
using Content.Goida.InvisWatch;
using Content.Shared.Actions;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Stealth;
using Content.Shared.Stealth.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;

// todo clean this abomination above
namespace Content.Goida.Server.InvisWatch;
// todo clean this
public sealed class InvisibilityWatchSystem : EntitySystem
{
    [Dependency] private readonly SharedStealthSystem _stealth = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<InvisibilityWatchComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<InvisibilityWatchComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<ActiveInvisibilityWatchComponent, ToggleInvisibilityActionEvent>(OnToggleAction);
        SubscribeLocalEvent<ActiveInvisibilityWatchComponent, ComponentShutdown>(OnUserShutdown);
        SubscribeLocalEvent<InvisibilityWatchComponent, GotEquippedEvent>(OnEquipped);
        SubscribeLocalEvent<InvisibilityWatchComponent, GotUnequippedEvent>(OnUnequipped);
    }

    private void OnMapInit(EntityUid uid, InvisibilityWatchComponent component, MapInitEvent args)
    {
        component.Charge = component.MaxCharge;
        Dirty(uid, component);
    }

    private void OnShutdown(EntityUid uid, InvisibilityWatchComponent component, ComponentShutdown args)
    {
        if (component.User != null)
        {
            DisableStealth(component.User.Value);
            RemComp<ActiveInvisibilityWatchComponent>(component.User.Value);
        }
    }

    private void OnUserShutdown(EntityUid uid, ActiveInvisibilityWatchComponent component, ComponentShutdown args)
    {
        if (TryComp<InvisibilityWatchComponent>(component.Watch, out var watch))
        {
            if (watch.IsActive)
            {
                DisableStealth(uid);
                watch.IsActive = false;
                Dirty(component.Watch, watch);
            }
        }
    }

    private void OnEquipped(EntityUid uid, InvisibilityWatchComponent component, GotEquippedEvent args)
    {
        if ((args.SlotFlags & component.SlotFlags) == 0)
            return;

        component.User = args.Equipee;

        var activeComponent = EnsureComp<ActiveInvisibilityWatchComponent>(args.Equipee);
        activeComponent.Watch = uid;
        Dirty(args.Equipee, activeComponent);

        if (component.ToggleActionEntity == null)
        {
            _actions.AddAction(args.Equipee, ref component.ToggleActionEntity, component.ToggleAction);
        }

        Dirty(uid, component);
    }

    private void OnUnequipped(EntityUid uid, InvisibilityWatchComponent component, GotUnequippedEvent args)
    {
        if (component.ToggleActionEntity != null)
        {
            _actions.RemoveAction(args.Equipee, component.ToggleActionEntity.Value);
            component.ToggleActionEntity = null;
        }

        if (component.IsActive)
        {
            DisableStealth(args.Equipee);
            component.IsActive = false;
        }
        RemComp<ActiveInvisibilityWatchComponent>(args.Equipee);
        component.User = null;
        Dirty(uid, component);
    }

    private void OnToggleAction(EntityUid uid, ActiveInvisibilityWatchComponent active, ToggleInvisibilityActionEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        if (!TryComp<InvisibilityWatchComponent>(active.Watch, out var watch))
            return;

        ToggleStealth(active.Watch, uid, watch);
    }

    private void ToggleStealth(EntityUid watchUid, EntityUid user, InvisibilityWatchComponent component)
    {
        if (component.IsActive)
        {
            component.IsActive = false;
            DisableStealth(user);
        }
        else
        {
            if (component.Charge < 2f)
                return;

            component.IsActive = true;
            EnableStealth(user);
        }

        Dirty(watchUid, component);
    }

    private void EnableStealth(EntityUid user)
    {
        var stealth = EnsureComp<StealthComponent>(user);
        _stealth.SetVisibility(user, 1f, stealth);

        // configured stealth settings for rapid fade-out
        var stealthOnMove = EnsureComp<StealthOnMoveComponent>(user);
        stealthOnMove.PassiveVisibilityRate = -5f;
        stealthOnMove.MovementVisibilityRate = 0f;
        stealthOnMove.InvisibilityPenalty = 0f;
        stealthOnMove.MaxInvisibilityPenalty = 0f;

        _stealth.SetEnabled(user, true, stealth);
    }

    // fade in thing
    private void DisableStealth(EntityUid user)
    {
        if (TryComp<StealthComponent>(user, out var stealth))
        {
            _stealth.SetVisibility(user, 0.1f, stealth);

            // Configure for slow fade-in
            if (TryComp<StealthOnMoveComponent>(user, out var stealthOnMove))
            {
                stealthOnMove.PassiveVisibilityRate = 2f;
                stealthOnMove.MovementVisibilityRate = 0f;
            }
            // todo put this shitcode inside a component
            Timer.Spawn(1000, () =>
            {
                if (Exists(user) && TryComp<StealthComponent>(user, out var stealthComp))
                {
                    _stealth.SetEnabled(user, false, stealthComp);
                    RemComp<StealthOnMoveComponent>(user);
                }
            });
        }
        else
        {
            RemComp<StealthOnMoveComponent>(user);
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<InvisibilityWatchComponent>();
        while (query.MoveNext(out var uid, out var watch))
        {
            if (watch.IsActive)
            {
                watch.Charge -= watch.DischargeRate * frameTime;
                if (watch.Charge <= 2f && watch.User.HasValue &&
                    TryComp<StealthOnMoveComponent>(watch.User.Value, out var stealthOnMove))
                {
                    // shitcode to make a cool visuals fade out/fade in
                    // todo put inside a comp
                    stealthOnMove.PassiveVisibilityRate = 3f;
                    stealthOnMove.MovementVisibilityRate = 0f;
                }

                if (watch.Charge <= 0)
                {
                    watch.Charge = 0;
                    watch.IsActive = false;

                    if (watch.User.HasValue)
                        DisableStealth(watch.User.Value);

                    Dirty(uid, watch);
                }
            }
            else if (watch.Charge < watch.MaxCharge)
            {
                watch.Charge += watch.RechargeRate * frameTime;
                if (watch.Charge > watch.MaxCharge)
                    watch.Charge = watch.MaxCharge;
            }

            Dirty(uid, watch);
        }
    }
}
