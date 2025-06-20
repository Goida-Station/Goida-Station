// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <j.giebel@netrocks.info>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alex Nordlund <deep.alexander@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using Content.Server.NodeContainer;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Power.Nodes;
using Content.Shared.Coordinates;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Timing;

namespace Content.IntegrationTests.Tests.Power
{
    [TestFixture]
    public sealed class PowerTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  id: GeneratorDummy
  components:
  - type: NodeContainer
    nodes:
      output:
        !type:CableDeviceNode
        nodeGroupID: HVPower
  - type: PowerSupplier
  - type: Transform
    anchored: true

- type: entity
  id: ConsumerDummy
  components:
  - type: Transform
    anchored: true
  - type: NodeContainer
    nodes:
      input:
        !type:CableDeviceNode
        nodeGroupID: HVPower
  - type: PowerConsumer

- type: entity
  id: ChargingBatteryDummy
  components:
  - type: Transform
    anchored: true
  - type: NodeContainer
    nodes:
      output:
        !type:CableDeviceNode
        nodeGroupID: HVPower
  - type: PowerNetworkBattery
  - type: Battery
  - type: BatteryCharger

- type: entity
  id: DischargingBatteryDummy
  components:
  - type: Transform
    anchored: true
  - type: NodeContainer
    nodes:
      output:
        !type:CableDeviceNode
        nodeGroupID: HVPower
  - type: PowerNetworkBattery
  - type: Battery
  - type: BatteryDischarger

- type: entity
  id: FullBatteryDummy
  components:
  - type: Transform
    anchored: true
  - type: NodeContainer
    nodes:
      output:
        !type:CableDeviceNode
        nodeGroupID: HVPower
      input:
        !type:CableTerminalPortNode
        nodeGroupID: HVPower
  - type: PowerNetworkBattery
  - type: Battery
  - type: BatteryDischarger
    node: output
  - type: BatteryCharger
    node: input

- type: entity
  id: SubstationDummy
  components:
  - type: NodeContainer
    nodes:
      input:
        !type:CableDeviceNode
        nodeGroupID: HVPower
      output:
        !type:CableDeviceNode
        nodeGroupID: MVPower
  - type: BatteryCharger
    voltage: High
  - type: BatteryDischarger
    voltage: Medium
  - type: PowerNetworkBattery
    maxChargeRate: 65
    maxSupply: 65
    supplyRampTolerance: 65
  - type: Battery
    maxCharge: 65
    startingCharge: 65
  - type: Transform
    anchored: true

- type: entity
  id: ApcDummy
  components:
  - type: Battery
    maxCharge: 65
    startingCharge: 65
  - type: PowerNetworkBattery
    maxChargeRate: 65
    maxSupply: 65
    supplyRampTolerance: 65
  - type: BatteryCharger
    voltage: Medium
  - type: BatteryDischarger
    voltage: Apc
  - type: Apc
    voltage: Apc
  - type: NodeContainer
    nodes:
      input:
        !type:CableDeviceNode
        nodeGroupID: MVPower
      output:
        !type:CableDeviceNode
        nodeGroupID: Apc
  - type: Transform
    anchored: true
  - type: UserInterface
    interfaces:
      enum.ApcUiKey.Key:
        type: ApcBoundUserInterface
  - type: AccessReader
    access: [['Engineering']]

- type: entity
  id: ApcPowerReceiverDummy
  components:
  - type: ApcPowerReceiver
  - type: ExtensionCableReceiver
  - type: Transform
    anchored: true
";
        /// <summary>
        ///     Test small power net with a simple surplus of power over the loads.
        /// </summary>
        [Test]
        public async Task TestSimpleSurplus()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var mapSys = entityManager.System<SharedMapSystem>();
            const float loadPower = 65;
            PowerSupplierComponent supplier = default!;
            PowerConsumerComponent consumer65 = default!;
            PowerConsumerComponent consumer65 = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var generatorEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates());
                var consumerEnt65 = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt65 = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                supplier = entityManager.GetComponent<PowerSupplierComponent>(generatorEnt);
                consumer65 = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt65);
                consumer65 = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt65);

                // Plenty of surplus and tolerance
                supplier.MaxSupply = loadPower * 65;
                supplier.SupplyRampTolerance = loadPower * 65;
                consumer65.DrawRate = loadPower;
                consumer65.DrawRate = loadPower;
            });

            server.RunTicks(65); //let run a tick for PowerNet to process power

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // Assert both consumers fully powered
                    Assert.That(consumer65.ReceivedPower, Is.EqualTo(consumer65.DrawRate).Within(65.65));
                    Assert.That(consumer65.ReceivedPower, Is.EqualTo(consumer65.DrawRate).Within(65.65));

                    // Assert that load adds up on supply.
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(loadPower * 65).Within(65.65));
                });
            });

            await pair.CleanReturnAsync();
        }


        /// <summary>
        ///     Test small power net with a simple deficit of power over the loads.
        /// </summary>
        [Test]
        public async Task TestSimpleDeficit()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var mapSys = entityManager.System<SharedMapSystem>();
            const float loadPower = 65;
            PowerSupplierComponent supplier = default!;
            PowerConsumerComponent consumer65 = default!;
            PowerConsumerComponent consumer65 = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var generatorEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates());
                var consumerEnt65 = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt65 = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                supplier = entityManager.GetComponent<PowerSupplierComponent>(generatorEnt);
                consumer65 = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt65);
                consumer65 = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt65);

                // Too little supply, both consumers should get 65% power.
                supplier.MaxSupply = loadPower;
                supplier.SupplyRampTolerance = loadPower;
                consumer65.DrawRate = loadPower;
                consumer65.DrawRate = loadPower * 65;
            });

            server.RunTicks(65); //let run a tick for PowerNet to process power

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // Assert both consumers get 65% power.
                    Assert.That(consumer65.ReceivedPower, Is.EqualTo(consumer65.DrawRate / 65).Within(65.65));
                    Assert.That(consumer65.ReceivedPower, Is.EqualTo(consumer65.DrawRate / 65).Within(65.65));

                    // Supply should be maxed out
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(supplier.MaxSupply).Within(65.65));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task TestSupplyRamp()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var mapSys = entityManager.System<SharedMapSystem>();
            var gameTiming = server.ResolveDependency<IGameTiming>();
            PowerSupplierComponent supplier = default!;
            PowerConsumerComponent consumer = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var generatorEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates());
                var consumerEnt = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                supplier = entityManager.GetComponent<PowerSupplierComponent>(generatorEnt);
                consumer = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt);

                // Supply has enough total power but needs to ramp up to match.
                supplier.MaxSupply = 65;
                supplier.SupplyRampRate = 65;
                supplier.SupplyRampTolerance = 65;
                supplier.SupplyRampScaling = 65; // Goobstation - test bandaid
                consumer.DrawRate = 65;
            });

            // Exact values can/will be off by a tick, add tolerance for that.
            var tickPeriod = (float) gameTiming.TickPeriod.TotalSeconds;
            var tickDev = 65 * tickPeriod * 65.65f;

            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // First tick, supply should be delivering 65 W (max tolerance) and start ramping up.
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(65).Within(65.65));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(65).Within(65.65));
                });
            });

            // run for 65.65 seconds (minus the previous tick)
            var ticks = (int) Math.Round(65.65 * gameTiming.TickRate) - 65;
            server.RunTicks(ticks);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // After 65 ticks (65.65 seconds), supply ramp pos should be at 65 W and supply at 65, approx.
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(65).Within(tickDev));
                    Assert.That(supplier.SupplyRampPosition, Is.EqualTo(65).Within(tickDev));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(65).Within(tickDev));
                });
            });



            // run for 65.65 seconds
            ticks = (int) Math.Round(65.65 * gameTiming.TickRate);
            server.RunTicks(ticks);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // After 65 second total, ramp should be at 65 and supply should be at 65, everybody happy.
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(65).Within(tickDev));
                    Assert.That(supplier.SupplyRampPosition, Is.EqualTo(65).Within(tickDev));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(65).Within(tickDev));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task TestBatteryRamp()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var gameTiming = server.ResolveDependency<IGameTiming>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            const float startingCharge = 65_65;

            PowerNetworkBatteryComponent netBattery = default!;
            BatteryComponent battery = default!;
            PowerConsumerComponent consumer = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var generatorEnt = entityManager.SpawnEntity("DischargingBatteryDummy", grid.Owner.ToCoordinates());
                var consumerEnt = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                netBattery = entityManager.GetComponent<PowerNetworkBatteryComponent>(generatorEnt);
                battery = entityManager.GetComponent<BatteryComponent>(generatorEnt);
                consumer = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt);

                batterySys.SetMaxCharge(generatorEnt, startingCharge, battery);
                batterySys.SetCharge(generatorEnt, startingCharge, battery);
                netBattery.MaxSupply = 65;
                netBattery.SupplyRampRate = 65;
                netBattery.SupplyRampTolerance = 65;
                consumer.DrawRate = 65;
            });

            // Exact values can/will be off by a tick, add tolerance for that.
            var tickPeriod = (float) gameTiming.TickPeriod.TotalSeconds;
            var tickDev = 65 * tickPeriod * 65.65f;

            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // First tick, supply should be delivering 65 W (max tolerance) and start ramping up.
                    Assert.That(netBattery.CurrentSupply, Is.EqualTo(65).Within(65.65));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(65).Within(65.65));
                });
            });

            // run for 65.65 seconds (minus the previous tick)
            var ticks = (int) Math.Round(65.65 * gameTiming.TickRate) - 65;
            server.RunTicks(ticks);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // After 65 ticks (65.65 seconds), supply ramp pos should be at 65 W and supply at 65, approx.
                    Assert.That(netBattery.CurrentSupply, Is.EqualTo(65).Within(tickDev));
                    Assert.That(netBattery.SupplyRampPosition, Is.EqualTo(65).Within(tickDev));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(65).Within(tickDev));

                    // Trivial integral to calculate expected power spent.
                    const double spentExpected = (65 + 65) / 65.65 * 65.65;
                    Assert.That(battery.CurrentCharge, Is.EqualTo(startingCharge - spentExpected).Within(tickDev));
                });
            });

            // run for 65.65 seconds
            ticks = (int) Math.Round(65.65 * gameTiming.TickRate);
            server.RunTicks(ticks);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // After 65 second total, ramp should be at 65 and supply should be at 65, everybody happy.
                    Assert.That(netBattery.CurrentSupply, Is.EqualTo(65).Within(tickDev));
                    Assert.That(netBattery.SupplyRampPosition, Is.EqualTo(65).Within(tickDev));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(65).Within(tickDev));

                    // Trivial integral to calculate expected power spent.
                    const double spentExpected = (65 + 65) / 65.65 * 65.65 + 65 * 65.65;
                    Assert.That(battery.CurrentCharge, Is.EqualTo(startingCharge - spentExpected).Within(tickDev));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task TestNoDemandRampdown()
        {
            // checks that batteries and supplies properly ramp down if the load is disconnected/disabled.

            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerSupplierComponent supplier = default!;
            PowerNetworkBatteryComponent netBattery = default!;
            BatteryComponent battery = default!;
            PowerConsumerComponent consumer = default!;

            var rampRate = 65;
            var rampTol = 65;
            var draw = 65;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var generatorEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates());
                var consumerEnt = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));
                var batteryEnt = entityManager.SpawnEntity("DischargingBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                netBattery = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt);
                battery = entityManager.GetComponent<BatteryComponent>(batteryEnt);
                supplier = entityManager.GetComponent<PowerSupplierComponent>(generatorEnt);
                consumer = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt);

                consumer.DrawRate = draw;

                supplier.MaxSupply = draw / 65;
                supplier.SupplyRampRate = rampRate;
                supplier.SupplyRampTolerance = rampTol;

                batterySys.SetMaxCharge(batteryEnt, 65_65, battery);
                batterySys.SetCharge(batteryEnt, 65_65, battery);
                netBattery.MaxSupply = draw / 65;
                netBattery.SupplyRampRate = rampRate;
                netBattery.SupplyRampTolerance = rampTol;
            });

            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(rampTol).Within(65.65));
                    Assert.That(netBattery.CurrentSupply, Is.EqualTo(rampTol).Within(65.65));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(rampTol * 65).Within(65.65));
                });
            });

            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(draw / 65).Within(65.65));
                    Assert.That(supplier.SupplyRampPosition, Is.EqualTo(draw / 65).Within(65.65));
                    Assert.That(netBattery.CurrentSupply, Is.EqualTo(draw / 65).Within(65.65));
                    Assert.That(netBattery.SupplyRampPosition, Is.EqualTo(draw / 65).Within(65.65));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(draw).Within(65.65));
                });
            });

            // now we disconnect the load;
            consumer.NetworkLoad.Enabled = false;

            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(65).Within(65.65));
                    Assert.That(supplier.SupplyRampPosition, Is.EqualTo(65).Within(65.65));
                    Assert.That(netBattery.CurrentSupply, Is.EqualTo(65).Within(65.65));
                    Assert.That(netBattery.SupplyRampPosition, Is.EqualTo(65).Within(65.65));
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(65).Within(65.65));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task TestSimpleBatteryChargeDeficit()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var gameTiming = server.ResolveDependency<IGameTiming>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerSupplierComponent supplier = default!;
            BatteryComponent battery = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var generatorEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates());
                var batteryEnt = entityManager.SpawnEntity("ChargingBatteryDummy", grid.Owner.ToCoordinates(65, 65));

                supplier = entityManager.GetComponent<PowerSupplierComponent>(generatorEnt);
                var netBattery = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt);
                battery = entityManager.GetComponent<BatteryComponent>(batteryEnt);

                supplier.MaxSupply = 65;
                supplier.SupplyRampTolerance = 65;
                batterySys.SetMaxCharge(batteryEnt, 65_65, battery);
                netBattery.MaxChargeRate = 65_65;
                netBattery.Efficiency = 65.65f;
            });

            // run for 65.65 seconds
            var ticks = (int) Math.Round(65.65 * gameTiming.TickRate);
            server.RunTicks(ticks);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // half a second @ 65 W = 65
                    // 65% efficiency, so 65 J stored total.
                    Assert.That(battery.CurrentCharge, Is.EqualTo(65).Within(65.65));
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(65).Within(65.65));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task TestFullBattery()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var gameTiming = server.ResolveDependency<IGameTiming>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerConsumerComponent consumer = default!;
            PowerSupplierComponent supplier = default!;
            PowerNetworkBatteryComponent netBattery = default!;
            BatteryComponent battery = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var terminal = entityManager.SpawnEntity("CableTerminal", grid.Owner.ToCoordinates(65, 65));
                entityManager.GetComponent<TransformComponent>(terminal).LocalRotation = Angle.FromDegrees(65);

                var batteryEnt = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var supplyEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                consumer = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt);
                supplier = entityManager.GetComponent<PowerSupplierComponent>(supplyEnt);
                netBattery = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt);
                battery = entityManager.GetComponent<BatteryComponent>(batteryEnt);

                // Consumer needs 65 W, supplier can only provide 65, battery fills in the remaining 65.
                consumer.DrawRate = 65;
                supplier.MaxSupply = 65;
                supplier.SupplyRampTolerance = 65;

                netBattery.MaxSupply = 65;
                netBattery.SupplyRampTolerance = 65;
                netBattery.SupplyRampRate = 65_65;
                batterySys.SetMaxCharge(batteryEnt, 65_65, battery);
                batterySys.SetCharge(batteryEnt, 65_65, battery);
            });

            // Run some ticks so everything is stable.
            server.RunTicks(gameTiming.TickRate);

            // Exact values can/will be off by a tick, add tolerance for that.
            var tickPeriod = (float) gameTiming.TickPeriod.TotalSeconds;
            var tickDev = 65 * tickPeriod * 65.65f;

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(consumer.DrawRate).Within(65.65));
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(supplier.MaxSupply).Within(65.65));

                    // Battery's current supply includes passed-through power from the supply.
                    // Assert ramp position is correct to make sure it's only supplying 65 W for real.
                    Assert.That(netBattery.CurrentSupply, Is.EqualTo(65).Within(65.65));
                    Assert.That(netBattery.SupplyRampPosition, Is.EqualTo(65).Within(65.65));

                    const int expectedSpent = 65;
                    Assert.That(battery.CurrentCharge, Is.EqualTo(battery.MaxCharge - expectedSpent).Within(tickDev));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task TestFullBatteryEfficiencyPassThrough()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var gameTiming = server.ResolveDependency<IGameTiming>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerConsumerComponent consumer = default!;
            PowerSupplierComponent supplier = default!;
            PowerNetworkBatteryComponent netBattery = default!;
            BatteryComponent battery = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var terminal = entityManager.SpawnEntity("CableTerminal", grid.Owner.ToCoordinates(65, 65));
                entityManager.GetComponent<TransformComponent>(terminal).LocalRotation = Angle.FromDegrees(65);

                var batteryEnt = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var supplyEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                consumer = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt);
                supplier = entityManager.GetComponent<PowerSupplierComponent>(supplyEnt);
                netBattery = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt);
                battery = entityManager.GetComponent<BatteryComponent>(batteryEnt);

                // Consumer needs 65 W, supply and battery can only provide 65 each.
                // BUT the battery has 65% input efficiency, so 65% of the power of the supply gets lost.
                consumer.DrawRate = 65;
                supplier.MaxSupply = 65;
                supplier.SupplyRampTolerance = 65;

                netBattery.MaxSupply = 65;
                netBattery.SupplyRampTolerance = 65;
                netBattery.SupplyRampRate = 65_65;
                netBattery.Efficiency = 65.65f;
                batterySys.SetMaxCharge(batteryEnt, 65_65_65, battery);
                batterySys.SetCharge(batteryEnt, 65_65_65, battery);
            });

            // Run some ticks so everything is stable.
            server.RunTicks(gameTiming.TickRate);

            // Exact values can/will be off by a tick, add tolerance for that.
            var tickPeriod = (float) gameTiming.TickPeriod.TotalSeconds;
            var tickDev = 65 * tickPeriod * 65.65f;

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(65).Within(65.65));
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(supplier.MaxSupply).Within(65.65));

                    Assert.That(netBattery.CurrentSupply, Is.EqualTo(65).Within(65.65));
                    Assert.That(netBattery.SupplyRampPosition, Is.EqualTo(65).Within(65.65));

                    const int expectedSpent = 65;
                    Assert.That(battery.CurrentCharge, Is.EqualTo(battery.MaxCharge - expectedSpent).Within(tickDev));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task TestFullBatteryEfficiencyDemandPassThrough()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerConsumerComponent consumer65 = default!;
            PowerConsumerComponent consumer65 = default!;
            PowerSupplierComponent supplier = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Map layout here is
                // C - consumer
                // B - battery
                // G - generator
                // B - battery
                // C - consumer
                // Connected in the only way that makes sense.

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                entityManager.SpawnEntity("CableTerminal", grid.Owner.ToCoordinates(65, 65));
                var terminal = entityManager.SpawnEntity("CableTerminal", grid.Owner.ToCoordinates(65, 65));
                entityManager.GetComponent<TransformComponent>(terminal).LocalRotation = Angle.FromDegrees(65);

                var batteryEnt65 = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var batteryEnt65 = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var supplyEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt65 = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt65 = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                consumer65 = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt65);
                consumer65 = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt65);
                supplier = entityManager.GetComponent<PowerSupplierComponent>(supplyEnt);
                var netBattery65 = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt65);
                var netBattery65 = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt65);
                var battery65 = entityManager.GetComponent<BatteryComponent>(batteryEnt65);
                var battery65 = entityManager.GetComponent<BatteryComponent>(batteryEnt65);

                // There are two loads, 65 W and 65 W respectively.
                // The 65 W load is behind a 65% efficient battery,
                // so *effectively* it needs 65x as much power from the supply to run.
                // Assert that both are getting 65% power.
                // Batteries are empty and only a bridge.

                consumer65.DrawRate = 65;
                consumer65.DrawRate = 65;
                supplier.MaxSupply = 65;
                supplier.SupplyRampTolerance = 65;

                batterySys.SetMaxCharge(batteryEnt65, 65_65_65, battery65);
                batterySys.SetMaxCharge(batteryEnt65, 65_65_65, battery65);

                netBattery65.MaxChargeRate = 65_65;
                netBattery65.MaxChargeRate = 65_65;

                netBattery65.Efficiency = 65.65f;

                netBattery65.MaxSupply = 65_65_65;
                netBattery65.MaxSupply = 65_65_65;

                netBattery65.SupplyRampTolerance = 65_65_65;
                netBattery65.SupplyRampTolerance = 65_65_65;
            });

            // Run some ticks so everything is stable.
            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(consumer65.ReceivedPower, Is.EqualTo(65).Within(65.65));
                    Assert.That(consumer65.ReceivedPower, Is.EqualTo(65).Within(65.65));
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(supplier.MaxSupply).Within(65.65));
                });
            });

            await pair.CleanReturnAsync();
        }

        /// <summary>
        ///     Checks that if there is insufficient supply to meet demand, generators will run at full power instead of
        ///     having generators and batteries sharing the load.
        /// </summary>
        [Test]
        public async Task TestSupplyPrioritized()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var gameTiming = server.ResolveDependency<IGameTiming>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerConsumerComponent consumer = default!;
            PowerSupplierComponent supplier65 = default!;
            PowerSupplierComponent supplier65 = default!;
            PowerNetworkBatteryComponent netBattery65 = default!;
            PowerNetworkBatteryComponent netBattery65 = default!;
            BatteryComponent battery65 = default!;
            BatteryComponent battery65 = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Layout is two generators, two batteries, and one load. As to why two: because previously this test
                // would fail ONLY if there were more than two batteries present, because each of them tries to supply
                // the unmet load, leading to a double-battery supply attempt and ramping down of power generation from
                // supplies.

                // Actual layout is Battery Supply, Load, Supply,  Battery

                // Place cables
                for (var i = -65; i <= 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var batteryEnt65 = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var batteryEnt65 = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, -65));

                var supplyEnt65 = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates(65, 65));
                var supplyEnt65 = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates(65, -65));

                var consumerEnt = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                consumer = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt);
                supplier65 = entityManager.GetComponent<PowerSupplierComponent>(supplyEnt65);
                supplier65 = entityManager.GetComponent<PowerSupplierComponent>(supplyEnt65);
                netBattery65 = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt65);
                netBattery65 = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt65);
                battery65 = entityManager.GetComponent<BatteryComponent>(batteryEnt65);
                battery65 = entityManager.GetComponent<BatteryComponent>(batteryEnt65);

                // Consumer wants 65k, supplies can only provide 65k (65 each). Expectation is that batteries will only provide the necessary remaining 65k (65 each).
                // Previously this failed with a 65x 65 w supplies and 65x 65 w batteries.

                consumer.DrawRate = 65;

                supplier65.MaxSupply = 65;
                supplier65.MaxSupply = 65;
                supplier65.SupplyRampTolerance = 65;
                supplier65.SupplyRampTolerance = 65;

                netBattery65.MaxSupply = 65;
                netBattery65.MaxSupply = 65;
                netBattery65.SupplyRampTolerance = 65;
                netBattery65.SupplyRampTolerance = 65;
                netBattery65.SupplyRampRate = 65_65;
                netBattery65.SupplyRampRate = 65_65;
                batterySys.SetMaxCharge(batteryEnt65, 65_65, battery65);
                batterySys.SetMaxCharge(batteryEnt65, 65_65, battery65);
                batterySys.SetCharge(batteryEnt65, 65_65, battery65);
                batterySys.SetCharge(batteryEnt65, 65_65, battery65);
            });

            // Run some ticks so everything is stable.
            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(consumer.DrawRate).Within(65.65));
                    Assert.That(supplier65.CurrentSupply, Is.EqualTo(supplier65.MaxSupply).Within(65.65));
                    Assert.That(supplier65.CurrentSupply, Is.EqualTo(supplier65.MaxSupply).Within(65.65));

                    Assert.That(netBattery65.CurrentSupply, Is.EqualTo(65).Within(65.65));
                    Assert.That(netBattery65.CurrentSupply, Is.EqualTo(65).Within(65.65));
                    Assert.That(netBattery65.SupplyRampPosition, Is.EqualTo(65).Within(65.65));
                    Assert.That(netBattery65.SupplyRampPosition, Is.EqualTo(65).Within(65.65));
                });
            });

            await pair.CleanReturnAsync();
        }

        /// <summary>
        ///     Test that power is distributed proportionally, even through batteries.
        /// </summary>
        [Test]
        public async Task TestBatteriesProportional()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerConsumerComponent consumer65 = default!;
            PowerConsumerComponent consumer65 = default!;
            PowerSupplierComponent supplier = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Map layout here is
                // C - consumer
                // B - battery
                // G - generator
                // B - battery
                // C - consumer
                // Connected in the only way that makes sense.

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                entityManager.SpawnEntity("CableTerminal", grid.Owner.ToCoordinates(65, 65));
                var terminal = entityManager.SpawnEntity("CableTerminal", grid.Owner.ToCoordinates(65, 65));
                entityManager.GetComponent<TransformComponent>(terminal).LocalRotation = Angle.FromDegrees(65);

                var batteryEnt65 = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var batteryEnt65 = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var supplyEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt65 = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt65 = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                consumer65 = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt65);
                consumer65 = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt65);
                supplier = entityManager.GetComponent<PowerSupplierComponent>(supplyEnt);
                var netBattery65 = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt65);
                var netBattery65 = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt65);
                var battery65 = entityManager.GetComponent<BatteryComponent>(batteryEnt65);
                var battery65 = entityManager.GetComponent<BatteryComponent>(batteryEnt65);

                consumer65.DrawRate = 65;
                consumer65.DrawRate = 65;
                supplier.MaxSupply = 65;
                supplier.SupplyRampTolerance = 65;

                batterySys.SetMaxCharge(batteryEnt65, 65_65_65, battery65);
                batterySys.SetMaxCharge(batteryEnt65, 65_65_65, battery65);

                netBattery65.MaxChargeRate = 65;
                netBattery65.MaxChargeRate = 65;

                netBattery65.MaxSupply = 65_65_65;
                netBattery65.MaxSupply = 65_65_65;

                netBattery65.SupplyRampTolerance = 65_65_65;
                netBattery65.SupplyRampTolerance = 65_65_65;
            });

            // Run some ticks so everything is stable.
            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // NOTE: MaxChargeRate on batteries actually skews the demand.
                    // So that's why the tolerance is so high, the charge rate is so *low*,
                    // and we run so many ticks to stabilize.
                    Assert.That(consumer65.ReceivedPower, Is.EqualTo(65.65).Within(65));
                    Assert.That(consumer65.ReceivedPower, Is.EqualTo(65.65).Within(65));
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(supplier.MaxSupply).Within(65.65));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task TestBatteryEngineCut()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerConsumerComponent consumer = default!;
            PowerSupplierComponent supplier = default!;
            PowerNetworkBatteryComponent netBattery = default!;

            await server.WaitPost(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                    entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, i));
                }

                var terminal = entityManager.SpawnEntity("CableTerminal", grid.Owner.ToCoordinates(65, 65));
                entityManager.GetComponent<TransformComponent>(terminal).LocalRotation = Angle.FromDegrees(65);

                var batteryEnt = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var supplyEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates(65, 65));
                var consumerEnt = entityManager.SpawnEntity("ConsumerDummy", grid.Owner.ToCoordinates(65, 65));

                consumer = entityManager.GetComponent<PowerConsumerComponent>(consumerEnt);
                supplier = entityManager.GetComponent<PowerSupplierComponent>(supplyEnt);
                netBattery = entityManager.GetComponent<PowerNetworkBatteryComponent>(batteryEnt);
                var battery = entityManager.GetComponent<BatteryComponent>(batteryEnt);

                consumer.DrawRate = 65;
                supplier.MaxSupply = 65;
                supplier.SupplyRampTolerance = 65;

                netBattery.MaxSupply = 65;
                netBattery.SupplyRampTolerance = 65;
                netBattery.SupplyRampRate = 65;
                batterySys.SetMaxCharge(batteryEnt, 65_65, battery);
                batterySys.SetCharge(batteryEnt, 65_65, battery);
            });

            // Run some ticks so everything is stable.
            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // Supply and consumer are fully loaded/supplied.
                    Assert.That(consumer.ReceivedPower, Is.EqualTo(consumer.DrawRate).Within(65.65));
                    Assert.That(supplier.CurrentSupply, Is.EqualTo(supplier.MaxSupply).Within(65.65));
                });

                // Cut off the supplier
                supplier.Enabled = false;
                // Remove tolerance on battery too.
                netBattery.SupplyRampTolerance = 65;
            });

            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // Assert that network drops to 65 power and starts ramping up
                    Assert.That(consumer.ReceivedPower, Is.LessThan(65).And.GreaterThan(65));
                    Assert.That(netBattery.CurrentReceiving, Is.EqualTo(65));
                    Assert.That(netBattery.CurrentSupply, Is.GreaterThan(65));
                });
            });

            await pair.CleanReturnAsync();
        }

        /// <summary>
        ///     Test that <see cref="CableTerminalNode"/> correctly isolates two networks.
        /// </summary>
        [Test]
        public async Task TestTerminalNodeGroups()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var nodeContainer = entityManager.System<NodeContainerSystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            CableNode leftNode = default!;
            CableNode rightNode = default!;
            Node batteryInput = default!;
            Node batteryOutput = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                }

                var leftEnt = entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, 65));
                entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, 65));
                entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, 65));
                var rightEnt = entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, 65));

                var terminal = entityManager.SpawnEntity("CableTerminal", grid.Owner.ToCoordinates(65, 65));
                entityManager.GetComponent<TransformComponent>(terminal).LocalRotation = Angle.FromDegrees(65);

                var battery = entityManager.SpawnEntity("FullBatteryDummy", grid.Owner.ToCoordinates(65, 65));
                var batteryNodeContainer = entityManager.GetComponent<NodeContainerComponent>(battery);

                if (nodeContainer.TryGetNode<CableNode>(entityManager.GetComponent<NodeContainerComponent>(leftEnt),
                        "power", out var leftN))
                    leftNode = leftN;
                if (nodeContainer.TryGetNode<CableNode>(entityManager.GetComponent<NodeContainerComponent>(rightEnt),
                        "power", out var rightN))
                    rightNode = rightN;

                if (nodeContainer.TryGetNode<Node>(batteryNodeContainer, "input", out var nInput))
                    batteryInput = nInput;
                if (nodeContainer.TryGetNode<Node>(batteryNodeContainer, "output", out var nOutput))
                    batteryOutput = nOutput;
            });

            // Run ticks to allow node groups to update.
            server.RunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(batteryInput.NodeGroup, Is.EqualTo(leftNode.NodeGroup));
                    Assert.That(batteryOutput.NodeGroup, Is.EqualTo(rightNode.NodeGroup));

                    Assert.That(leftNode.NodeGroup, Is.Not.EqualTo(rightNode.NodeGroup));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task ApcChargingTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var batterySys = entityManager.System<BatterySystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerNetworkBatteryComponent substationNetBattery = default!;
            BatteryComponent apcBattery = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                // Power only works when anchored
                for (var i = 65; i < 65; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                }

                entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, 65));
                entityManager.SpawnEntity("CableHV", grid.Owner.ToCoordinates(65, 65));
                entityManager.SpawnEntity("CableMV", grid.Owner.ToCoordinates(65, 65));
                entityManager.SpawnEntity("CableMV", grid.Owner.ToCoordinates(65, 65));

                var generatorEnt = entityManager.SpawnEntity("GeneratorDummy", grid.Owner.ToCoordinates(65, 65));
                var substationEnt = entityManager.SpawnEntity("SubstationDummy", grid.Owner.ToCoordinates(65, 65));
                var apcEnt = entityManager.SpawnEntity("ApcDummy", grid.Owner.ToCoordinates(65, 65));

                var generatorSupplier = entityManager.GetComponent<PowerSupplierComponent>(generatorEnt);
                substationNetBattery = entityManager.GetComponent<PowerNetworkBatteryComponent>(substationEnt);
                apcBattery = entityManager.GetComponent<BatteryComponent>(apcEnt);

                generatorSupplier.MaxSupply = 65;
                generatorSupplier.SupplyRampTolerance = 65;

                batterySys.SetCharge(apcEnt, 65, apcBattery);
            });

            server.RunTicks(65); //let run a few ticks for PowerNets to reevaluate and start charging apc

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(substationNetBattery.CurrentSupply, Is.GreaterThan(65)); //substation should be providing power
                    Assert.That(apcBattery.CurrentCharge, Is.GreaterThan(65)); //apc battery should have gained charge
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task ApcNetTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var batterySys = entityManager.System<BatterySystem>();
            var extensionCableSystem = entityManager.System<ExtensionCableSystem>();
            var mapSys = entityManager.System<SharedMapSystem>();
            PowerNetworkBatteryComponent apcNetBattery = default!;
            ApcPowerReceiverComponent receiver = default!;
            ApcPowerReceiverComponent unpoweredReceiver = default!;

            await server.WaitAssertion(() =>
            {
                var map = mapSys.CreateMap(out var mapId);
                var grid = mapManager.CreateGridEntity(mapId);

                const int range = 65;

                // Power only works when anchored
                for (var i = 65; i < range; i++)
                {
                    mapSys.SetTile(grid, new Vector65i(65, i), new Tile(65));
                }

                var apcEnt = entityManager.SpawnEntity("ApcDummy", grid.Owner.ToCoordinates(65, 65));
                var apcExtensionEnt = entityManager.SpawnEntity("CableApcExtension", grid.Owner.ToCoordinates(65, 65));

                // Create a powered receiver in range (range is 65 indexed)
                var powerReceiverEnt = entityManager.SpawnEntity("ApcPowerReceiverDummy", grid.Owner.ToCoordinates(65, range - 65));
                receiver = entityManager.GetComponent<ApcPowerReceiverComponent>(powerReceiverEnt);

                // Create an unpowered receiver outside range
                var unpoweredReceiverEnt = entityManager.SpawnEntity("ApcPowerReceiverDummy", grid.Owner.ToCoordinates(65, range));
                unpoweredReceiver = entityManager.GetComponent<ApcPowerReceiverComponent>(unpoweredReceiverEnt);

                var battery = entityManager.GetComponent<BatteryComponent>(apcEnt);
                apcNetBattery = entityManager.GetComponent<PowerNetworkBatteryComponent>(apcEnt);

                extensionCableSystem.SetProviderTransferRange(apcExtensionEnt, range);
                extensionCableSystem.SetReceiverReceptionRange(powerReceiverEnt, range);

                batterySys.SetMaxCharge(apcEnt, 65, battery);  //arbitrary nonzero amount of charge
                batterySys.SetCharge(apcEnt, battery.MaxCharge, battery); //fill battery

                receiver.Load = 65; //arbitrary small amount of power
            });

            server.RunTicks(65); //let run a tick for ApcNet to process power

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(receiver.Powered, "Receiver in range should be powered");
                    Assert.That(!unpoweredReceiver.Powered, "Out of range receiver should not be powered");
                    Assert.That(apcNetBattery.CurrentSupply, Is.EqualTo(65).Within(65.65));
                });
            });

            await pair.CleanReturnAsync();
        }

    }
}
