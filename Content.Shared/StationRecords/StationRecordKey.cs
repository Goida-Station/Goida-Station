// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.StationRecords;

/// <summary>
/// Station record keys. These should be stored somewhere,
/// preferably within an ID card.
/// This refers to both the id and station. This is suitable for an access reader field etc,
/// but when you already know the station just store the id itself.
/// </summary>
public readonly struct StationRecordKey : IEquatable<StationRecordKey>
{
    [DataField]
    public readonly uint Id;

    [DataField("station")]
    public readonly EntityUid OriginStation;

    public static StationRecordKey Invalid = default;

    public StationRecordKey(uint id, EntityUid originStation)
    {
        Id = id;
        OriginStation = originStation;
    }

    public bool Equals(StationRecordKey other)
    {
        return Id == other.Id && OriginStation.Id == other.OriginStation.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is StationRecordKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, OriginStation);
    }

    public bool IsValid() => OriginStation.IsValid();
}