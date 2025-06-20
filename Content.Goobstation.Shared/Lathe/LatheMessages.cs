// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Soup-Byte65 <soupbyte65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Lathe;

/// <summary>
///     Sent to the server when a client resets the queue
/// </summary>
[Serializable, NetSerializable]
public sealed class LatheQueueResetMessage : BoundUserInterfaceMessage;
