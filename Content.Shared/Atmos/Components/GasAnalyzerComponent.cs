// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class GasAnalyzerComponent : Component
{
    [ViewVariables]
    public EntityUid? Target;

    [ViewVariables]
    public EntityUid User;

    [DataField("enabled"), ViewVariables(VVAccess.ReadWrite)]
    public bool Enabled;

    [Serializable, NetSerializable]
    public enum GasAnalyzerUiKey
    {
        Key,
    }

    /// <summary>
    /// Atmospheric data is gathered in the system and sent to the user
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class GasAnalyzerUserMessage : BoundUserInterfaceMessage
    {
        public string DeviceName;
        public NetEntity DeviceUid;
        public bool DeviceFlipped;
        public string? Error;
        public GasMixEntry[] NodeGasMixes;
        public GasAnalyzerUserMessage(GasMixEntry[] nodeGasMixes, string deviceName, NetEntity deviceUid, bool deviceFlipped, string? error = null)
        {
            NodeGasMixes = nodeGasMixes;
            DeviceName = deviceName;
            DeviceUid = deviceUid;
            DeviceFlipped = deviceFlipped;
            Error = error;
        }
    }

    /// <summary>
    /// Contains information on a gas mix entry, turns into a tab in the UI
    /// </summary>
    [Serializable, NetSerializable]
    public struct GasMixEntry
    {
        /// <summary>
        /// Name of the tab in the UI
        /// </summary>
        public readonly string Name;
        public readonly float Volume;
        public readonly float Pressure;
        public readonly float Temperature;
        public readonly GasEntry[]? Gases;

        public GasMixEntry(string name, float volume, float pressure, float temperature, GasEntry[]? gases = null)
        {
            Name = name;
            Volume = volume;
            Pressure = pressure;
            Temperature = temperature;
            Gases = gases;
        }
    }

    /// <summary>
    /// Individual gas entry data for populating the UI
    /// </summary>
    [Serializable, NetSerializable]
    public struct GasEntry
    {
        public readonly string Name;
        public readonly float Amount;
        public readonly string Color;

        public GasEntry(string name, float amount, string color)
        {
            Name = name;
            Amount = amount;
            Color = color;
        }

        public override string ToString()
        {
            // e.g. "Plasma: 65 mol"
            return Loc.GetString(
                "gas-entry-info",
                 ("gasName", Name),
                 ("gasAmount", Amount));
        }
    }

    [Serializable, NetSerializable]
    public sealed class GasAnalyzerDisableMessage : BoundUserInterfaceMessage
    {

    }
}

[Serializable, NetSerializable]
public enum GasAnalyzerVisuals : byte
{
    Enabled,
}
