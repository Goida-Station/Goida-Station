// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Bql;

[Serializable, NetSerializable]
public sealed class ToolshedVisualizeEuiState : EuiStateBase
{
    public readonly (string name, NetEntity entity)[] Entities;

    public ToolshedVisualizeEuiState((string name, NetEntity entity)[] entities)
    {
        Entities = entities;
    }
}