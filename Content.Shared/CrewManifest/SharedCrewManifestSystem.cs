// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.CrewManifest;

/// <summary>
///     A message to send to the server when requesting a crew manifest.
///     CrewManifestSystem will open an EUI that will send the crew manifest
///     to the player when it is updated.
/// </summary>
[Serializable, NetSerializable]
public sealed class RequestCrewManifestMessage : EntityEventArgs
{
    public NetEntity Id { get; }

    public RequestCrewManifestMessage(NetEntity id)
    {
        Id = id;
    }
}

[Serializable, NetSerializable]
public sealed class CrewManifestEuiState : EuiStateBase
{
    public string StationName { get; }
    public CrewManifestEntries? Entries { get; }

    public CrewManifestEuiState(string stationName, CrewManifestEntries? entries)
    {
        StationName = stationName;
        Entries = entries;
    }
}

[Serializable, NetSerializable]
public sealed class CrewManifestEntries
{
    /// <summary>
    ///     Entries in the crew manifest. Goes by department ID.
    /// </summary>
    // public Dictionary<string, List<CrewManifestEntry>> Entries = new();
    public CrewManifestEntry[] Entries = Array.Empty<CrewManifestEntry>();
}

[Serializable, NetSerializable]
public sealed class CrewManifestEntry
{
    public string Name { get; }

    public string JobTitle { get; }

    public string JobIcon { get; }

    public string JobPrototype { get; }

    public CrewManifestEntry(string name, string jobTitle, string jobIcon, string jobPrototype)
    {
        Name = name;
        JobTitle = jobTitle;
        JobIcon = jobIcon;
        JobPrototype = jobPrototype;
    }
}

/// <summary>
///     Tells the server to open a crew manifest UI from
///     this entity's point of view.
/// </summary>
[Serializable, NetSerializable]
public sealed class CrewManifestOpenUiMessage : BoundUserInterfaceMessage
{}