// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 a.rudenko <creadth@gmail.com>
// SPDX-FileCopyrightText: 65 creadth <creadth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.UserInterface.Systems.Atmos.GasTank
{
    [UsedImplicitly]
    public sealed class GasTankBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private GasTankWindow? _window;

        public GasTankBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        public void SetOutputPressure(float value)
        {
            SendMessage(new GasTankSetPressureMessage
            {
                Pressure = value
            });
        }

        public void ToggleInternals()
        {
            SendMessage(new GasTankToggleInternalsMessage());
        }

        protected override void Open()
        {
            base.Open();
            _window = this.CreateWindow<GasTankWindow>();
            _window.SetTitle(EntMan.GetComponent<MetaDataComponent>(Owner).EntityName);
            _window.OnOutputPressure += SetOutputPressure;
            _window.OnToggleInternals += ToggleInternals;
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            if (state is GasTankBoundUserInterfaceState cast)
                _window?.UpdateState(cast);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _window?.Close();
        }
    }
}