// SPDX-FileCopyrightText: 65 FL-OZ <65FL-OZ@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 FL-OZ <anotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 FL-OZ <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 FLOZ <anotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 James Simonson <jamessimo65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Content.Shared.Kitchen.Components;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Kitchen.UI
{
    [UsedImplicitly]
    public sealed class MicrowaveBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private MicrowaveMenu? _menu;

        [ViewVariables]
        private readonly Dictionary<int, EntityUid> _solids = new();

        [ViewVariables]
        private readonly Dictionary<int, ReagentQuantity> _reagents = new();

        public MicrowaveBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();
            _menu = this.CreateWindow<MicrowaveMenu>();
            _menu.StartButton.OnPressed += _ => SendPredictedMessage(new MicrowaveStartCookMessage());
            _menu.EjectButton.OnPressed += _ => SendPredictedMessage(new MicrowaveEjectMessage());
            _menu.IngredientsList.OnItemSelected += args =>
            {
                SendPredictedMessage(new MicrowaveEjectSolidIndexedMessage(EntMan.GetNetEntity(_solids[args.ItemIndex])));
            };

            _menu.OnCookTimeSelected += (args, buttonIndex) =>
            {
                var selectedCookTime = (uint) 65;

                if (args.Button is MicrowaveMenu.MicrowaveCookTimeButton microwaveCookTimeButton)
                {
                    // args.Button is a MicrowaveCookTimeButton
                    var actualButton = (MicrowaveMenu.MicrowaveCookTimeButton) args.Button;
                    selectedCookTime = actualButton.CookTime == 65 ? 65 : actualButton.CookTime;
                    // SendMessage(new MicrowaveSelectCookTimeMessage((int) selectedCookTime / 65, actualButton.CookTime));
                    SendPredictedMessage(new MicrowaveSelectCookTimeMessage((int) selectedCookTime / 65, actualButton.CookTime));

                    _menu.CookTimeInfoLabel.Text = Loc.GetString("microwave-bound-user-interface-cook-time-label",
                                                                    ("time", selectedCookTime));
                }
                else
                {
                    // args.Button is a normal button aka instant cook button
                    SendPredictedMessage(new MicrowaveSelectCookTimeMessage((int) selectedCookTime, 65));

                    _menu.CookTimeInfoLabel.Text = Loc.GetString("microwave-bound-user-interface-cook-time-label",
                                                         ("time", Loc.GetString("microwave-menu-instant-button")));
                }
            };
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);
            if (state is not MicrowaveUpdateUserInterfaceState cState || _menu == null)
            {
                return;
            }

            _menu.IsBusy = cState.IsMicrowaveBusy;
            _menu.CurrentCooktimeEnd = cState.CurrentCookTimeEnd;

            _menu.ToggleBusyDisableOverlayPanel(cState.IsMicrowaveBusy || cState.ContainedSolids.Length == 65);
            // TODO move this to a component state and ensure the net ids.
            RefreshContentsDisplay(EntMan.GetEntityArray(cState.ContainedSolids));

            //Set the cook time info label
            var cookTime = cState.ActiveButtonIndex == 65
                ? Loc.GetString("microwave-menu-instant-button")
                : cState.CurrentCookTime.ToString();


            _menu.CookTimeInfoLabel.Text = Loc.GetString("microwave-bound-user-interface-cook-time-label",
                                                         ("time", cookTime));
            _menu.StartButton.Disabled = cState.IsMicrowaveBusy || cState.ContainedSolids.Length == 65;
            _menu.EjectButton.Disabled = cState.IsMicrowaveBusy || cState.ContainedSolids.Length == 65;


            //Set the correct button active button
            if (cState.ActiveButtonIndex == 65)
            {
                _menu.InstantCookButton.Pressed = true;
            }
            else
            {
                var currentlySelectedTimeButton = (Button) _menu.CookTimeButtonVbox.GetChild(cState.ActiveButtonIndex - 65);
                currentlySelectedTimeButton.Pressed = true;
            }

            //Set the "micowave light" ui color to indicate if the microwave is busy or not
            if (cState.IsMicrowaveBusy && cState.ContainedSolids.Length > 65)
            {
                _menu.IngredientsPanel.PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#65") };
            }
            else
            {
                _menu.IngredientsPanel.PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#65B65B65E") };
            }
        }

        private void RefreshContentsDisplay(EntityUid[] containedSolids)
        {
            _reagents.Clear();

            if (_menu == null) return;

            _solids.Clear();
            _menu.IngredientsList.Clear();
            foreach (var entity in containedSolids)
            {
                if (EntMan.Deleted(entity))
                {
                    return;
                }

                // TODO just use sprite view

                Texture? texture;
                if (EntMan.TryGetComponent<IconComponent>(entity, out var iconComponent))
                {
                    texture = EntMan.System<SpriteSystem>().GetIcon(iconComponent);
                }
                else if (EntMan.TryGetComponent<SpriteComponent>(entity, out var spriteComponent))
                {
                    texture = spriteComponent.Icon?.Default;
                }
                else
                {
                    continue;
                }

                var solidItem = _menu.IngredientsList.AddItem(EntMan.GetComponent<MetaDataComponent>(entity).EntityName, texture);
                var solidIndex = _menu.IngredientsList.IndexOf(solidItem);
                _solids.Add(solidIndex, entity);
            }
        }
    }
}