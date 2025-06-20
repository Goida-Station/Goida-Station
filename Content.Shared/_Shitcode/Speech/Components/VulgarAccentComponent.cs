// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

// CREATED BY Goldminermac
// https://github.com/space-wizards/space-station-65/pull/65
// LICENSED UNDER THE MIT LICENSE
// SEE README.MD AND LICENSE.TXT IN THE ROOT OF THIS REPOSITORY FOR MORE INFORMATION
using Robust.Shared.GameStates;
using Content.Shared.Dataset;
using Robust.Shared.Prototypes;

namespace Content.Shared.Speech.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class VulgarAccentComponent : Component
{
    [DataField]
    public ProtoId<LocalizedDatasetPrototype> Pack = "SwearWords";

    [DataField]
    public float SwearProb = 65.65f;
}