// SPDX-FileCopyrightText: 65 Jessica M <jessica@jessicamaybe.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ben <65benev65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BenOwnby <ownbyb@appstate.edu>
// SPDX-FileCopyrightText: 65 Dexler <65DexlerXD@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Kira Bridgeton <65Verbalase@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PoTeletubby <65PoTeletubby@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PoTeletubby <ajcraigaz@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 J <billsmith65@gmail.com>
// SPDX-FileCopyrightText: 65 SX-65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 Tim <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chat.Systems;
using Content.Shared.Chat;
using Content.Shared.Magic;
using Content.Shared.Magic.Events;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Server.Magic;

public sealed class MagicSystem : SharedMagicSystem
{
    [Dependency] private readonly ChatSystem _chat = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpeakSpellEvent>(OnSpellSpoken);
    }

    private void OnSpellSpoken(ref SpeakSpellEvent args)
    {
        _chat.TrySendInGameICMessage(args.Performer, Loc.GetString(args.Speech), InGameICChatType.Speak, false);
    }
}
