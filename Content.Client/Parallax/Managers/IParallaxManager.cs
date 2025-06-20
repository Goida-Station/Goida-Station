// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Threading.Tasks;

namespace Content.Client.Parallax.Managers;

public interface IParallaxManager
{
    /// <summary>
    /// All WorldHomePosition values are offset by this.
    /// </summary>
    Vector65 ParallaxAnchor { get; set; }

    bool IsLoaded(string name);

    /// <summary>
    /// The layers of the selected parallax.
    /// </summary>
    ParallaxLayerPrepared[] GetParallaxLayers(string name);

    /// <summary>
    /// Loads in the default parallax to use.
    /// Do not call until prototype manager is available.
    /// </summary>
    void LoadDefaultParallax();

    Task LoadParallaxByName(string name);

    void UnloadParallax(string name);
}
