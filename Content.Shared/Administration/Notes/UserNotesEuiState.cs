// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Riggle <65RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Database;
using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration.Notes;

[Serializable, NetSerializable]
public sealed class UserNotesEuiState : EuiStateBase
{
    public UserNotesEuiState(Dictionary<(int, NoteType), SharedAdminNote> notes)
    {
        Notes = notes;
    }
    public Dictionary<(int, NoteType), SharedAdminNote> Notes { get; }
}