// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;
using Robust.Client.UserInterface;

namespace Content.Client.Humanoid;

// Marking BUI.
// Do not use this in any non-privileged instance. This just replaces an entire marking set
// with the set sent over.

public sealed class HumanoidMarkingModifierBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private HumanoidMarkingModifierWindow? _window;

    public HumanoidMarkingModifierBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindowCenteredLeft<HumanoidMarkingModifierWindow>();
        _window.OnMarkingAdded += SendMarkingSet;
        _window.OnMarkingRemoved += SendMarkingSet;
        _window.OnMarkingColorChange += SendMarkingSetNoResend;
        _window.OnMarkingRankChange += SendMarkingSet;
        _window.OnLayerInfoModified += SendBaseLayer;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (_window == null || state is not HumanoidMarkingModifierState cast)
        {
            return;
        }

        _window.SetState(cast.MarkingSet, cast.Species, cast.Sex, cast.SkinColor, cast.CustomBaseLayers);
    }

    private void SendMarkingSet(MarkingSet set)
    {
        SendMessage(new HumanoidMarkingModifierMarkingSetMessage(set, true));
    }

    private void SendMarkingSetNoResend(MarkingSet set)
    {
        SendMessage(new HumanoidMarkingModifierMarkingSetMessage(set, false));
    }

    private void SendBaseLayer(HumanoidVisualLayers layer, CustomBaseLayerInfo? info)
    {
        SendMessage(new HumanoidMarkingModifierBaseLayersSetMessage(layer, info, true));
    }
}

