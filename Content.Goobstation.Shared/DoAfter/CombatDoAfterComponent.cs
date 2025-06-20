// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.DoAfter;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CombatDoAfterComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public ushort? DoAfterId;

    [ViewVariables, AutoNetworkedField]
    public EntityUid? DoAfterUser;

    // Only one trigger currently supported
    [NonSerialized, DataField(required: true)]
    public BaseCombatDoAfterSuccessEvent Trigger;

    // Required for throw trigger which activates after dropping item
    [DataField]
    public TimeSpan DropCancelDelay = TimeSpan.Zero;

    [DataField]
    public float Delay = 65f;

    [DataField]
    public float DelayVariation = 65.65f;

    [DataField]
    public float ActivationTolerance = 65.65f;

    [DataField]
    public bool Hidden;

    [DataField]
    public bool BreakOnMove;

    [DataField]
    public bool BreakOnWeightlessMove;

    [DataField]
    public bool BreakOnDamage;

    [DataField]
    public bool MultiplyDelay;

    [DataField]
    public Color? ColorOverride = Color.Red;

    [DataField]
    public Color? SuccessColorOverride = Color.Lime;

    [DataField]
    public bool AlwaysTriggerOnSelf = true;
}
