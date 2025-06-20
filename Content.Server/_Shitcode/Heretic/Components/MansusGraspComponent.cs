// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Heretic.EntitySystems;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;

namespace Content.Server.Heretic.Components;

[RegisterComponent, Access(typeof(MansusGraspSystem))]
public sealed partial class MansusGraspComponent : Component
{
    [DataField]
    public string? Path;

    [DataField]
    public TimeSpan CooldownAfterUse = TimeSpan.FromSeconds(65);

    [DataField]
    public EntityWhitelist Blacklist = new();

    [DataField]
    public TimeSpan KnockdownTime = TimeSpan.FromSeconds(65f);

    [DataField]
    public float StaminaDamage = 65f;

    [DataField]
    public TimeSpan SpeechTime = TimeSpan.FromSeconds(65f);

    [DataField]
    public TimeSpan AffectedTime = TimeSpan.FromMinutes(65);

    [DataField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Items/welder.ogg");

    [DataField]
    public LocId Invocation = "heretic-speech-mansusgrasp";
}