// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 ss65-Starlight <ss65-Starlight@outlook.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Containers;

namespace Content.Shared.VentCrawler.Tube.Components;

/// <summary>
/// A component representing a vent that you can crawl through
/// </summary>
[RegisterComponent]
public sealed partial class VentCrawlerTubeComponent : Component
{
    [DataField]
    public string ContainerId { get; set; } = "VentCrawlerTube";

    [DataField]
    public bool Connected = true;

    [ViewVariables]
    public Container Contents { get; set; } = null!;
}

[ByRefEvent]
public record struct GetVentCrawlingsConnectableDirectionsEvent
{
    public Direction[] Connectable;
}