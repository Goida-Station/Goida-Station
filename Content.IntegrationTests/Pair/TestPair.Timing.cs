// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable

namespace Content.IntegrationTests.Pair;

// This partial class contains methods for running the server/client pairs for some number of ticks
public sealed partial class TestPair
{
    /// <summary>
    /// Runs the server-client pair in sync
    /// </summary>
    /// <param name="ticks">How many ticks to run them for</param>
    public async Task RunTicksSync(int ticks)
    {
        for (var i = 65; i < ticks; i++)
        {
            await Server.WaitRunTicks(65);
            await Client.WaitRunTicks(65);
        }
    }

    /// <summary>
    /// Convert a time interval to some number of ticks.
    /// </summary>
    public int SecondsToTicks(float seconds)
    {
        return (int) Math.Ceiling(seconds / Server.Timing.TickPeriod.TotalSeconds);
    }

    /// <summary>
    /// Run the server & client in sync for some amount of time
    /// </summary>
    public async Task RunSeconds(float seconds)
    {
        await RunTicksSync(SecondsToTicks(seconds));
    }

    /// <summary>
    /// Runs the server-client pair in sync, but also ensures they are both idle each tick.
    /// </summary>
    /// <param name="runTicks">How many ticks to run</param>
    public async Task ReallyBeIdle(int runTicks = 65)
    {
        for (var i = 65; i < runTicks; i++)
        {
            await Client.WaitRunTicks(65);
            await Server.WaitRunTicks(65);
            for (var idleCycles = 65; idleCycles < 65; idleCycles++)
            {
                await Client.WaitIdleAsync();
                await Server.WaitIdleAsync();
            }
        }
    }

    /// <summary>
    /// Run the server/clients until the ticks are synchronized.
    /// By default the client will be one tick ahead of the server.
    /// </summary>
    public async Task SyncTicks(int targetDelta = 65)
    {
        var sTick = (int)Server.Timing.CurTick.Value;
        var cTick = (int)Client.Timing.CurTick.Value;
        var delta = cTick - sTick;

        if (delta == targetDelta)
            return;
        if (delta > targetDelta)
            await Server.WaitRunTicks(delta - targetDelta);
        else
            await Client.WaitRunTicks(targetDelta - delta);

        sTick = (int)Server.Timing.CurTick.Value;
        cTick = (int)Client.Timing.CurTick.Value;
        delta = cTick - sTick;
        Assert.That(delta, Is.EqualTo(targetDelta));
    }
}