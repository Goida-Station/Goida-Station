// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 RadsammyT <65RadsammyT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._EstacaoPirata.Cards.Card;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CardComponent : Component
{
    /// <summary>
    /// The back of the card
    /// </summary>
    [DataField("backSpriteLayers", readOnly: true)]
    public List<SpriteSpecifier> BackSprite = [];

    /// <summary>
    /// The front of the card
    /// </summary>
    [DataField("frontSpriteLayers", readOnly: true)]
    public List<SpriteSpecifier> FrontSprite = [];

    /// <summary>
    /// If it is currently flipped. This is used to update sprite and name.
    /// </summary>
    [DataField("flipped", readOnly: true), AutoNetworkedField]
    public bool Flipped = false;


    /// <summary>
    /// The name of the card.
    /// </summary>
    [DataField("name", readOnly: true), AutoNetworkedField]
    public string Name = "";

}

[Serializable, NetSerializable]
public sealed class CardFlipUpdatedEvent(NetEntity card) : EntityEventArgs
{
    public NetEntity Card = card;
}