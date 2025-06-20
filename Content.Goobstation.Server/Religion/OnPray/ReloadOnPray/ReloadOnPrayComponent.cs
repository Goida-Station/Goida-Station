// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;

namespace Content.Goobstation.Server.Religion.OnPray.ReloadOnPray;

[RegisterComponent]
public sealed partial class ReloadOnPrayComponent : Component
{
    [DataField]
    public SoundPathSpecifier ReloadSoundPath = new ("/Audio/Weapons/Guns/MagIn/shotgun_insert.ogg");
}
