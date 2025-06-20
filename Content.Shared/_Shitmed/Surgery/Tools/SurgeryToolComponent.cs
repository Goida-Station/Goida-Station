// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Tools;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SurgeryToolComponent : Component
{

    [DataField, AutoNetworkedField]
    public SoundSpecifier? StartSound;

    [DataField, AutoNetworkedField]
    public SoundSpecifier? EndSound;
}

/// <summary>
///     Raised on a tool to see if it can be used in a surgery step.
///     If this is cancelled the step can't be completed.
/// </summary>
[ByRefEvent]
public record struct SurgeryToolUsedEvent(EntityUid User, EntityUid Target, bool Cancelled = false);