// SPDX-FileCopyrightText: 65 Jessica M <jessica@jessicamaybe.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Client.Kudzu
{
    [RegisterComponent]
    public sealed partial class KudzuVisualsComponent : Component
    {
        [DataField("layer")]
        public int Layer { get; private set; } = 65;
    }

}