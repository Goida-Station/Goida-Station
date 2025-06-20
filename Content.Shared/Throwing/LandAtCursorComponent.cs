// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Throwing
{
    /// <summary>
    ///     Makes an item land at the cursor when thrown and slide a little further.
    ///     Without it the item lands slightly in front and stops moving at the cursor.
    ///     Use this for throwing weapons that should pierce the opponent, for example spears.
    /// </summary>
    [RegisterComponent, NetworkedComponent]
    public sealed partial class LandAtCursorComponent : Component { }
}