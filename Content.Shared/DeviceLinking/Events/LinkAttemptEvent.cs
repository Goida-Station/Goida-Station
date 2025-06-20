// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.DeviceLinking.Events;

public sealed class LinkAttemptEvent : CancellableEntityEventArgs
{
    public readonly EntityUid Source;
    public readonly EntityUid Sink;
    public readonly EntityUid? User;
    public readonly string SourcePort;
    public readonly string SinkPort;

    public LinkAttemptEvent(EntityUid? user, EntityUid source, string sourcePort, EntityUid sink, string sinkPort)
    {
        User = user;
        Source = source;
        SourcePort = sourcePort;
        Sink = sink;
        SinkPort = sinkPort;
    }
}