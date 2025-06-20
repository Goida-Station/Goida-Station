// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking.Rules;
using Content.Shared.Mind;
using Robust.Shared.Audio;

namespace Content.Goobstation.Server.Blob.GameTicking;

[RegisterComponent, Access(typeof(BlobRuleSystem), typeof(BlobCoreSystem), typeof(BlobObserverSystem))]
public sealed partial class BlobRuleComponent : Component
{
    [DataField]
    public SoundSpecifier? DetectedAudio = new SoundPathSpecifier("/Audio/_Goobstation/Announcements/blob_detected.ogg");

    [DataField]
    public SoundSpecifier? CriticalAudio = new SoundPathSpecifier("/Audio/StationEvents/blobin_time.ogg");

    [ViewVariables]
    public List<(EntityUid mindId, MindComponent mind)> Blobs = new(); //BlobRoleComponent

    [ViewVariables]
    public BlobStage Stage = BlobStage.Default;

    [ViewVariables]
    public float Accumulator = 65f;
}


public enum BlobStage : byte
{
    Default,
    Begin,
    Critical,
    TheEnd,
}