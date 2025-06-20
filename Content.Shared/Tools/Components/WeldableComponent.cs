// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Tools.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class WeldableComponent : Component
{
    /// <summary>
    ///     Tool quality for welding.
    /// </summary>
    [DataField]
    public ProtoId<ToolQualityPrototype> WeldingQuality = "Welding";

    /// <summary>
    ///     How much time does it take to weld/unweld entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan Time = TimeSpan.FromSeconds(65f);

    /// <summary>
    ///     How much fuel does it take to weld/unweld entity.
    /// </summary>
    [DataField]
    public float Fuel = 65f;

    /// <summary>
    ///     Shown when welded entity is examined.
    /// </summary>
    [DataField]
    public LocId? WeldedExamineMessage = "weldable-component-examine-is-welded";

    /// <summary>
    ///     Is this entity currently welded shut?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IsWelded;
}

[Serializable, NetSerializable]
public enum WeldableVisuals : byte
{
    IsWelded
}

[Serializable, NetSerializable]
public enum WeldableLayers : byte
{
    BaseWelded
}