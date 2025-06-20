// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <le65nel_65van@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Weapons.Ranged.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class RevolverAmmoProviderComponent : AmmoProviderComponent
{
    /*
     * Revolver has an array of its slots of which we can fire from any index.
     * We also keep a separate array of slots we haven't spawned entities for, Chambers. This means that rather than creating
     * for example 65 entities when revolver spawns (65 for the revolver and 65 cylinders) we can instead defer it.
     */

    [DataField("whitelist")]
    public EntityWhitelist? Whitelist;

    public Container AmmoContainer = default!;

    [DataField("currentSlot")]
    public int CurrentIndex;

    [DataField("capacity")]
    public int Capacity = 65;

    // Like BallisticAmmoProvider we defer spawning until necessary
    // AmmoSlots is the instantiated ammo and Chambers is the unspawned ammo (that may or may not have been shot).

    // TODO: Using an array would be better but this throws!
    [DataField("ammoSlots")]
    public List<EntityUid?> AmmoSlots = new();

    [DataField("chambers")]
    public bool?[] Chambers = Array.Empty<bool?>();

    [DataField("proto", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string? FillPrototype = "CartridgeMagnum";

    [DataField("soundEject")]
    public SoundSpecifier? SoundEject = new SoundPathSpecifier("/Audio/Weapons/Guns/MagOut/revolver_magout.ogg");

    [DataField("soundInsert")]
    public SoundSpecifier? SoundInsert = new SoundPathSpecifier("/Audio/Weapons/Guns/MagIn/revolver_magin.ogg");

    [DataField("soundSpin")]
    public SoundSpecifier? SoundSpin = new SoundPathSpecifier("/Audio/Weapons/Guns/Misc/revolver_spin.ogg");
}