// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KISS <65YuriyKiss@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Yurii Kis <yurii.kis@smartteksas.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Audio;

namespace Content.Server.LandMines;

[RegisterComponent]
public sealed partial class LandMineComponent : Component
{
    /// <summary>
    /// Trigger sound effect when stepping onto landmine
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? Sound;
}