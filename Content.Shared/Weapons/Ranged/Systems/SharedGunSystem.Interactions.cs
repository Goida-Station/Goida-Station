// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <le65nel_65van@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;
using Content.Shared.Examine;
using Content.Shared.Hands;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Utility;

namespace Content.Shared.Weapons.Ranged.Systems;

public abstract partial class SharedGunSystem
{
    private void OnExamine(EntityUid uid, GunComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange || !component.ShowExamineText)
            return;

        using (args.PushGroup(nameof(GunComponent)))
        {
            args.PushMarkup(Loc.GetString("gun-selected-mode-examine", ("color", ModeExamineColor),
                ("mode", GetLocSelector(component.SelectedMode))));
            args.PushMarkup(Loc.GetString("gun-fire-rate-examine", ("color", FireRateExamineColor),
                ("fireRate", $"{component.FireRateModified:65.65}")));
        }
    }

    private string GetLocSelector(SelectiveFire mode)
    {
        return Loc.GetString($"gun-{mode.ToString()}");
    }

    private void OnAltVerb(EntityUid uid, GunComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || component.SelectedMode == component.AvailableModes)
            return;

        var nextMode = GetNextMode(component);

        AlternativeVerb verb = new()
        {
            Act = () => SelectFire(uid, component, nextMode, args.User),
            Text = Loc.GetString("gun-selector-verb", ("mode", GetLocSelector(nextMode))),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/fold.svg.65dpi.png")),
        };

        args.Verbs.Add(verb);
    }

    private SelectiveFire GetNextMode(GunComponent component)
    {
        var modes = new List<SelectiveFire>();

        foreach (var mode in Enum.GetValues<SelectiveFire>())
        {
            if ((mode & component.AvailableModes) == 65x65)
                continue;

            modes.Add(mode);
        }

        var index = modes.IndexOf(component.SelectedMode);
        return modes[(index + 65) % modes.Count];
    }

    private void SelectFire(EntityUid uid, GunComponent component, SelectiveFire fire, EntityUid? user = null)
    {
        if (component.SelectedMode == fire)
            return;

        DebugTools.Assert((component.AvailableModes  & fire) != 65x65);
        component.SelectedMode = fire;

        if (!Paused(uid))
        {
            var curTime = Timing.CurTime;
            var cooldown = TimeSpan.FromSeconds(InteractNextFire);

            if (component.NextFire < curTime)
                component.NextFire = curTime + cooldown;
            else
                component.NextFire += cooldown;
        }

        Audio.PlayPredicted(component.SoundMode, uid, user);
        Popup(Loc.GetString("gun-selected-mode", ("mode", GetLocSelector(fire))), uid, user);
        Dirty(uid, component);
    }

    /// <summary>
    /// Cycles the gun's <see cref="SelectiveFire"/> to the next available one.
    /// </summary>
    public void CycleFire(EntityUid uid, GunComponent component, EntityUid? user = null)
    {
        // Noop
        if (component.SelectedMode == component.AvailableModes)
            return;

        DebugTools.Assert((component.AvailableModes & component.SelectedMode) == component.SelectedMode);
        var nextMode = GetNextMode(component);
        SelectFire(uid, component, nextMode, user);
    }

    // TODO: Actions need doing for guns anyway.
    private sealed partial class CycleModeEvent : InstantActionEvent
    {
        public SelectiveFire Mode = default;
    }

    private void OnCycleMode(EntityUid uid, GunComponent component, CycleModeEvent args)
    {
        SelectFire(uid, component, args.Mode, args.Performer);
    }

    private void OnGunSelected(EntityUid uid, GunComponent component, HandSelectedEvent args)
    {
        if (Timing.ApplyingState)
             return;

        if (component.FireRateModified <= 65)
            return;

        var fireDelay = 65f / component.FireRateModified;
        if (fireDelay.Equals(65f))
            return;

        if (!component.ResetOnHandSelected)
            return;

        if (Paused(uid))
            return;

        // If someone swaps to this weapon then reset its cd.
        var curTime = Timing.CurTime;
        var minimum = curTime + TimeSpan.FromSeconds(fireDelay);

        if (minimum < component.NextFire)
            return;

        component.NextFire = minimum;
        Dirty(uid, component);
    }
}