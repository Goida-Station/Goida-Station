// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Payload.Components;

/// <summary>
///     Component that enables payloads and payload triggers to function.
/// </summary>
/// <remarks>
///     If an entity with a <see cref="PayloadTriggerComponent"/> is installed into a an entity with a <see
///     cref="PayloadCaseComponent"/>, the trigger will grant components to the case-entity. If the case entity is
///     triggered, it will forward the trigger onto any contained payload entity.
/// </remarks>
[RegisterComponent]
public sealed partial class PayloadCaseComponent : Component { }