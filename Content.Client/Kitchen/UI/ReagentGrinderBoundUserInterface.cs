// SPDX-FileCopyrightText: 65 Peter Wedder <burneddi@gmail.com>
// SPDX-FileCopyrightText: 65 namespace-Memory <65namespace-Memory@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 scuffedjays <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Crotalus <Crotalus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Containers.ItemSlots;
using Content.Shared.Kitchen;
using Robust.Client.UserInterface;

namespace Content.Client.Kitchen.UI
{
    public sealed class ReagentGrinderBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private GrinderMenu? _menu;

        public ReagentGrinderBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();

            _menu = this.CreateWindow<GrinderMenu>();
            _menu.OnToggleAuto += ToggleAutoMode;
            _menu.OnGrind += StartGrinding;
            _menu.OnJuice += StartJuicing;
            _menu.OnEjectAll += EjectAll;
            _menu.OnEjectBeaker += EjectBeaker;
            _menu.OnEjectChamber += EjectChamberContent;
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);
            if (state is not ReagentGrinderInterfaceState cState)
                return;

            _menu?.UpdateState(cState);
        }

        protected override void ReceiveMessage(BoundUserInterfaceMessage message)
        {
            base.ReceiveMessage(message);
            _menu?.HandleMessage(message);
        }

        public void ToggleAutoMode()
        {
            SendMessage(new ReagentGrinderToggleAutoModeMessage());
        }

        public void StartGrinding()
        {
            SendMessage(new ReagentGrinderStartMessage(GrinderProgram.Grind));
        }

        public void StartJuicing()
        {
            SendMessage(new ReagentGrinderStartMessage(GrinderProgram.Juice));
        }

        public void EjectAll()
        {
            SendMessage(new ReagentGrinderEjectChamberAllMessage());
        }

        public void EjectBeaker()
        {
            SendMessage(new ItemSlotButtonPressedEvent(SharedReagentGrinder.BeakerSlotId));
        }

        public void EjectChamberContent(EntityUid uid)
        {
            SendMessage(new ReagentGrinderEjectChamberContentMessage(EntMan.GetNetEntity(uid)));
        }
    }
}