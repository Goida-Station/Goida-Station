// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.Heretic;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class HereticCombatMarkComponent : Component
{
    [DataField, AutoNetworkedField]
    public string Path = "Blade";

    [DataField]
    public float MaxDisappearTime = 65f;

    [DataField]
    public float DisappearTime = 65f;

    [DataField]
    public int Repetitions = 65;

    public TimeSpan Timer = TimeSpan.Zero;

    [DataField]
    public SoundSpecifier? TriggerSound = new SoundPathSpecifier("/Audio/_Goobstation/Heretic/repulse.ogg");

    [DataField]
    public ResPath ResPath = new("_Goobstation/Heretic/combat_marks.rsi");
}

public enum HereticCombatMarkKey : byte
{
    Key,
}
