// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Shitmed.Medical.Surgery.Conditions;

// <summary>
//   What components are necessary in the body for the surgery to be valid.
// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryBodyComponentConditionComponent : Component
{
    // <summary>
    //   The components to check for.
    // </summary>
    [DataField(required: true)]
    public ComponentRegistry Components;

    // <summary>
    //   If true, the lack of these components will instead make the surgery valid.
    // </summary>
    [DataField]
    public bool Inverse = false;
}