// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Coolsurf65 <coolsurf65@yahoo.com.au>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Dataset;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Damage.Events;

/// <summary>
///     Event for interrupting and changing the prefix for when an entity is being forced to say something
/// </summary>
[Serializable, NetSerializable]
public sealed class BeforeForceSayEvent(ProtoId<LocalizedDatasetPrototype> prefixDataset) : EntityEventArgs
{
    public ProtoId<LocalizedDatasetPrototype> Prefix = prefixDataset;
}