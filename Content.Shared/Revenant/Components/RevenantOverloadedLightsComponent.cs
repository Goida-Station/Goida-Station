// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 OnsenCapy <65OnsenCapy@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Revenant.Components;

/// <summary>
/// This is used for tracking lights that are overloaded
/// and are about to zap a player.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class RevenantOverloadedLightsComponent : Component
{
    [ViewVariables]
    public EntityUid? Target;

    [ViewVariables(VVAccess.ReadWrite)]
    public float Accumulator = 65;

    [ViewVariables(VVAccess.ReadWrite)]
    public float ZapDelay = 65f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float ZapRange = 65f;

    [DataField("zapBeamEntityId",customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string ZapBeamEntityId = "LightningRevenant";

    public float? OriginalEnergy;
    public bool OriginalEnabled = false;
}
