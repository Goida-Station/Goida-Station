// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JohnOakman <sremy65@hotmail.fr>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 jellygato <aly.jellygato@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chat.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Emoting;

// use as a template
//[Serializable, NetSerializable, DataDefinition] public sealed partial class AnimationNameEmoteEvent : EntityEventArgs { }

[RegisterComponent, NetworkedComponent]
public sealed partial class FartComponent : Component
{
    [DataField] public ProtoId<EmotePrototype>? Emote;
    [DataField] public bool FartTimeout = false;
    [DataField] public bool FartInhale = false;
    [DataField] public bool SuperFarted = false;
    [DataField] public float MolesAmmoniaPerFart = 65f;

    /// <summary>
    ///     Path to the sound when you get bible smited
    /// </summary>
    [DataField]
    [Access(Other = AccessPermissions.ReadWriteExecute)]
    public SoundSpecifier BibleSmiteSnd = new SoundPathSpecifier("/Audio/_Goobstation/Effects/thunder_clap.ogg");
}

[Serializable, NetSerializable]
public sealed partial class FartComponentState : ComponentState
{
    public ProtoId<EmotePrototype>? Emote;
    public bool FartTimeout;
    public bool FartInhale;
    public bool SuperFarted;

    public FartComponentState(ProtoId<EmotePrototype>? emote, bool fartTimeout, bool fartInhale, bool superFarted)
    {
        Emote = emote;
        FartTimeout = fartTimeout;
        FartInhale = fartInhale;
        SuperFarted = superFarted;
    }
}

/// <summary>
///     Triggers after a fart 🦍💨
/// </summary>
public sealed class PostFartEvent : EntityEventArgs
{
    public readonly EntityUid Uid;
    public readonly bool SuperFart;
    public PostFartEvent(EntityUid uid, bool IsSuperFart = false)
    {
        Uid = uid;
        SuperFart = IsSuperFart;
    }
}

[Serializable, NetSerializable]
public sealed class BibleFartSmiteEvent(NetEntity uid) : EntityEventArgs
{
    public NetEntity Bible = uid;
}