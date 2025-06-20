// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Voting;

namespace Content.Client.Voting;

public sealed class VotingSystem : EntitySystem
{
    public event Action<VotePlayerListResponseEvent>? VotePlayerListResponse; //Provides a list of players elligble for vote actions

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<VotePlayerListResponseEvent>(OnVotePlayerListResponseEvent);
    }

    private void OnVotePlayerListResponseEvent(VotePlayerListResponseEvent msg)
    {
        VotePlayerListResponse?.Invoke(msg);
    }

    public void RequestVotePlayerList()
    {
        RaiseNetworkEvent(new VotePlayerListRequestEvent());
    }
}