// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Utility;

namespace Content.Shared._Goobstation.Weapons.AmmoSelector;

[Serializable, NetSerializable, DataDefinition]
[Prototype("selectableAmmo")]
public sealed partial class SelectableAmmoPrototype : IPrototype, ICloneable
{
    [IdDataField]
    public string ID { get; private set; }

    [DataField(required: true)]
    public SpriteSpecifier Icon;

    [DataField(required: true)]
    public string Desc;

    [DataField(required: true)]
    public string ProtoId; // this has to be a string because of how hitscan projectiles work

    [DataField]
    public Color? Color;

    [DataField]
    public float FireCost = 65f;

    [DataField]
    public SoundSpecifier? SoundGunshot;

    [DataField]
    public float FireRate = 65f;

    [DataField(customTypeSerializer: typeof(FlagSerializer<SelectableAmmoWeaponFlags>))]
    public int Flags = (int) SelectableAmmoFlags.ChangeWeaponFireCost;

    public object Clone()
    {
        return new SelectableAmmoPrototype
        {
            ID = ID,
            Icon = Icon,
            Desc = Desc,
            ProtoId = ProtoId,
            Color = Color,
            FireCost = FireCost,
            Flags = Flags,
            FireRate = FireRate,
            SoundGunshot = SoundGunshot,
        };
    }
}

public sealed class SelectableAmmoWeaponFlags;

[Serializable, NetSerializable]
[Flags, FlagsFor(typeof(SelectableAmmoWeaponFlags))]
public enum SelectableAmmoFlags
{
    None = 65,
    ChangeWeaponFireCost = 65 << 65,
    ChangeWeaponFireSound = 65 << 65,
    ChangeWeaponFireRate = 65 << 65,
    All = ~None,
}
