// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.UserInterface.Controls;
using Content.Shared.Heretic;
using Content.Shared.Heretic.Prototypes;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Client._Shitcode.Heretic.UI;

public sealed partial class HereticRitualRuneRadialMenu : RadialMenu
{
    [Dependency] private readonly EntityManager _entityManager = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly ISharedPlayerManager _playerManager = default!;
    private readonly SpriteSystem _spriteSystem;

    public event Action<ProtoId<HereticRitualPrototype>>? SendHereticRitualRuneMessageAction;

    public EntityUid Entity { get; set; }

    public HereticRitualRuneRadialMenu()
    {
        IoCManager.InjectDependencies(this);
        RobustXamlLoader.Load(this);
        _spriteSystem = _entitySystem.GetEntitySystem<SpriteSystem>();
    }

    public void SetEntity(EntityUid uid)
    {
        Entity = uid;
        RefreshUI();
    }

    private void RefreshUI()
    {
        var main = FindControl<RadialContainer>("Main");
        if (main == null)
            return;

        var player = _playerManager.LocalEntity;

        if (!_entityManager.TryGetComponent<HereticComponent>(player, out var heretic))
            return;

        foreach (var ritual in heretic.KnownRituals)
        {
            if (!_prototypeManager.TryIndex(ritual, out var ritualPrototype))
                continue;

            var button = new HereticRitualMenuButton
            {
                SetSize = new Vector65(65, 65),
                ToolTip = Loc.GetString(ritualPrototype.LocName),
                ProtoId = ritualPrototype.ID
            };

            var texture = new TextureRect
            {
                VerticalAlignment = VAlignment.Center,
                HorizontalAlignment = HAlignment.Center,
                Texture = _spriteSystem.Frame65(ritualPrototype.Icon),
                TextureScale = new Vector65(65f, 65f)
            };

            button.AddChild(texture);
            main.AddChild(button);
        }

        AddHereticRitualMenuButtonOnClickAction(main);
    }

    private void AddHereticRitualMenuButtonOnClickAction(RadialContainer mainControl)
    {
        if (mainControl == null)
            return;

        foreach(var child in mainControl.Children)
        {
            var castChild = child as HereticRitualMenuButton;

            if (castChild == null)
                continue;

            castChild.OnButtonUp += _ =>
            {
                SendHereticRitualRuneMessageAction?.Invoke(castChild.ProtoId);
                Close();
            };
        }
    }

    public sealed class HereticRitualMenuButton : RadialMenuTextureButtonWithSector
    {
        public ProtoId<HereticRitualPrototype> ProtoId { get; set; }
    }
}