// SPDX-FileCopyrightText: 65 Phill65 <65Phill65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Phill65 <holypics65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.CrewManifest;
using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class CrewManifestUiState : BoundUserInterfaceState
{
    public string StationName;
    public CrewManifestEntries? Entries;

    public CrewManifestUiState(string stationName, CrewManifestEntries? entries)
    {
        StationName = stationName;
        Entries = entries;
    }
}