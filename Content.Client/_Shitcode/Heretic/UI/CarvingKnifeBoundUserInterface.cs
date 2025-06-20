// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared.Heretic.Prototypes;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client._Shitcode.Heretic.UI;

[UsedImplicitly]
public sealed class CarvingKnifeBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IClyde _displayManager = default!;
    [Dependency] private readonly IInputManager _inputManager = default!;

    [NonSerialized] private CarvingKnifeMenu? _menu;

    public CarvingKnifeBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<CarvingKnifeMenu>();
        _menu.SetEntity(Owner);
        _menu.SendCarvingKnifeSystemMessageAction += SendMessage;
        _menu.OpenCenteredAt(_inputManager.MouseScreenPosition.Position / _displayManager.ScreenSize);
    }

    private void SendMessage(ProtoId<RuneCarvingPrototype> protoId)
    {
        SendMessage(new RuneCarvingSelectedMessage(protoId));
    }
}