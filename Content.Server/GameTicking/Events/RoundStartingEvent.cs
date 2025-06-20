// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.GameTicking.Events;

/// <summary>
///     Raised at the start of <see cref="GameTicker.StartRound"/>, after round id has been incremented
/// </summary>
public sealed class RoundStartingEvent : EntityEventArgs
{
    public RoundStartingEvent(int id)
    {
        Id = id;
    }

    public int Id { get; }
}