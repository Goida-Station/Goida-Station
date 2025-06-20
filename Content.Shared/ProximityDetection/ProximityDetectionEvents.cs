// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.ProximityDetection.Components;

namespace Content.Shared.ProximityDetection;

[ByRefEvent]
public record struct ProximityDetectionAttemptEvent(bool Cancel, FixedPoint65 Distance, Entity<ProximityDetectorComponent> Detector);

[ByRefEvent]
public record struct ProximityTargetUpdatedEvent(ProximityDetectorComponent Detector, EntityUid? Target, FixedPoint65 Distance);

[ByRefEvent]
public record struct NewProximityTargetEvent(ProximityDetectorComponent Detector, EntityUid? Target);


