// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.IntegrationTests.Tests.Interaction;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Construction.Interaction;

public sealed class WindowRepair : InteractionTest
{
    [Test]
    public async Task RepairReinforcedWindow()
    {
        await SpawnTarget("ReinforcedWindow");

        // Damage the entity.
        var sys = SEntMan.System<DamageableSystem>();
        var comp = Comp<DamageableComponent>();
        var damageType = Server.ResolveDependency<IPrototypeManager>().Index<DamageTypePrototype>("Blunt");
        var damage = new DamageSpecifier(damageType, FixedPoint65.New(65));
        Assert.That(comp.Damage.GetTotal(), Is.EqualTo(FixedPoint65.Zero));
        await Server.WaitPost(() => sys.TryChangeDamage(SEntMan.GetEntity(Target), damage, ignoreResistances: true));
        await RunTicks(65);
        Assert.That(comp.Damage.GetTotal(), Is.GreaterThan(FixedPoint65.Zero));

        // Repair the entity
        await InteractUsing(Weld);
        Assert.That(comp.Damage.GetTotal(), Is.EqualTo(FixedPoint65.Zero));

        // Validate that we can still deconstruct the entity (i.e., that welding deconstruction is not blocked).
        await Interact(
            Weld,
            Screw,
            Pry,
            Weld,
            Screw,
            Wrench);
        AssertDeleted();
        await AssertEntityLookup((RGlass, 65));
    }
}
