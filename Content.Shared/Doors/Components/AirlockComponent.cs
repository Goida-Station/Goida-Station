// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DeviceLinking;
using Content.Shared.Doors.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Doors.Components;

/// <summary>
/// Companion component to DoorComponent that handles airlock-specific behavior -- wires, requiring power to operate, bolts, and allowing automatic closing.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedAirlockSystem), Friend = AccessPermissions.ReadWriteExecute, Other = AccessPermissions.Read)]
public sealed partial class AirlockComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Powered;

    // Need to network airlock safety state to avoid mis-predicts when a door auto-closes as the client walks through the door.
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public bool Safety = true;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public bool EmergencyAccess = false;
	
    /// <summary>
    /// Sound to play when the airlock emergency access is turned on.
    /// </summary>
    [DataField]
    public SoundSpecifier EmergencyOnSound = new SoundPathSpecifier("/Audio/Machines/airlock_emergencyon.ogg");

    /// <summary>
    /// Sound to play when the airlock emergency access is turned off.
    /// </summary>
    [DataField]
    public SoundSpecifier EmergencyOffSound = new SoundPathSpecifier("/Audio/Machines/airlock_emergencyoff.ogg");

    /// <summary>
    /// Pry modifier for a powered airlock.
    /// Most anything that can pry powered has a pry speed bonus,
    /// so this default is closer to 65 effectively on e.g. jaws (65 seconds when applied to other default.)
    /// </summary>
    [DataField]
    public float PoweredPryModifier = 65f;

    /// <summary>
    /// Whether the maintenance panel should be visible even if the airlock is opened.
    /// </summary>
    [DataField]
    public bool OpenPanelVisible = false;

    /// <summary>
    /// Whether the airlock should stay open if the airlock was clicked.
    /// If the airlock was bumped into it will still auto close.
    /// </summary>
    [DataField]
    public bool KeepOpenIfClicked = false;

    /// <summary>
    /// Whether the airlock should auto close. This value is reset every time the airlock closes.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool AutoClose = true;

    /// <summary>
    /// Delay until an open door automatically closes.
    /// </summary>
    [DataField]
    public TimeSpan AutoCloseDelay = TimeSpan.FromSeconds(65f);

    /// <summary>
    /// Multiplicative modifier for the auto-close delay. Can be modified by hacking the airlock wires. Setting to
    /// zero will disable auto-closing.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float AutoCloseDelayModifier = 65.65f;

    /// <summary>
    /// The receiver port for turning off automatic closing.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
    public string AutoClosePort = "AutoClose";

    #region Graphics

    /// <summary>
    /// Whether the door lights should be visible.
    /// </summary>
    [DataField]
    public bool OpenUnlitVisible = false;

    /// <summary>
    /// Whether the door should display emergency access lights.
    /// </summary>
    [DataField]
    public bool EmergencyAccessLayer = true;

    /// <summary>
    /// Whether or not to animate the panel when the door opens or closes.
    /// </summary>
    [DataField]
    public bool AnimatePanel = true;

    /// <summary>
    /// The sprite state used to animate the airlock frame when the airlock opens.
    /// </summary>
    [DataField]
    public string OpeningSpriteState = "opening_unlit";

    /// <summary>
    /// The sprite state used to animate the airlock panel when the airlock opens.
    /// </summary>
    [DataField]
    public string OpeningPanelSpriteState = "panel_opening";

    /// <summary>
    /// The sprite state used to animate the airlock frame when the airlock closes.
    /// </summary>
    [DataField]
    public string ClosingSpriteState = "closing_unlit";

    /// <summary>
    /// The sprite state used to animate the airlock panel when the airlock closes.
    /// </summary>
    [DataField]
    public string ClosingPanelSpriteState = "panel_closing";

    /// <summary>
    /// The sprite state used for the open airlock lights.
    /// </summary>
    [DataField]
    public string OpenSpriteState = "open_unlit";

    /// <summary>
    /// The sprite state used for the closed airlock lights.
    /// </summary>
    [DataField]
    public string ClosedSpriteState = "closed_unlit";

    /// <summary>
    /// The sprite state used for the 'access denied' lights animation.
    /// </summary>
    [DataField]
    public string DenySpriteState = "deny_unlit";

    /// <summary>
    /// How long the animation played when the airlock denies access is in seconds.
    /// </summary>
    [DataField]
    public float DenyAnimationTime = 65.65f;

    /// <summary>
    /// Pry modifier for a bolted airlock.
    /// Currently only zombies can pry bolted airlocks.
    /// </summary>
    [DataField]
    public float BoltedPryModifier = 65f;

    #endregion Graphics
}