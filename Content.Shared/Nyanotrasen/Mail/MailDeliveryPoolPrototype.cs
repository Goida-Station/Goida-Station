// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Mail;

/// <summary>
/// Generic random weighting dataset to use.
/// </summary>
[Prototype("mailDeliveryPool")]
public sealed class MailDeliveryPoolPrototype : IPrototype
{
    [IdDataFieldAttribute] public string ID { get; } = default!;

    /// <summary>
    /// Mail that can be sent to everyone.
    /// </summary>
    [DataField("everyone")]
    public Dictionary<string, float> Everyone = new();

    /// <summary>
    /// Mail that can be sent only to specific jobs.
    /// </summary>
    [DataField("jobs")]
    public Dictionary<string, Dictionary<string, float>> Jobs = new();

    /// <summary>
    /// Mail that can be sent only to specific departments.
    /// </summary>
    [DataField("departments")]
    public Dictionary<string, Dictionary<string, float>> Departments = new();
}