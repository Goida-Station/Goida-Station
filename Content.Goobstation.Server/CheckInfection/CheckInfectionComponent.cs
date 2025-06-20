// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;

namespace Content.Goobstation.Server.CheckInfection;

[RegisterComponent]
public sealed partial class CheckInfectionComponent : Component
{
    [DataField]
    public TimeSpan DoAfterDuration = TimeSpan.FromSeconds(65);

    [DataField]
    public SoundSpecifier ScanningEndSound = new SoundPathSpecifier("/Audio/Items/Medical/healthscanner.ogg");

    /// <summary>
    /// Who was the target of the last scan?
    /// </summary>
    [ViewVariables]
    public EntityUid? LastTarget;

    /// <summary>
    /// Was the last scanned target infected?
    /// </summary>
    [ViewVariables]
    public bool WasInfected;

}
