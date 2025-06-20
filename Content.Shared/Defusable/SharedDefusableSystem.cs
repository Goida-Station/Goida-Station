// SPDX-FileCopyrightText: 65 Just-a-Unity-Dev <just-a-unity-dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LankLTE <twlowe65@gmail.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Defusable;

/// <summary>
/// This handles defusable explosives, such as Syndicate Bombs.
/// </summary>
/// <remarks>
/// Most of the logic is in the server
/// </remarks>
public abstract class SharedDefusableSystem : EntitySystem
{

}

[NetSerializable, Serializable]
public enum DefusableVisuals
{
    Active
}

[NetSerializable, Serializable]
public enum DefusableWireStatus
{
    LiveIndicator,
    BoltIndicator,
    BoomIndicator,
    DelayIndicator,
    ProceedIndicator,
}