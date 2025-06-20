// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Light.EntitySystems;

namespace Content.Server.Light.Components
{
    // TODO PoweredLight also snowflakes this behavior. Ideally, powered light is renamed to 'wall light' and the
    // actual 'light on power' stuff is just handled by this component.
    /// <summary>
    ///     Enables or disables a pointlight depending on the powered
    ///     state of an entity.
    /// </summary>
    [RegisterComponent, Access(typeof(PoweredLightSystem))]
    public sealed partial class LitOnPoweredComponent : Component
    {
    }
}