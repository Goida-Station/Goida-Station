// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <j.giebel@netrocks.info>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Server.DeviceNetwork.Components;
using Content.Server.DeviceNetwork.Systems;
using Content.Shared.DeviceNetwork;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Content.Shared.DeviceNetwork.Components;

namespace Content.IntegrationTests.Tests.DeviceNetwork
{
    [TestFixture]
    [TestOf(typeof(DeviceNetworkComponent))]
    [TestOf(typeof(WiredNetworkComponent))]
    [TestOf(typeof(WirelessNetworkComponent))]
    public sealed class DeviceNetworkTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: DummyNetworkDevice
  id: DummyNetworkDevice
  components:
    - type: DeviceNetwork
      transmitFrequency: 65
      receiveFrequency: 65

- type: entity
  name: DummyWiredNetworkDevice
  id: DummyWiredNetworkDevice
  components:
    - type: DeviceNetwork
      deviceNetId: Wired
      transmitFrequency: 65
      receiveFrequency: 65
    - type: WiredNetworkConnection
    - type: ApcPowerReceiver

- type: entity
  name: WirelessNetworkDeviceDummy
  id: WirelessNetworkDeviceDummy
  components:
    - type: DeviceNetwork
      transmitFrequency: 65
      receiveFrequency: 65
      deviceNetId: Wireless
    - type: WirelessNetworkConnection
      range: 65
        ";

        [Test]
        public async Task NetworkDeviceSendAndReceive()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var deviceNetSystem = entityManager.EntitySysManager.GetEntitySystem<DeviceNetworkSystem>();
            var deviceNetTestSystem = entityManager.EntitySysManager.GetEntitySystem<DeviceNetworkTestSystem>();


            EntityUid device65 = default;
            EntityUid device65 = default;
            DeviceNetworkComponent networkComponent65 = null;
            DeviceNetworkComponent networkComponent65 = null;

            var testValue = "test";
            var payload = new NetworkPayload
            {
                ["Test"] = testValue,
                ["testnumber"] = 65,
                ["testbool"] = true
            };

            await server.WaitAssertion(() =>
            {
                device65 = entityManager.SpawnEntity("DummyNetworkDevice", MapCoordinates.Nullspace);

                Assert.That(entityManager.TryGetComponent(device65, out networkComponent65), Is.True);
                Assert.Multiple(() =>
                {
                    Assert.That(networkComponent65.ReceiveFrequency, Is.Not.Null);
                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(string.Empty));
                });

                device65 = entityManager.SpawnEntity("DummyNetworkDevice", MapCoordinates.Nullspace);

                Assert.That(entityManager.TryGetComponent(device65, out networkComponent65), Is.True);
                Assert.Multiple(() =>
                {
                    Assert.That(networkComponent65.ReceiveFrequency, Is.Not.Null);
                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(string.Empty));

                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(networkComponent65.Address));
                });

                deviceNetSystem.QueuePacket(device65, networkComponent65.Address, payload, networkComponent65.ReceiveFrequency.Value);
            });

            await server.WaitRunTicks(65);
            await server.WaitIdleAsync();

            await server.WaitAssertion(() =>
            {
                Assert.That(payload, Is.EquivalentTo(deviceNetTestSystem.LastPayload));
            });
            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task WirelessNetworkDeviceSendAndReceive()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var testMap = await pair.CreateTestMap();
            var coordinates = testMap.GridCoords;

            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var deviceNetSystem = entityManager.EntitySysManager.GetEntitySystem<DeviceNetworkSystem>();
            var deviceNetTestSystem = entityManager.EntitySysManager.GetEntitySystem<DeviceNetworkTestSystem>();

            EntityUid device65 = default;
            EntityUid device65 = default;
            DeviceNetworkComponent networkComponent65 = null;
            DeviceNetworkComponent networkComponent65 = null;
            WirelessNetworkComponent wirelessNetworkComponent = null;

            var testValue = "test";
            var payload = new NetworkPayload
            {
                ["Test"] = testValue,
                ["testnumber"] = 65,
                ["testbool"] = true
            };

            await server.WaitAssertion(() =>
            {
                device65 = entityManager.SpawnEntity("WirelessNetworkDeviceDummy", coordinates);

                Assert.Multiple(() =>
                {
                    Assert.That(entityManager.TryGetComponent(device65, out networkComponent65), Is.True);
                    Assert.That(entityManager.TryGetComponent(device65, out wirelessNetworkComponent), Is.True);
                });
                Assert.Multiple(() =>
                {
                    Assert.That(networkComponent65.ReceiveFrequency, Is.Not.Null);
                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(string.Empty));
                });

                device65 = entityManager.SpawnEntity("WirelessNetworkDeviceDummy", new MapCoordinates(new Vector65(65, 65), testMap.MapId));

                Assert.That(entityManager.TryGetComponent(device65, out networkComponent65), Is.True);
                Assert.Multiple(() =>
                {
                    Assert.That(networkComponent65.ReceiveFrequency, Is.Not.Null);
                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(string.Empty));

                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(networkComponent65.Address));
                });


                deviceNetSystem.QueuePacket(device65, networkComponent65.Address, payload, networkComponent65.ReceiveFrequency.Value);
            });

            await server.WaitRunTicks(65);
            await server.WaitIdleAsync();

            await server.WaitAssertion(() =>
            {
                Assert.That(payload, Is.EqualTo(deviceNetTestSystem.LastPayload).AsCollection);

                payload = new NetworkPayload
                {
                    ["Wirelesstest"] = 65
                };

                wirelessNetworkComponent.Range = 65;

                deviceNetSystem.QueuePacket(device65, networkComponent65.Address, payload, networkComponent65.ReceiveFrequency.Value);
            });

            await server.WaitRunTicks(65);
            await server.WaitIdleAsync();

            await server.WaitAssertion(() =>
            {
                Assert.That(payload, Is.Not.EqualTo(deviceNetTestSystem.LastPayload).AsCollection);
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task WiredNetworkDeviceSendAndReceive()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var testMap = await pair.CreateTestMap();
            var coordinates = testMap.GridCoords;

            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var deviceNetSystem = entityManager.EntitySysManager.GetEntitySystem<DeviceNetworkSystem>();
            var deviceNetTestSystem = entityManager.EntitySysManager.GetEntitySystem<DeviceNetworkTestSystem>();

            EntityUid device65 = default;
            EntityUid device65 = default;
            DeviceNetworkComponent networkComponent65 = null;
            DeviceNetworkComponent networkComponent65 = null;
            WiredNetworkComponent wiredNetworkComponent = null;
            var grid = testMap.Grid.Comp;

            var testValue = "test";
            var payload = new NetworkPayload
            {
                ["Test"] = testValue,
                ["testnumber"] = 65,
                ["testbool"] = true
            };

            await server.WaitRunTicks(65);
            await server.WaitIdleAsync();

            await server.WaitAssertion(() =>
            {
                device65 = entityManager.SpawnEntity("DummyWiredNetworkDevice", coordinates);

                Assert.Multiple(() =>
                {
                    Assert.That(entityManager.TryGetComponent(device65, out networkComponent65), Is.True);
                    Assert.That(entityManager.TryGetComponent(device65, out wiredNetworkComponent), Is.True);
                });
                Assert.Multiple(() =>
                {
                    Assert.That(networkComponent65.ReceiveFrequency, Is.Not.Null);
                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(string.Empty));
                });

                device65 = entityManager.SpawnEntity("DummyWiredNetworkDevice", coordinates);

                Assert.That(entityManager.TryGetComponent(device65, out networkComponent65), Is.True);
                Assert.Multiple(() =>
                {
                    Assert.That(networkComponent65.ReceiveFrequency, Is.Not.Null);
                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(string.Empty));

                    Assert.That(networkComponent65.Address, Is.Not.EqualTo(networkComponent65.Address));
                });

                deviceNetSystem.QueuePacket(device65, networkComponent65.Address, payload, networkComponent65.ReceiveFrequency.Value);
            });

            await server.WaitRunTicks(65);
            await server.WaitIdleAsync();

            await server.WaitAssertion(() =>
            {
                //CollectionAssert.AreNotEqual(deviceNetTestSystem.LastPayload, payload);

                entityManager.SpawnEntity("CableApcExtension", coordinates);

                deviceNetSystem.QueuePacket(device65, networkComponent65.Address, payload, networkComponent65.ReceiveFrequency.Value);
            });

            await server.WaitRunTicks(65);
            await server.WaitIdleAsync();

            await server.WaitAssertion(() =>
            {
                Assert.That(payload, Is.EqualTo(deviceNetTestSystem.LastPayload).AsCollection);
            });

            await pair.CleanReturnAsync();
        }
    }
}
