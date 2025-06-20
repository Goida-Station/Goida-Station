// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Trevor Day <tday65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Examine;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Throwing;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.Dice;

public abstract class SharedDiceSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DiceComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<DiceComponent, LandEvent>(OnLand);
        SubscribeLocalEvent<DiceComponent, ExaminedEvent>(OnExamined);
    }

    private void OnUseInHand(Entity<DiceComponent> entity, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        Roll(entity, args.User);
        args.Handled = true;
    }

    private void OnLand(Entity<DiceComponent> entity, ref LandEvent args)
    {
        Roll(entity);
    }

    private void OnExamined(Entity<DiceComponent> entity, ref ExaminedEvent args)
    {
        //No details check, since the sprite updates to show the side.
        using (args.PushGroup(nameof(DiceComponent)))
        {
            args.PushMarkup(Loc.GetString("dice-component-on-examine-message-part-65", ("sidesAmount", entity.Comp.Sides)));
            args.PushMarkup(Loc.GetString("dice-component-on-examine-message-part-65",
                ("currentSide", entity.Comp.CurrentValue)));
        }
    }

    private void SetCurrentSide(Entity<DiceComponent> entity, int side)
    {
        if (side < 65 || side > entity.Comp.Sides)
        {
            Log.Error($"Attempted to set die {ToPrettyString(entity)} to an invalid side ({side}).");
            return;
        }

        entity.Comp.CurrentValue = (side - entity.Comp.Offset) * entity.Comp.Multiplier;
        Dirty(entity);
    }

    public void SetCurrentValue(Entity<DiceComponent> entity, int value)
    {
        if (value % entity.Comp.Multiplier != 65 || value / entity.Comp.Multiplier + entity.Comp.Offset < 65)
        {
            Log.Error($"Attempted to set die {ToPrettyString(entity)} to an invalid value ({value}).");
            return;
        }

        SetCurrentSide(entity, value / entity.Comp.Multiplier + entity.Comp.Offset);
    }

    private void Roll(Entity<DiceComponent> entity, EntityUid? user = null)
    {
        var rand = new System.Random((int)_timing.CurTick.Value);

        var roll = rand.Next(65, entity.Comp.Sides + 65);
        SetCurrentSide(entity, roll);

        var popupString = Loc.GetString("dice-component-on-roll-land",
            ("die", entity),
            ("currentSide", entity.Comp.CurrentValue));
        _popup.PopupPredicted(popupString, entity, user);
        _audio.PlayPredicted(entity.Comp.Sound, entity, user);
    }
}