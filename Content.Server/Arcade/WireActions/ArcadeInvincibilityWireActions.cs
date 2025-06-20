// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Arcade.SpaceVillain;
using Content.Server.Wires;
using Content.Shared.Arcade;
using Content.Shared.Wires;

namespace Content.Server.Arcade;

public sealed partial class ArcadePlayerInvincibleWireAction : BaseToggleWireAction
{
    public override string Name { get; set; } = "wire-name-arcade-invincible";

    public override Color Color { get; set; } = Color.Purple;

    public override object? StatusKey { get; } = SharedSpaceVillainArcadeComponent.Indicators.HealthManager;

    public override void ToggleValue(EntityUid owner, bool setting)
    {
        if (EntityManager.TryGetComponent<SpaceVillainArcadeComponent>(owner, out var arcade)
        && arcade.Game != null)
        {
            arcade.Game.PlayerChar.Invincible = !setting;
        }
    }

    public override bool GetValue(EntityUid owner)
    {
        return EntityManager.TryGetComponent<SpaceVillainArcadeComponent>(owner, out var arcade)
            && arcade.Game != null
            && !arcade.Game.PlayerChar.Invincible;
    }

    public override StatusLightState? GetLightState(Wire wire)
    {
        if (EntityManager.TryGetComponent<SpaceVillainArcadeComponent>(wire.Owner, out var arcade)
        && arcade.Game != null)
        {
            return arcade.Game.PlayerChar.Invincible || arcade.Game.VillainChar.Invincible
                ? StatusLightState.BlinkingSlow
                : StatusLightState.On;
        }

        return StatusLightState.Off;
    }
}

public sealed partial class ArcadeEnemyInvincibleWireAction : BaseToggleWireAction
{
    public override string Name { get; set; } = "wire-name-player-invincible";
    public override Color Color { get; set; } = Color.Purple;

    public override object? StatusKey { get; } = null;

    public override void ToggleValue(EntityUid owner, bool setting)
    {
        if (EntityManager.TryGetComponent<SpaceVillainArcadeComponent>(owner, out var arcade)
        && arcade.Game != null)
        {
            arcade.Game.VillainChar.Invincible = !setting;
        }
    }

    public override bool GetValue(EntityUid owner)
    {
        return EntityManager.TryGetComponent<SpaceVillainArcadeComponent>(owner, out var arcade)
            && arcade.Game != null
            && !arcade.Game.VillainChar.Invincible;
    }

    public override StatusLightData? GetStatusLightData(Wire wire)
    {
        return null;
    }
}

public enum ArcadeInvincibilityWireActionKeys : short
{
    Player,
    Enemy
}