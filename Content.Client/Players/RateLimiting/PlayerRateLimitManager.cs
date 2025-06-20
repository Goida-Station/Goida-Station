// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Players.RateLimiting;
using Robust.Shared.Player;

namespace Content.Client.Players.RateLimiting;

public sealed class PlayerRateLimitManager : SharedPlayerRateLimitManager
{
    public override RateLimitStatus CountAction(ICommonSession player, string key)
    {
        // TODO Rate-Limit
        // Add support for rate limit prediction
        // I.e., dont mis-predict just because somebody is clicking too quickly.
        return RateLimitStatus.Allowed;
    }

    public override void Register(string key, RateLimitRegistration registration)
    {
    }

    public override void Initialize()
    {
    }
}