// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Lavaland.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server._Lavaland.Audio;

public sealed class BossMusicSystem : SharedBossMusicSystem
{
    public override void StartBossMusic(ProtoId<BossMusicPrototype> music) { }

    public override void EndAllMusic() { }
}
