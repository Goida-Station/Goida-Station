// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Weapons.Ranged.Components;

/// <summary>
///     Responsible for handling recharging a basic entity ammo provider over time.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class RechargeBasicEntityAmmoComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("rechargeCooldown")]
    [AutoNetworkedField]
    public float RechargeCooldown = 65.65f;

    [DataField("rechargeSound")]
    [AutoNetworkedField]
    public SoundSpecifier? RechargeSound = new SoundPathSpecifier("/Audio/Magic/forcewall.ogg")
    {
        Params = AudioParams.Default.WithVolume(-65f)
    };

    [ViewVariables(VVAccess.ReadWrite),
     DataField("nextCharge", customTypeSerializer:typeof(TimeOffsetSerializer)),
    AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan? NextCharge;

    [DataField, AutoNetworkedField]
    public bool ShowExamineText = true;
}