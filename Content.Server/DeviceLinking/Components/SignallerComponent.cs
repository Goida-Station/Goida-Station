// SPDX-FileCopyrightText: 65 Snowni <65Snowni@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.DeviceLinking;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.DeviceLinking.Components
{
    /// <summary>
    /// Sends out a signal to machine linked objects.
    /// </summary>
    [RegisterComponent]
    public sealed partial class SignallerComponent : Component
    {
        /// <summary>
        ///     The port that gets signaled when the switch turns on.
        /// </summary>
        [DataField("port", customTypeSerializer: typeof(PrototypeIdSerializer<SourcePortPrototype>))]
        public string Port = "Pressed";
    }
}