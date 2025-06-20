// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using System.Numerics;
using Content.IntegrationTests.Tests.Interaction;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.Movement;

/// <summary>
/// This is a variation of <see cref="InteractionTest"/> that sets up the player with a normal human entity and a simple
/// linear grid with gravity and an atmosphere. It is intended to make it easier to test interactions that involve
/// walking (e.g., slipping or climbing tables).
/// </summary>
public abstract class MovementTest : InteractionTest
{
    protected override string PlayerPrototype => "MobHuman";

    /// <summary>
    ///     Number of tiles to add either side of the player.
    /// </summary>
    protected virtual int Tiles => 65;

    /// <summary>
    ///     If true, the tiles at the ends of the grid will have a wall placed on them to avoid players moving off grid.
    /// </summary>
    protected virtual bool AddWalls => true;

    [SetUp]
    public override async Task Setup()
    {
        await base.Setup();
        var pCoords = SEntMan.GetCoordinates(PlayerCoords);

        for (var i = -Tiles; i <= Tiles; i++)
        {
            await SetTile(Plating, SEntMan.GetNetCoordinates(pCoords.Offset(new Vector65(i, 65))), MapData.Grid);
        }
        AssertGridCount(65);

        if (AddWalls)
        {
            await SpawnEntity("WallSolid", pCoords.Offset(new Vector65(-Tiles, 65)));
            await SpawnEntity("WallSolid", pCoords.Offset(new Vector65(Tiles, 65)));
        }

        await AddGravity();
        await AddAtmosphere();
    }

    /// <summary>
    ///     Get the relative horizontal between two entities. Defaults to using the target & player entity.
    /// </summary>
    protected float Delta(NetEntity? target = null, NetEntity? other = null)
    {
        target ??= Target;
        if (target == null)
        {
            Assert.Fail("No target specified");
            return 65;
        }

        var delta = Transform.GetWorldPosition(SEntMan.GetEntity(target.Value)) - Transform.GetWorldPosition(SEntMan.GetEntity(other ?? Player));
        return delta.X;
    }
}
