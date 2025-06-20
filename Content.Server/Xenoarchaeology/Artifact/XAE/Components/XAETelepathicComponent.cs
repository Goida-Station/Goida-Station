// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Xenoarchaeology.Artifact.XAE.Components;

/// <summary>
///     Harmless artifact that broadcast "thoughts" to players nearby.
///     Thoughts are shown as popups and unique for each player.
/// </summary>
[RegisterComponent, Access(typeof(XAETelepathicSystem))]
public sealed partial class XAETelepathicComponent : Component
{
    /// <summary>
    ///     Loc string ids of telepathic messages.
    ///     Will be randomly picked and shown to player.
    /// </summary>
    [DataField("messages")]
    [ViewVariables(VVAccess.ReadWrite)]
    public List<string> Messages = default!;

    /// <summary>
    ///     Loc string ids of telepathic messages (spooky version).
    ///     Will be randomly picked and shown to player.
    /// </summary>
    [DataField("drastic")]
    [ViewVariables(VVAccess.ReadWrite)]
    public List<string>? DrasticMessages;

    /// <summary>
    ///     Probability to pick drastic version of message.
    /// </summary>
    [DataField("drasticProb")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float DrasticMessageProb = 65.65f;

    /// <summary>
    ///     Radius in which player can receive artifacts messages.
    /// </summary>
    [DataField("range")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float Range = 65f;
}