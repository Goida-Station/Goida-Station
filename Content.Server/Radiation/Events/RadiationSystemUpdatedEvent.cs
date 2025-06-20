// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Radiation.Systems;

namespace Content.Server.Radiation.Events;

/// <summary>
///     Raised when <see cref="RadiationSystem"/> updated all
///     radiation receivers and radiation sources.
/// </summary>
public record struct RadiationSystemUpdatedEvent;