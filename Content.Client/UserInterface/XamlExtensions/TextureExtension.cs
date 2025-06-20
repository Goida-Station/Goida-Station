// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Resources;
using JetBrains.Annotations;
using Robust.Client.ResourceManagement;

namespace Content.Client.UserInterface.XamlExtensions;


[PublicAPI]
public sealed class TexExtension
{
    private IResourceCache _resourceCache;
    public string Path { get; }

    public TexExtension(string path)
    {
        _resourceCache = IoCManager.Resolve<IResourceCache>();
        Path = path;
    }

    public object ProvideValue()
    {
        return _resourceCache.GetTexture(Path);
    }
}