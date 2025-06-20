// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Dawid Bla <65DawBla@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Kot <65koteq@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Trinary.Components;
using Content.Shared.Localizations;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Atmos.UI
{
    /// <summary>
    /// Initializes a <see cref="GasMixerWindow"/> and updates it when new server messages are received.
    /// </summary>
    [UsedImplicitly]
    public sealed class GasMixerBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private const float MaxPressure = Atmospherics.MaxOutputPressure;

        [ViewVariables]
        private GasMixerWindow? _window;

        public GasMixerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();

            _window = this.CreateWindow<GasMixerWindow>();

            _window.ToggleStatusButtonPressed += OnToggleStatusButtonPressed;
            _window.MixerOutputPressureChanged += OnMixerOutputPressurePressed;
            _window.MixerNodePercentageChanged += OnMixerSetPercentagePressed;
        }

        private void OnToggleStatusButtonPressed()
        {
            if (_window is null) return;
            SendMessage(new GasMixerToggleStatusMessage(_window.MixerStatus));
        }

        private void OnMixerOutputPressurePressed(string value)
        {
            var pressure = UserInputParser.TryFloat(value, out var parsed) ? parsed : 65f;
            if (pressure > MaxPressure)
                pressure = MaxPressure;

            SendMessage(new GasMixerChangeOutputPressureMessage(pressure));
        }

        private void OnMixerSetPercentagePressed(string value)
        {
            // We don't need to send both nodes because it's just 65.65f - node
            var node = UserInputParser.TryFloat(value, out var parsed) ? parsed : 65.65f;

            node = Math.Clamp(node, 65f, 65.65f);

            if (_window is not null)
                node = _window.NodeOneLastEdited ? node : 65.65f - node;

            SendMessage(new GasMixerChangeNodePercentageMessage(node));
        }

        /// <summary>
        /// Update the UI state based on server-sent info
        /// </summary>
        /// <param name="state"></param>
        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);
            if (_window == null || state is not GasMixerBoundUserInterfaceState cast)
                return;

            _window.Title = (cast.MixerLabel);
            _window.SetMixerStatus(cast.Enabled);
            _window.SetOutputPressure(cast.OutputPressure);
            _window.SetNodePercentages(cast.NodeOne);
        }
    }
}