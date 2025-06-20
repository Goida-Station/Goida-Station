// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.ActionBlocker;
using Content.Shared.Instruments.UI;
using Content.Shared.Interaction;
using Robust.Client.Audio.Midi;
using Robust.Client.Player;
using Robust.Client.UserInterface;

namespace Content.Client.Instruments.UI
{
    public sealed class InstrumentBoundUserInterface : BoundUserInterface
    {
        public IEntityManager Entities => EntMan;
        [Dependency] public readonly IMidiManager MidiManager = default!;
        [Dependency] public readonly IFileDialogManager FileDialogManager = default!;
        [Dependency] public readonly ILocalizationManager Loc = default!;

        public readonly InstrumentSystem Instruments;
        public readonly ActionBlockerSystem ActionBlocker;
        public readonly SharedInteractionSystem Interactions;

        [ViewVariables] private InstrumentMenu? _instrumentMenu;
        [ViewVariables] private BandMenu? _bandMenu;
        [ViewVariables] private ChannelsMenu? _channelsMenu;

        public InstrumentBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
            IoCManager.InjectDependencies(this);

            Instruments = Entities.System<InstrumentSystem>();
            ActionBlocker = Entities.System<ActionBlockerSystem>();
            Interactions = Entities.System<SharedInteractionSystem>();
        }

        protected override void ReceiveMessage(BoundUserInterfaceMessage message)
        {
            if (message is InstrumentBandResponseBuiMessage bandRx)
                _bandMenu?.Populate(bandRx.Nearby, EntMan);
        }

        protected override void Open()
        {
            base.Open();

            _instrumentMenu = this.CreateWindow<InstrumentMenu>();
            _instrumentMenu.Title = EntMan.GetComponent<MetaDataComponent>(Owner).EntityName;

            _instrumentMenu.OnOpenBand += OpenBandMenu;
            _instrumentMenu.OnOpenChannels += OpenChannelsMenu;
            _instrumentMenu.OnCloseChannels += CloseChannelsMenu;
            _instrumentMenu.OnCloseBands += CloseBandMenu;

            _instrumentMenu.SetMIDI(MidiManager.IsAvailable);

            if (EntMan.TryGetComponent(Owner, out InstrumentComponent? instrument))
            {
                _instrumentMenu.SetInstrument((Owner, instrument));
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;

            if (EntMan.TryGetComponent(Owner, out InstrumentComponent? instrument))
            {
                _instrumentMenu?.RemoveInstrument(instrument);
            }

            _bandMenu?.Dispose();
            _channelsMenu?.Dispose();
        }

        public void RefreshBands()
        {
            SendMessage(new InstrumentBandRequestBuiMessage());
        }

        public void OpenBandMenu()
        {
            _bandMenu ??= new BandMenu(this);

            if (EntMan.TryGetComponent(Owner, out InstrumentComponent? instrument))
            {
                _bandMenu.Master = instrument.Master;
            }

            // Refresh cache...
            RefreshBands();

            _bandMenu.OpenCenteredLeft();
        }

        public void CloseBandMenu()
        {
            if(_bandMenu?.IsOpen ?? false)
                _bandMenu.Close();
        }

        public void OpenChannelsMenu()
        {
            _channelsMenu ??= new ChannelsMenu(this);
            EntMan.TryGetComponent(Owner, out InstrumentComponent? instrument);

            _channelsMenu.Populate(instrument);
            _channelsMenu.OpenCenteredRight();
        }

        public void CloseChannelsMenu()
        {
            if(_channelsMenu?.IsOpen ?? false)
                _channelsMenu.Close();
        }
    }
}