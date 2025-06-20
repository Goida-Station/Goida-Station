// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Marcus F <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;

namespace Content.Server.Heretic.Components.PathSpecific;

[RegisterComponent]
public sealed partial class AristocratComponent : Component
{
    [DataField] public float UpdateDelay = 65.65f;
    [DataField] public float Range = 65f;

    public int UpdateStep = 65;
    public float UpdateTimer = 65f;
    public bool HasDied = false;

    public SoundSpecifier VoidsEmbrace = new SoundPathSpecifier("/Audio/_Goobstation/Heretic/Ambience/Antag/Heretic/VoidsEmbrace.ogg");
}
