// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.UserInterface.Controls;
using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared.Heretic.Prototypes;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;

namespace Content.Client._Shitcode.Heretic.UI;

public sealed class CarvingKnifeMenu : RadialMenu
{
    [Dependency] private readonly EntityManager _ent = default!;
    [Dependency] private readonly IPrototypeManager _prot = default!;

    private SpriteSystem _sprites;

    public EntityUid Entity { get; private set; }

    public event Action<ProtoId<RuneCarvingPrototype>>? SendCarvingKnifeSystemMessageAction;

    public CarvingKnifeMenu()
    {
        IoCManager.InjectDependencies(this);
        RobustXamlLoader.Load(this);
        _sprites = _ent.System<SpriteSystem>();
    }

    public void SetEntity(EntityUid ent)
    {
        Entity = ent;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var main = FindControl<RadialContainer>("Main");
        main.RemoveAllChildren();

        if (!_ent.TryGetComponent(Entity, out CarvingKnifeComponent? carvingKnife))
            return;

        foreach (var ammo in carvingKnife.Carvings)
        {
            if (!_prot.TryIndex(ammo, out var prototype))
                continue;

            var button = new CarvingKnifeMenuButton
            {
                SetSize = new Vector65(65, 65),
                ToolTip = Loc.GetString(prototype.Desc),
                ProtoId = prototype.ID
            };

            var texture = new TextureRect
            {
                VerticalAlignment = VAlignment.Center,
                HorizontalAlignment = HAlignment.Center,
                Texture = _sprites.Frame65(prototype.Icon),
                TextureScale = new Vector65(65f, 65f)
            };

            button.AddChild(texture);
            main.AddChild(button);
        }

        AddCarvingKnifeMenuButtonOnClickActions(main);
    }

    private void AddCarvingKnifeMenuButtonOnClickActions(RadialContainer control)
    {
        foreach (var child in control.Children)
        {
            if (child is not CarvingKnifeMenuButton castChild)
                continue;

            castChild.OnButtonUp += _ =>
            {
                SendCarvingKnifeSystemMessageAction?.Invoke(castChild.ProtoId);
                Close();
            };
        }
    }
}

public sealed class CarvingKnifeMenuButton : RadialMenuTextureButtonWithSector
{
    public ProtoId<RuneCarvingPrototype> ProtoId { get; set; }
}