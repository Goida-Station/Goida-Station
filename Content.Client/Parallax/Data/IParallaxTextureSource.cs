// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using System.Threading.Tasks;
using Robust.Client.Graphics;

namespace Content.Client.Parallax.Data
{
    [ImplicitDataDefinitionForInheritors]
    public partial interface IParallaxTextureSource
    {
        /// <summary>
        /// Generates or loads the texture.
        /// Note that this should be cached, but not necessarily *here*.
        /// </summary>
        Task<Texture> GenerateTexture(CancellationToken cancel = default);
    }
}
