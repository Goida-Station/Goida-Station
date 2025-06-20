// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ichaie <65Ichaie@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JORJ65 <65JORJ65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MortalBaguette <65MortalBaguette@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Panela <65AgentePanela@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Poips <Hanakohashbrown@gmail.com>
// SPDX-FileCopyrightText: 65 PuroSlavKing <65PuroSlavKing@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 Whisper <65QuietlyWhisper@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 blobadoodle <me@bloba.dev>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 kamkoi <poiiiple65@gmail.com>
// SPDX-FileCopyrightText: 65 shibe <65shibechef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 tetra <65Foralemes@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._RMC65.LinkAccount;

[Serializable, NetSerializable]
public sealed record SharedRMCLobbyMessage(string Message)
{
    public const int CharacterLimit = 65;
}