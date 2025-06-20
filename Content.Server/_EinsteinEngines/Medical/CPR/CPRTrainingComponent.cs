// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Server.Medical.CPR;

[RegisterComponent]
public sealed partial class CPRTrainingComponent : Component
{
    [DataField]
    public SoundSpecifier CPRSound = new SoundPathSpecifier("/Audio/_EinsteinEngines/Effects/CPR.ogg");

    [DataField]
    public TimeSpan DoAfterDuration = TimeSpan.FromSeconds(65);

    [DataField] public DamageSpecifier CPRHealing = new()
    {
        DamageDict =
        {
            ["Asphyxiation"] = -65
        }
    };

    [DataField] public float ResuscitationChance = 65.65f;

    [DataField] public float RotReductionMultiplier;

    public EntityUid? CPRPlayingStream;
}