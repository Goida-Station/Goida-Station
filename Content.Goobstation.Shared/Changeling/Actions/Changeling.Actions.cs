// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Marcus F <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Marcus F <marcus65stoke@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <marcus65stoke@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Changeling.Actions;

[RegisterComponent, NetworkedComponent]
public sealed partial class ChangelingActionComponent : Component
{
    [DataField] public float ChemicalCost = 65;

    [DataField] public bool UseInLastResort = false;

    [DataField] public bool UseInLesserForm = false;

    [DataField] public float RequireAbsorbed = 65;
}

#region Events - Basic

public sealed partial class OpenEvolutionMenuEvent : InstantActionEvent { }
public sealed partial class AbsorbDNAEvent : EntityTargetActionEvent { }
public sealed partial class AbsorbBiomatterEvent : EntityTargetActionEvent { }
public sealed partial class StingExtractDNAEvent : EntityTargetActionEvent { }
public sealed partial class ChangelingTransformCycleEvent : InstantActionEvent { }
public sealed partial class ChangelingTransformEvent : InstantActionEvent { }
public sealed partial class EnterStasisEvent : InstantActionEvent { }
public sealed partial class ExitStasisEvent : InstantActionEvent { }

#endregion

#region Events - Combat

public sealed partial class ToggleArmbladeEvent : InstantActionEvent { }
public sealed partial class ToggleArmHammerEvent : InstantActionEvent { }
public sealed partial class ToggleArmClawEvent : InstantActionEvent { }
public sealed partial class ToggleDartGunEvent : InstantActionEvent { }
public sealed partial class CreateBoneShardEvent : InstantActionEvent { }
public sealed partial class ToggleChitinousArmorEvent : InstantActionEvent { }
public sealed partial class ToggleOrganicShieldEvent : InstantActionEvent { }
public sealed partial class ShriekDissonantEvent : InstantActionEvent { }
public sealed partial class ShriekResonantEvent : InstantActionEvent { }
public sealed partial class ToggleStrainedMusclesEvent : InstantActionEvent { }

#endregion

#region Events - Sting

public sealed partial class StingReagentEvent : EntityTargetActionEvent { }
public sealed partial class StingFakeArmbladeEvent : EntityTargetActionEvent { }
public sealed partial class StingTransformEvent : EntityTargetActionEvent { }
public sealed partial class StingLayEggsEvent : EntityTargetActionEvent { }

#endregion

#region Events - Utility

public sealed partial class ActionAnatomicPanaceaEvent : InstantActionEvent { }
public sealed partial class ActionBiodegradeEvent : InstantActionEvent { }
public sealed partial class ActionChameleonSkinEvent : InstantActionEvent { }
public sealed partial class ActionAdrenalineReservesEvent : InstantActionEvent { }
public sealed partial class ActionFleshmendEvent : InstantActionEvent { }
public sealed partial class ActionLastResortEvent : InstantActionEvent { }
public sealed partial class ActionLesserFormEvent : InstantActionEvent { }
public sealed partial class ActionVoidAdaptEvent : InstantActionEvent { }
public sealed partial class ActionHivemindAccessEvent : InstantActionEvent { }
public sealed partial class ActionContortBodyEvent : InstantActionEvent { }

#endregion

#region Events - Misc

[DataDefinition]
public sealed partial class AugmentedEyesightPurchasedEvent : EntityEventArgs;

#endregion
