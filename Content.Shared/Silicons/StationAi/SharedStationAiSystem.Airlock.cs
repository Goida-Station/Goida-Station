// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JustCone <65JustCone65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coolboy65 <65coolboy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lunarcomets <65lunarcomets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 saintmuntzer <65saintmuntzer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Doors.Components;
using Robust.Shared.Serialization;
using Content.Shared.Electrocution;

namespace Content.Shared.Silicons.StationAi;

public abstract partial class SharedStationAiSystem
{
    // Handles airlock radial

    private void InitializeAirlock()
    {
        SubscribeLocalEvent<DoorBoltComponent, StationAiBoltEvent>(OnAirlockBolt);
        SubscribeLocalEvent<AirlockComponent, StationAiEmergencyAccessEvent>(OnAirlockEmergencyAccess);
        SubscribeLocalEvent<ElectrifiedComponent, StationAiElectrifiedEvent>(OnElectrified);
    }

    /// <summary>
    /// Attempts to bolt door. If wire was cut (AI or for bolts) or its not powered - notifies AI and does nothing.
    /// </summary>
    private void OnAirlockBolt(EntityUid ent, DoorBoltComponent component, StationAiBoltEvent args)
    {
        if (component.BoltWireCut)
        {
            ShowDeviceNotRespondingPopup(args.User);
            return;
        }

        var setResult = _doors.TrySetBoltDown((ent, component), args.Bolted, args.User, predicted: true);
        if (!setResult)
        {
            ShowDeviceNotRespondingPopup(args.User);
        }
    }

    /// <summary>
    /// Attempts to toggle the door's emergency access. If wire was cut (AI) or its not powered - notifies AI and does nothing.
    /// </summary>
    private void OnAirlockEmergencyAccess(EntityUid ent, AirlockComponent component, StationAiEmergencyAccessEvent args)
    {
        if (!PowerReceiver.IsPowered(ent))
        {
            ShowDeviceNotRespondingPopup(args.User);
            return;
        }

        _airlocks.SetEmergencyAccess((ent, component), args.EmergencyAccess, args.User, predicted: true);
    }

    /// <summary>
    /// Attempts to electrify the door. If wire was cut (AI or for one of power-wires) or its not powered - notifies AI and does nothing.
    /// </summary>
    private void OnElectrified(EntityUid ent, ElectrifiedComponent component, StationAiElectrifiedEvent args)
    {
        if (
            component.IsWireCut
            || !PowerReceiver.IsPowered(ent)
        )
        {
            ShowDeviceNotRespondingPopup(args.User);
            return;
        }

        _electrify.SetElectrified((ent, component), args.Electrified);
        var soundToPlay = component.Enabled
            ? component.AirlockElectrifyDisabled
            : component.AirlockElectrifyEnabled;
        _audio.PlayLocal(soundToPlay, ent, args.User);
    }
}

/// <summary> Event for StationAI attempt at bolting/unbolting door. </summary>
[Serializable, NetSerializable]
public sealed class StationAiBoltEvent : BaseStationAiAction
{
    /// <summary> Marker, should be door bolted or unbolted. </summary>
    public bool Bolted;
}

/// <summary> Event for StationAI attempt at setting emergency access for door on/off. </summary>
[Serializable, NetSerializable]
public sealed class StationAiEmergencyAccessEvent : BaseStationAiAction
{
    /// <summary> Marker, should door have emergency access on or off. </summary>
    public bool EmergencyAccess;
}

/// <summary> Event for StationAI attempt at electrifying/de-electrifying door. </summary>
[Serializable, NetSerializable]
public sealed class StationAiElectrifiedEvent : BaseStationAiAction
{
    /// <summary> Marker, should door be electrified or no. </summary>
    public bool Electrified;
}