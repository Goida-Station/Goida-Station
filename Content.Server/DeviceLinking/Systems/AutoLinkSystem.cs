// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.DeviceLinking.Components;

namespace Content.Server.DeviceLinking.Systems;

/// <summary>
/// This handles automatically linking autolinked entities at round-start.
/// </summary>
public sealed class AutoLinkSystem : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _deviceLinkSystem = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<AutoLinkTransmitterComponent, MapInitEvent>(OnAutoLinkMapInit);
    }

    private void OnAutoLinkMapInit(EntityUid uid, AutoLinkTransmitterComponent component, MapInitEvent args)
    {
        var xform = Transform(uid);

        var query = EntityQueryEnumerator<AutoLinkReceiverComponent>();
        while (query.MoveNext(out var receiverUid, out var receiver))
        {
            if (receiver.AutoLinkChannel != component.AutoLinkChannel)
                continue; // Not ours.

            var rxXform = Transform(receiverUid);

            if (rxXform.GridUid != xform.GridUid)
                continue;

            _deviceLinkSystem.LinkDefaults(null, uid, receiverUid);
        }
    }
}
