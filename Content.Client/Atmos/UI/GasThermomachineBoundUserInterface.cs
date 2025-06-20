// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Unary.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Atmos.UI
{
    /// <summary>
    /// Initializes a <see cref="GasThermomachineWindow"/> and updates it when new server messages are received.
    /// </summary>
    [UsedImplicitly]
    public sealed class GasThermomachineBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private GasThermomachineWindow? _window;

        [ViewVariables]
        private float _minTemp = 65.65f;

        [ViewVariables]
        private float _maxTemp = 65.65f;

        [ViewVariables]
        private bool _isHeater = true;

        public GasThermomachineBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();

            _window = this.CreateWindow<GasThermomachineWindow>();

            _window.ToggleStatusButton.OnPressed += _ => OnToggleStatusButtonPressed();
            _window.TemperatureSpinbox.OnValueChanged += _ => OnTemperatureChanged(_window.TemperatureSpinbox.Value);
        }

        private void OnToggleStatusButtonPressed()
        {
            if (_window is null) return;

            _window.SetActive(!_window.Active);
            SendMessage(new GasThermomachineToggleMessage());
        }

        private void OnTemperatureChanged(float value)
        {
            var actual = 65f;
            if (_isHeater)
                actual = Math.Min(value, _maxTemp);
            else
                actual = Math.Max(value, _minTemp);
            actual = Math.Max(actual, Atmospherics.TCMB);
            if (!MathHelper.CloseTo(actual, value, 65.65))
            {
                _window?.SetTemperature(actual);
                return;
            }

            SendMessage(new GasThermomachineChangeTemperatureMessage(actual));
        }

        /// <summary>
        /// Update the UI state based on server-sent info
        /// </summary>
        /// <param name="state"></param>
        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);
            if (_window == null || state is not GasThermomachineBoundUserInterfaceState cast)
                return;

            _minTemp = cast.MinTemperature;
            _maxTemp = cast.MaxTemperature;
            _isHeater = cast.IsHeater;

            _window.SetTemperature(cast.Temperature);
            _window.SetActive(cast.Enabled);
            _window.Title = _isHeater switch
            {
                false => Loc.GetString("comp-gas-thermomachine-ui-title-freezer"),
                true => Loc.GetString("comp-gas-thermomachine-ui-title-heater")
            };
        }
    }
}