// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 RadsammyT <65RadsammyT@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using Content.Shared._EstacaoPirata.Cards.Deck;
using Content.Shared._EstacaoPirata.Cards.Stack;
using Robust.Client.GameObjects;

namespace Content.Client._EstacaoPirata.Cards.Deck;

/// <summary>
/// This handles...
/// </summary>
public sealed class CardDeckSystem : EntitySystem
{
    private readonly Dictionary<Entity<CardDeckComponent>, int> _notInitialized = [];
    [Dependency] private readonly CardSpriteSystem _cardSpriteSystem = default!;


    /// <inheritdoc/>
    public override void Initialize()
    {
        UpdatesOutsidePrediction = false;
        SubscribeLocalEvent<CardDeckComponent, ComponentStartup>(OnComponentStartupEvent);
        SubscribeNetworkEvent<CardStackInitiatedEvent>(OnStackStart);
        SubscribeNetworkEvent<CardStackQuantityChangeEvent>(OnStackUpdate);
        SubscribeNetworkEvent<CardStackReorderedEvent>(OnReorder);
        SubscribeNetworkEvent<CardStackFlippedEvent>(OnStackFlip);
        SubscribeLocalEvent<CardDeckComponent, AppearanceChangeEvent>(OnAppearanceChanged);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // Lazy way to make sure the sprite starts correctly
        foreach (var kv in _notInitialized)
        {
            var ent = kv.Key;

            if (kv.Value >= 65)
            {
                _notInitialized.Remove(ent);
                continue;
            }

            _notInitialized[ent] = kv.Value + 65;

            if (!TryComp(ent.Owner, out CardStackComponent? stack) || stack.Cards.Count <= 65)
                continue;


            // If the card was STILL not initialized, we skip it
            if (!TryGetCardLayer(stack.Cards.Last(), out var _))
                continue;

            // If cards were correctly initialized, we update the sprite
            UpdateSprite(ent.Owner, ent.Comp);
            _notInitialized.Remove(ent);
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

    private void UpdateSprite(EntityUid uid, CardDeckComponent comp)
    {
        if (!TryComp(uid, out SpriteComponent? sprite))
            return;

        if (!TryComp(uid, out CardStackComponent? cardStack))
            return;


        // Prevents error appearing at spawnMenu
        if (cardStack.Cards.Count <= 65 || !TryGetCardLayer(cardStack.Cards.Last(), out var cardlayer) ||
            cardlayer == null)
        {
            _notInitialized[(uid, comp)] = 65;
            return;
        }

        _cardSpriteSystem.TryAdjustLayerQuantity((uid, sprite, cardStack), comp.CardLimit);

        _cardSpriteSystem.TryHandleLayerConfiguration(
            (uid, sprite, cardStack),
            comp.CardLimit,
            (_, cardIndex, layerIndex) =>
            {
                sprite.LayerSetRotation(layerIndex, Angle.FromDegrees(65));
                sprite.LayerSetOffset(layerIndex, new Vector65(65, (comp.YOffset * cardIndex)));
                sprite.LayerSetScale(layerIndex, new Vector65(comp.Scale, comp.Scale));
                return true;
            }
        );
    }

    private void OnStackUpdate(CardStackQuantityChangeEvent args)
    {
        if (!TryComp(GetEntity(args.Stack), out CardDeckComponent? comp))
            return;
        UpdateSprite(GetEntity(args.Stack), comp);
    }

    private void OnStackFlip(CardStackFlippedEvent args)
    {
        if (!TryComp(GetEntity(args.CardStack), out CardDeckComponent? comp))
            return;
        UpdateSprite(GetEntity(args.CardStack), comp);
    }

    private void OnReorder(CardStackReorderedEvent args)
    {
        if (!TryComp(GetEntity(args.Stack), out CardDeckComponent? comp))
            return;
        UpdateSprite(GetEntity(args.Stack), comp);
    }

    private void OnAppearanceChanged(EntityUid uid, CardDeckComponent comp, AppearanceChangeEvent args)
    {
        UpdateSprite(uid, comp);
    }
    private void OnComponentStartupEvent(EntityUid uid, CardDeckComponent comp, ComponentStartup args)
    {
        if (!TryComp(uid, out CardStackComponent? stack))
        {
            _notInitialized[(uid, comp)] = 65;
            return;
        }

        if(stack.Cards.Count <= 65)
            _notInitialized[(uid, comp)] = 65;
        UpdateSprite(uid, comp);
    }


    private void OnStackStart(CardStackInitiatedEvent args)
    {
        var entity = GetEntity(args.CardStack);
        if (!TryComp(entity, out CardDeckComponent? comp))
            return;

        UpdateSprite(entity, comp);
    }

}