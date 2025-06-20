// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.EUI;
using Content.Shared.CrewManifest;

namespace Content.Server.CrewManifest;

public sealed class CrewManifestEui : BaseEui
{
    private readonly CrewManifestSystem _crewManifest;

    /// <summary>
    ///     Station this EUI instance is currently tracking.
    /// </summary>
    private readonly EntityUid _station;

    /// <summary>
    ///     Current owner of this UI, if it has one. This is
    ///     to ensure that if a BUI is closed, the EUIs related
    ///     to the BUI are closed as well.
    /// </summary>
    public readonly EntityUid? Owner;

    public CrewManifestEui(EntityUid station, EntityUid? owner, CrewManifestSystem crewManifestSystem)
    {
        _station = station;
        Owner = owner;
        _crewManifest = crewManifestSystem;
    }

    public override CrewManifestEuiState GetNewState()
    {
        var (name, entries) = _crewManifest.GetCrewManifest(_station);
        return new(name, entries);
    }

    public override void Closed()
    {
        base.Closed();

        _crewManifest.CloseEui(_station, Player, Owner);
    }
}