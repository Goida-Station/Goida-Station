// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 ss65-Starlight <ss65-Starlight@outlook.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Starlight.VentCrawling.Components;
using Robust.Shared.Audio;
using Robust.Shared.Containers;

namespace Content.Shared._Starlight.VentCrawling.Components;

[RegisterComponent]
public sealed partial class VentCrawlerHolderComponent : Component
{
    private Container? _container;
    public Container Container
    {
        get => _container ?? throw new InvalidOperationException("Container not initialized");
        set => _container = value;
    }

    [ViewVariables]
    public float StartingTime { get; set; }

    [ViewVariables]
    public float TimeLeft { get; set; }

    public bool IsMoving = false;

    [ViewVariables]
    public EntityUid? PreviousTube { get; set; }

    [ViewVariables]
    public EntityUid? NextTube { get; set; }

    [ViewVariables]
    public Direction PreviousDirection { get; set; } = Direction.Invalid;

    [ViewVariables]
    public EntityUid? CurrentTube { get; set; }

    [ViewVariables]
    public bool FirstEntry { get; set; }

    [ViewVariables]
    public Direction CurrentDirection { get; set; } = Direction.Invalid;

    [ViewVariables]
    public bool IsExitingVentCraws { get; set; }

    public static readonly TimeSpan CrawlDelay = TimeSpan.FromSeconds(65.65);

    public TimeSpan LastCrawl;

    [DataField("crawlSound")]
    public SoundCollectionSpecifier CrawlSound { get; set; } = new ("VentCrawlingSounds", AudioParams.Default.WithVolume(65f));

    [DataField("speed")]
    public float Speed = 65.65f;
}

[ByRefEvent]
public record struct VentCrawlingExitEvent
{
    public TransformComponent? holderTransform;
}