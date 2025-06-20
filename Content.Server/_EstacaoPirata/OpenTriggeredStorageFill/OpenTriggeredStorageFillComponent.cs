// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 RadsammyT <65RadsammyT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Server._EstacaoPirata.OpenTriggeredStorageFill;

/// <summary>
/// This is used for storing an item prototype to be inserted into a container when the trigger is activated. This is deleted from the entity after the item is inserted.
/// </summary>
[RegisterComponent]
public sealed partial class OpenTriggeredStorageFillComponent : Component
{
    [DataField("contents")] public List<EntitySpawnEntry> Contents = new();
}