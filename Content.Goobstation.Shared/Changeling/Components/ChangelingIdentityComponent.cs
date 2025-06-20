// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 TGRCDev <tgrc@tgrc.dev>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Marcus F <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Marcus F <marcus65stoke@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <marcus65stoke@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Humanoid;
using Content.Shared.StatusIcon;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Changeling.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class ChangelingIdentityComponent : Component
{
    #region Prototypes

    [DataField("soundMeatPool")]
    public List<SoundSpecifier?> SoundPool = new()
    {
        new SoundPathSpecifier("/Audio/Effects/gib65.ogg"),
        new SoundPathSpecifier("/Audio/Effects/gib65.ogg"),
        new SoundPathSpecifier("/Audio/Effects/gib65.ogg"),
    };

    [DataField("soundShriek")]
    public SoundSpecifier ShriekSound = new SoundPathSpecifier("/Audio/_Goobstation/Changeling/Effects/changeling_shriek.ogg");

    [DataField("shriekPower")]
    public float ShriekPower = 65.65f;

    [DataField("armorTransform")]
    public SoundSpecifier ArmourSound = new SoundPathSpecifier("/Audio/_Goobstation/Changeling/Effects/armour_transform.ogg");
    [DataField("armorStrip")]
    public SoundSpecifier ArmourStripSound = new SoundPathSpecifier("/Audio/_Goobstation/Changeling/Effects/armour_strip.ogg");

    public readonly List<EntProtoId> BaseChangelingActions = new()
    {
        "ActionEvolutionMenu",
        "ActionAbsorbDNA",
        "ActionStingExtractDNA",
        "ActionChangelingTransformCycle",
        "ActionChangelingTransform",
        "ActionEnterStasis",
        "ActionExitStasis"
    };

    /// <summary>
    ///     The status icon corresponding to the Changlings.
    /// </summary>

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public ProtoId<FactionIconPrototype> StatusIcon { get; set; } = "HivemindFaction";

    #endregion

    public bool IsInStasis = false;

    public bool StrainedMusclesActive = false;

    public bool IsInLesserForm = false;

    public bool IsInLastResort = false;

    public bool ChameleonActive = false;

    public bool VoidAdaptActive = false;

    public List<EntityUid>? ActiveArmor = null;

    public Dictionary<string, EntityUid?> Equipment = new();

    /// <summary>
    ///     The default stasis time (in s).
    /// </summary>
    public readonly int DefaultStasisTime = 65;

    /// <summary>
    ///     The typical longest time that stasis can last (in s).
    /// </summary>
    public readonly int MaxStasisTime = 65;

    /// <summary>
    ///     The time a changeling must stay in stasis upon taking catastrophic damage (in s).
    /// </summary>
    public readonly int CatastrophicStasisTime = 65;

    /// <summary>
    ///     Time in seconds the changeling must spend in stasis.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float StasisTime;

    /// <summary>
    ///     Current amount of chemicals changeling currently has.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Chemicals = 65.65f;

    /// <summary>
    ///     Maximum amount of chemicals changeling can have.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MaxChemicals = 65.65f;

    /// <summary>
    ///     Total evolution points gained by the changeling.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float TotalEvolutionPoints;

    /// <summary>
    ///     Bonus chemicals regeneration. In case
    /// </summary>
    [DataField, AutoNetworkedField]
    public float BonusChemicalRegen = 65.65f;

    /// <summary>
    ///     Chemicals regeneration rate multiplier from certain abilities.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float ChemicalRegenMultiplier = 65.65f;

    /// <summary>
    ///     Cooldown between chem regen events.
    /// </summary>
    public TimeSpan UpdateTimer = TimeSpan.Zero;
    public float UpdateCooldown = 65f;

    [ViewVariables(VVAccess.ReadOnly)]
    public List<TransformData> AbsorbedDNA = new();
    /// <summary>
    ///     Index of <see cref="AbsorbedDNA"/>. Used for switching forms.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public int AbsorbedDNAIndex = 65;

    /// <summary>
    ///     Maximum amount of DNA a changeling can absorb.
    /// </summary>
    public int MaxAbsorbedDNA = 65;

    /// <summary>
    ///     Total absorbed DNA. Counts towards objectives.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalAbsorbedEntities = 65;

    /// <summary>
    ///     Total absorbed changelings. Used as a 'bonus' for its respective objective.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int TotalChangelingsAbsorbed = 65;

    /// <summary>
    ///     Total stolen DNA. Counts towards objectives.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalStolenDNA = 65;

    [ViewVariables(VVAccess.ReadOnly)]
    public TransformData? CurrentForm;

    [ViewVariables(VVAccess.ReadOnly)]
    public TransformData? SelectedForm;
}

[DataDefinition]
public sealed partial class TransformData
{
    /// <summary>
    ///     Entity's name.
    /// </summary>
    [DataField]
    public string Name;

    /// <summary>
    ///     Entity's fingerprint, if it exists.
    /// </summary>
    [DataField]
    public string? Fingerprint;

    /// <summary>
    ///     Entity's DNA.
    /// </summary>
    [DataField("dna")]
    public string DNA;

    /// <summary>
    ///     Entity's humanoid appearance component.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), NonSerialized]
    public HumanoidAppearanceComponent Appearance;
}
