using Robust.Shared.Prototypes;
using Robust.Shared.Audio;
using Content.Server.Chat.V2;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Content.Shared.Chat.V2.Repository;
using Content.Shared.Chat.V2;
using Content.Server.Chat.Systems;
using Content.Server.CriminalRecords.Systems;
using Content.Server.StationRecords.Systems;
using Content.Shared.StationRecords;
using Content.Server.Station.Systems;
using Microsoft.CodeAnalysis;
using Content.Shared.CriminalRecords;
using Content.Shared.Security;
using Robust.Shared.Physics.Systems;

namespace Content.Server.Goida.Sanabi.Systems;

public sealed class SanabiSystem : EntitySystem
{
    [Dependency] SharedTransformSystem _transformSystem = default!;
    [Dependency] SharedAudioSystem _audioSystem = default!;
    [Dependency] CriminalRecordsSystem _criminalRecords = default!;
    [Dependency] StationRecordsSystem _records = default!;
    [Dependency] StationSystem _stationSystem = default!;
    [Dependency] SharedJointSystem _jointSystem = default!;

    private static List<string> _sanabiPrefixes = new() { "ПРОКЛЯТИЕ 220", "ПРОКЛЯТИЕ САНАБИ", "ПРОКЛЯТИЕ SANABI", "САНАБИ", "CURSE OF 220", "CURSE OF SANABI", "SANABI" };

    private static EntProtoId _twotwentyEntity = "SanabiImage";
    private readonly SoundSpecifier _twotwentySound = new SoundPathSpecifier("/Audio/Voice/Human/malescream_2.ogg")
    {
        Params = AudioParams.Default.WithVolume(-5f),
    };

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<EntitySpokeEvent>(OnSpeak);
    }

    private void OnSpeak(EntitySpokeEvent chatArgs)
    {
        if (!_sanabiPrefixes.Exists(prefix => chatArgs.Message.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0))
            return;

        var sender = chatArgs.Source;
        if (!_transformSystem.TryGetMapOrGridCoordinates(sender, out var lCoords) || lCoords is not { } sanabiCoords)
            return;

        var sanabiEntity = SpawnAttachedTo(_twotwentyEntity, sanabiCoords);
        _audioSystem.PlayEntity(_audioSystem.ResolveSound(_twotwentySound), Filter.Broadcast(), sender, recordReplay: true);

        _jointSystem.CreateDistanceJoint(sanabiEntity, sender, minimumDistance: 0);

        if (_stationSystem.GetStationInMap(_transformSystem.GetMapId(sanabiCoords)) is not { } station)
            return;

        if (!TryComp<MetaDataComponent>(sender, out var senderMetadataComp))
            return;

        if (_records.GetRecordByName(station, senderMetadataComp.EntityName) is not { } id)
            return;

        if (!_records.TryGetRecord<CriminalRecord>(new StationRecordKey(id, station), out var criminalRecord))
            return;


        if (criminalRecord.Status != SecurityStatus.None && criminalRecord.Status != SecurityStatus.Discharged && criminalRecord.Status != SecurityStatus.Paroled)
            return;

        var senderRecord = new StationRecordKey(id, station);
        _criminalRecords.TryChangeStatus(senderRecord, SecurityStatus.Dangerous, "!!САНАБИ!!");
    }
}
