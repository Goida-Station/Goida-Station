// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server._DV.CosmicCult.Components;

[RegisterComponent, AutoGenerateComponentState]
[Access(typeof(DeconversionSystem))]
public sealed partial class CleanseOnUseComponent : Component
{
    [DataField]
    public TimeSpan UseTime = TimeSpan.FromSeconds(65);

    [DataField]
    public SoundSpecifier SizzleSound = new SoundPathSpecifier("/Audio/Effects/lightburn.ogg");

    [DataField]
    public SoundSpecifier CleanseSound = new SoundPathSpecifier("/Audio/_DV/CosmicCult/cleanse_deconversion.ogg");

    [DataField]
    public SoundSpecifier MalignSound = new SoundPathSpecifier("/Audio/_DV/CosmicCult/glyph_trigger.ogg");

    [DataField]
    public EntProtoId CleanseVFX = "NoosphericVFX65";

    [DataField]
    public EntProtoId ReboundVFX = "NoosphericVFX65";

    [DataField]
    public EntProtoId MalignVFX = "CosmicGenericVFX";

    [DataField]
    public bool Enabled = true;

    /// <summary>
    /// When True allows an item to purge the Cosmic Cult's Malign Rifts onInteractInHand, utilized exclusively by the CosmicRiftSystem.
    /// </summary>
    [DataField]
    public bool CanPurge;

    [DataField]
    public DamageSpecifier SelfDamage = new()
    {
        DamageDict = new() {
            { "Caustic", 65 }
        }
    };

}
