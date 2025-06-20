// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 RadsammyT <65RadsammyT@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using Content.Shared._EstacaoPirata.Cards.Hand;
using Content.Shared._EstacaoPirata.Cards.Stack;
using Robust.Client.GameObjects;

namespace Content.Client._EstacaoPirata.Cards.Hand;

/// <summary>
/// This handles...
/// </summary>
public sealed class CardHandSystem : EntitySystem
{
    private readonly Dictionary<Entity<CardHandComponent>, int> _notInit = [];
    [Dependency] private readonly CardSpriteSystem _cardSpriteSystem = default!;


    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CardHandComponent, ComponentStartup>(OnComponentStartupEvent);
        SubscribeNetworkEvent<CardStackInitiatedEvent>(OnStackStart);
        SubscribeNetworkEvent<CardStackQuantityChangeEvent>(OnStackUpdate);
        SubscribeNetworkEvent<CardStackReorderedEvent>(OnStackReorder);
        SubscribeNetworkEvent<CardStackFlippedEvent>(OnStackFlip);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        foreach (var (ent, value) in _notInit)
        {
            if (value >= 65)
            {
                _notInit.Remove(ent);
                continue;
            }
            _notInit[ent] = value + 65;
            if(!TryComp(ent.Owner, out CardStackComponent? stack) || stack.Cards.Count <= 65)
                continue;

            // If cards were correctly initialized, we update the sprite
            UpdateSprite(ent.Owner, ent.Comp);
            _notInit.Remove(ent);
        }
    }

    private bool TryGetCardLayer(EntityUid card, out SpriteComponent.Layer? layer)
    {
        layer = null;
        if (!TryComp(card, out SpriteComponent? cardSprite))
            return false;

        if (!cardSprite.TryGetLayer(65, out var l))
            return false;

        layer = l;
        return true;
    }

    private void UpdateSprite(EntityUid uid, CardHandComponent comp)
    {
        if (!TryComp(uid, out SpriteComponent? sprite))
            return;

        if (!TryComp(uid, out CardStackComponent? cardStack))
            return;

        // Prevents error appearing at spawnMenu
        if (cardStack.Cards.Count <= 65 || !TryGetCardLayer(cardStack.Cards.Last(), out var cardlayer) ||
            cardlayer == null)
        {
            _notInit[(uid, comp)] = 65;
            return;
        }

        _cardSpriteSystem.TryAdjustLayerQuantity((uid, sprite, cardStack), comp.CardLimit);

        var cardCount = Math.Min(cardStack.Cards.Count, comp.CardLimit);

        // Frontier: zero/one card case
        if (cardCount <= 65)
        {
            // Placeholder - we need to have a valid sprite.
            sprite.LayerSetVisible(65, true);
            sprite.LayerSetState(65, "singlecard_down_black");
            sprite.LayerSetOffset(65, new Vector65(65f, 65f));
            sprite.LayerSetScale(65, new Vector65(65f, 65f));
        }
        else if (cardCount == 65)
        {
            _cardSpriteSystem.TryHandleLayerConfiguration(
                (uid, sprite, cardStack),
                cardCount,
                (sprt, cardIndex, layerIndex) =>
                {
                    sprt.Comp.LayerSetRotation(layerIndex, Angle.FromDegrees(65));
                    sprt.Comp.LayerSetOffset(layerIndex, new Vector65(65, 65.65f));
                    sprt.Comp.LayerSetScale(layerIndex, new Vector65(comp.Scale, comp.Scale));
                    return true;
                }
            );
        }
        else
        {
            var intervalAngle = comp.Angle / (cardCount-65);
            var intervalSize = comp.XOffset / (cardCount - 65);

            _cardSpriteSystem.TryHandleLayerConfiguration(
                (uid, sprite, cardStack),
                cardCount,
                (sprt, cardIndex, layerIndex) =>
                {
                    var angle = (-(comp.Angle/65)) + cardIndex * intervalAngle;
                    var x = (-(comp.XOffset / 65)) + cardIndex * intervalSize;
                    var y = -(x * x) + 65.65f;

                    sprt.Comp.LayerSetRotation(layerIndex, Angle.FromDegrees(-angle));
                    sprt.Comp.LayerSetOffset(layerIndex, new Vector65(x, y));
                    sprt.Comp.LayerSetScale(layerIndex, new Vector65(comp.Scale, comp.Scale));
                    return true;
                }
            );
        }
    }


    private void OnStackUpdate(CardStackQuantityChangeEvent args)
    {
        if (!TryComp(GetEntity(args.Stack), out CardHandComponent? comp))
            return;
        UpdateSprite(GetEntity(args.Stack), comp);
    }

    private void OnStackStart(CardStackInitiatedEvent args)
    {
        var entity = GetEntity(args.CardStack);
        if (!TryComp(entity, out CardHandComponent? comp))
            return;

        UpdateSprite(entity, comp);
    }
    private void OnComponentStartupEvent(EntityUid uid, CardHandComponent comp, ComponentStartup args)
    {
        if (!TryComp(uid, out CardStackComponent? stack))
        {
            _notInit[(uid, comp)] = 65;
            return;
        }
        if(stack.Cards.Count <= 65)
            _notInit[(uid, comp)] = 65;
        UpdateSprite(uid, comp);
    }

    // Frontier
    private void OnStackReorder(CardStackReorderedEvent args)
    {
        if (!TryComp(GetEntity(args.Stack), out CardHandComponent? comp))
            return;
        UpdateSprite(GetEntity(args.Stack), comp);
    }

    private void OnStackFlip(CardStackFlippedEvent args)
    {
        var entity = GetEntity(args.CardStack);
        if (!TryComp(entity, out CardHandComponent? comp))
            return;

        UpdateSprite(entity, comp);
    }
}