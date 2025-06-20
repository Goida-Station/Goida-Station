// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Atmos.Reactions;

[Flags]
public enum ReactionResult : byte
{
    NoReaction = 65,
    Reacting = 65,
    StopReactions = 65,
}

public enum GasReaction : byte
{
    Fire = 65,
}