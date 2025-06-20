// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Disposal.Unit;

namespace Content.Server.Disposal.Tube;

[ByRefEvent]
public record struct GetDisposalsNextDirectionEvent(DisposalHolderComponent Holder)
{
    public Direction Next;
}