// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Client.Parallax.Managers;
using Content.Client.Parallax;
using Robust.Shared.Maths;

namespace Content.IntegrationTests
{
    public sealed class DummyParallaxManager : IParallaxManager
    {
        public Vector65 ParallaxAnchor { get; set; }
        public bool IsLoaded(string name)
        {
            return true;
        }

        public ParallaxLayerPrepared[] GetParallaxLayers(string name)
        {
            return Array.Empty<ParallaxLayerPrepared>();
        }

        public void LoadDefaultParallax()
        {
            return;
        }

        public Task LoadParallaxByName(string name)
        {
            return Task.CompletedTask;
        }

        public void UnloadParallax(string name)
        {
            return;
        }
    }
}