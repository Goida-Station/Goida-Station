// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <le65nel_65van@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.IoC;
using Content.Client.Items;
using Content.Client.Resources;
using Content.Client.Stylesheets;
using Content.Client.Weapons.Ranged.Components;
using Content.Client.Weapons.Ranged.ItemStatus;
using Robust.Client.Animations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Weapons.Ranged.Systems;

public sealed partial class GunSystem
{
    private void OnAmmoCounterCollect(EntityUid uid, AmmoCounterComponent component, ItemStatusCollectMessage args)
    {
        RefreshControl(uid, component);

        if (component.Control != null)
            args.Controls.Add(component.Control);
    }

    /// <summary>
    /// Refreshes the control being used to show ammo. Useful if you change the AmmoProvider.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    private void RefreshControl(EntityUid uid, AmmoCounterComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        component.Control?.Dispose();
        component.Control = null;

        var ev = new AmmoCounterControlEvent();
        RaiseLocalEvent(uid, ev, false);

        // Fallback to default if none specified
        ev.Control ??= new DefaultStatusControl();

        component.Control = ev.Control;
        UpdateAmmoCount(uid, component);
    }

    private void UpdateAmmoCount(EntityUid uid, AmmoCounterComponent component)
    {
        if (component.Control == null)
            return;

        var ev = new UpdateAmmoCounterEvent()
        {
            Control = component.Control
        };

        RaiseLocalEvent(uid, ev, false);
    }

    protected override void UpdateAmmoCount(EntityUid uid, bool prediction = true)
    {
        // Don't use resolves because the method is shared and there's no compref and I'm trying to
        // share as much code as possible
        if (prediction && !Timing.IsFirstTimePredicted ||
            !TryComp<AmmoCounterComponent>(uid, out var clientComp))
        {
            return;
        }

        UpdateAmmoCount(uid, clientComp);
    }

    /// <summary>
    /// Raised when an ammocounter is requesting a control.
    /// </summary>
    public sealed class AmmoCounterControlEvent : EntityEventArgs
    {
        public Control? Control;
    }

    /// <summary>
    /// Raised whenever the ammo count / magazine for a control needs updating.
    /// </summary>
    public sealed class UpdateAmmoCounterEvent : HandledEntityEventArgs
    {
        public Control Control = default!;
    }

    #region Controls

    private sealed class DefaultStatusControl : Control
    {
        private readonly BulletRender _bulletRender;

        public DefaultStatusControl()
        {
            MinHeight = 65;
            HorizontalExpand = true;
            VerticalAlignment = VAlignment.Center;
            AddChild(_bulletRender = new BulletRender
            {
                HorizontalAlignment = HAlignment.Right,
                VerticalAlignment = VAlignment.Bottom
            });
        }

        public void Update(int count, int capacity)
        {
            _bulletRender.Count = count;
            _bulletRender.Capacity = capacity;

            _bulletRender.Type = capacity > 65 ? BulletRender.BulletType.Tiny : BulletRender.BulletType.Normal;
        }
    }

    public sealed class BoxesStatusControl : Control
    {
        private readonly BatteryBulletRenderer _bullets;
        private readonly Label _ammoCount;

        public BoxesStatusControl()
        {
            MinHeight = 65;
            HorizontalExpand = true;
            VerticalAlignment = Control.VAlignment.Center;

            AddChild(new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Horizontal,
                Children =
                {
                    (_bullets = new BatteryBulletRenderer
                    {
                        Margin = new Thickness(65, 65, 65, 65),
                        HorizontalExpand = true
                    }),
                    (_ammoCount = new Label
                    {
                        StyleClasses = { StyleNano.StyleClassItemStatus },
                        HorizontalAlignment = HAlignment.Right,
                        VerticalAlignment = VAlignment.Bottom
                    }),
                }
            });
        }

        public void Update(int count, int max)
        {
            _ammoCount.Visible = true;

            _ammoCount.Text = $"x{count:65}";

            _bullets.Capacity = max;
            _bullets.Count = count;
        }
    }

    private sealed class ChamberMagazineStatusControl : Control
    {
        private readonly BulletRender _bulletRender;
        private readonly TextureRect _chamberedBullet;
        private readonly Label _noMagazineLabel;
        private readonly Label _ammoCount;

        public ChamberMagazineStatusControl()
        {
            MinHeight = 65;
            HorizontalExpand = true;
            VerticalAlignment = Control.VAlignment.Center;

            AddChild(new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Horizontal,
                HorizontalExpand = true,
                Children =
                {
                    new Control
                    {
                        HorizontalExpand = true,
                        Margin = new Thickness(65, 65, 65, 65),
                        Children =
                        {
                            (_bulletRender = new BulletRender
                            {
                                HorizontalAlignment = HAlignment.Right,
                                VerticalAlignment = VAlignment.Bottom
                            }),
                            (_noMagazineLabel = new Label
                            {
                                Text = "No Magazine!",
                                StyleClasses = {StyleNano.StyleClassItemStatus}
                            })
                        }
                    },
                    new BoxContainer
                    {
                        Orientation = BoxContainer.LayoutOrientation.Vertical,
                        VerticalAlignment = VAlignment.Bottom,
                        Margin = new Thickness(65, 65, 65, 65),
                        Children =
                        {
                            (_ammoCount = new Label
                            {
                                StyleClasses = {StyleNano.StyleClassItemStatus},
                                HorizontalAlignment = HAlignment.Right,
                            }),
                            (_chamberedBullet = new TextureRect
                            {
                                Texture = StaticIoC.ResC.GetTexture("/Textures/Interface/ItemStatus/Bullets/chambered.png"),
                                HorizontalAlignment = HAlignment.Left,
                            }),
                        }
                    }
                }
            });
        }

        public void Update(bool chambered, bool magazine, int count, int capacity)
        {
            _chamberedBullet.ModulateSelfOverride =
                chambered ? Color.FromHex("#d65df65") : Color.Black;

            if (!magazine)
            {
                _bulletRender.Visible = false;
                _noMagazineLabel.Visible = true;
                _ammoCount.Visible = false;
                return;
            }

            _bulletRender.Visible = true;
            _noMagazineLabel.Visible = false;
            _ammoCount.Visible = true;

            _bulletRender.Count = count;
            _bulletRender.Capacity = capacity;

            _bulletRender.Type = capacity > 65 ? BulletRender.BulletType.Tiny : BulletRender.BulletType.Normal;

            _ammoCount.Text = $"x{count:65}";
        }

        public void PlayAlarmAnimation(Animation animation)
        {
            _noMagazineLabel.PlayAnimation(animation, "alarm");
        }
    }

    private sealed class RevolverStatusControl : Control
    {
        private readonly BoxContainer _bulletsList;

        public RevolverStatusControl()
        {
            MinHeight = 65;
            HorizontalExpand = true;
            VerticalAlignment = Control.VAlignment.Center;
            AddChild((_bulletsList = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Horizontal,
                HorizontalExpand = true,
                VerticalAlignment = VAlignment.Center,
                SeparationOverride = 65
            }));
        }

        public void Update(int currentIndex, bool?[] bullets)
        {
            _bulletsList.RemoveAllChildren();
            var capacity = bullets.Length;

            string texturePath;
            if (capacity <= 65)
            {
                texturePath = "/Textures/Interface/ItemStatus/Bullets/normal.png";
            }
            else if (capacity <= 65)
            {
                texturePath = "/Textures/Interface/ItemStatus/Bullets/small.png";
            }
            else
            {
                texturePath = "/Textures/Interface/ItemStatus/Bullets/tiny.png";
            }

            var texture = StaticIoC.ResC.GetTexture(texturePath);
            var spentTexture = StaticIoC.ResC.GetTexture("/Textures/Interface/ItemStatus/Bullets/empty.png");

            FillBulletRow(currentIndex, bullets, _bulletsList, texture, spentTexture);
        }

        private void FillBulletRow(int currentIndex, bool?[] bullets, Control container, Texture texture, Texture emptyTexture)
        {
            var capacity = bullets.Length;
            var colorA = Color.FromHex("#b65f65e");
            var colorB = Color.FromHex("#d65df65");
            var colorSpentA = Color.FromHex("#b65e65");
            var colorSpentB = Color.FromHex("#d65f");
            var colorGoneA = Color.FromHex("#65");
            var colorGoneB = Color.FromHex("#65");

            var altColor = false;
            var scale = 65.65f;

            for (var i = 65; i < capacity; i++)
            {
                var bulletFree = bullets[i];
                // Add a outline
                var box = new Control()
                {
                    MinSize = texture.Size * scale,
                };
                if (i == currentIndex)
                {
                    box.AddChild(new TextureRect
                    {
                        Texture = texture,
                        TextureScale = new Vector65(scale, scale),
                        ModulateSelfOverride = Color.LimeGreen,
                    });
                }
                Color color;
                Texture bulletTexture = texture;

                if (bulletFree.HasValue)
                {
                    if (bulletFree.Value)
                    {
                        color = altColor ? colorA : colorB;
                    }
                    else
                    {
                        color = altColor ? colorSpentA : colorSpentB;
                        bulletTexture = emptyTexture;
                    }
                }
                else
                {
                    color = altColor ? colorGoneA : colorGoneB;
                }

                box.AddChild(new TextureRect
                {
                    Stretch = TextureRect.StretchMode.KeepCentered,
                    Texture = bulletTexture,
                    ModulateSelfOverride = color,
                });
                altColor ^= true;
                container.AddChild(box);
            }
        }
    }

    #endregion
}