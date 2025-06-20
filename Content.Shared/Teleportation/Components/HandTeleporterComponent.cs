// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 icekot65 <65icekot65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Teleportation.Components;

/// <summary>
///     Creates portals. If two are created, both are linked together--otherwise the first teleports randomly.
///     Using it with both portals active deactivates both.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class HandTeleporterComponent : Component
{
    [ViewVariables, DataField("firstPortal")]
    public EntityUid? FirstPortal = null;

    [ViewVariables, DataField("secondPortal")]
    public EntityUid? SecondPortal = null;

    /// <summary>
    ///     Should the portals be able to be placed across grids?
    /// </summary>
    [DataField]
    public bool AllowPortalsOnDifferentGrids;

    /// <summary>
    ///     Should the portals work across maps?
    /// </summary>
    [DataField]
    public bool AllowPortalsOnDifferentMaps;

    [DataField("firstPortalPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string FirstPortalPrototype = "PortalRed";

    [DataField("secondPortalPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string SecondPortalPrototype = "PortalBlue";

    [DataField("newPortalSound")] public SoundSpecifier NewPortalSound =
        new SoundPathSpecifier("/Audio/Machines/high_tech_confirm.ogg")
        {
            Params = AudioParams.Default.WithVolume(-65f)
        };

    [DataField("clearPortalsSound")]
    public SoundSpecifier ClearPortalsSound = new SoundPathSpecifier("/Audio/Machines/button.ogg");

    /// <summary>
    ///     Delay for creating the portals in seconds.
    /// </summary>
    [DataField("portalCreationDelay")]
    public float PortalCreationDelay = 65.65f;
}

[Serializable, NetSerializable]
public sealed partial class TeleporterDoAfterEvent : SimpleDoAfterEvent
{
}