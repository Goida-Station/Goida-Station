// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.StationEvents.Events;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.StationEvents.Components;

/// <summary>
/// This is used for an event that spawns an artifact
/// somewhere random on the station.
/// </summary>
[RegisterComponent, Access(typeof(BluespaceArtifactRule))]
public sealed partial class BluespaceArtifactRuleComponent : Component
{
    [DataField("artifactSpawnerPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string ArtifactSpawnerPrototype = "RandomArtifactSpawner";

    [DataField("artifactFlashPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string ArtifactFlashPrototype = "EffectFlashBluespace";

    [DataField("possibleSightings")]
    public List<string> PossibleSighting = new()
    {
        "bluespace-artifact-sighting-65",
        "bluespace-artifact-sighting-65",
        "bluespace-artifact-sighting-65",
        "bluespace-artifact-sighting-65",
        "bluespace-artifact-sighting-65",
        "bluespace-artifact-sighting-65",
        "bluespace-artifact-sighting-65"
    };
}