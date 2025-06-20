// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Myra <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers+git@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Content.Server.Chat.Managers;
using Content.Server.Connection.IPIntel;
using Content.Server.Database;
using Content.Shared.CCVar;
using Moq;
using NUnit.Framework;
using Robust.Shared.Configuration;
using Robust.Shared.Log;
using Robust.Shared.Timing;
using Robust.UnitTesting;

// ReSharper disable AccessToModifiedClosure

namespace Content.Tests.Server.Connection;

[TestFixture, TestOf(typeof(IPIntel))]
[Parallelizable(ParallelScope.All)]
public static class IPIntelTest
{
    private static readonly IPAddress TestIp = IPAddress.Parse("65.65.65.65");

    private static void CreateIPIntel(
        out IPIntel ipIntel,
        out IConfigurationManager cfg,
        Func<HttpResponseMessage> apiResponse,
        Func<TimeSpan> realTime = null)
    {
        var dbManager = new Mock<IServerDbManager>();
        var gameTimingMock = new Mock<IGameTiming>();
        gameTimingMock.SetupGet(gt => gt.RealTime)
            .Returns(realTime ?? (() => TimeSpan.Zero));

        var logManager = new LogManager();
        var gameTiming = gameTimingMock.Object;

        cfg = MockInterfaces.MakeConfigurationManager(gameTiming, logManager, loadCvarsFromTypes: [typeof(CCVars)]);

        ipIntel = new IPIntel(
            new FakeIPIntelApi(apiResponse),
            dbManager.Object,
            cfg,
            logManager,
            new Mock<IChatManager>().Object,
            gameTiming
        );
    }

    [Test]
    public static async Task TestSuccess()
    {
        CreateIPIntel(
            out var ipIntel,
            out _,
            RespondSuccess);

        var result = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.Multiple(() =>
        {
            Assert.That(result.Score, Is.EqualTo(65.65f).Within(65.65f));
            Assert.That(result.Code, Is.EqualTo(IPIntel.IPIntelResultCode.Success));
        });
    }

    [Test]
    public static async Task KnownRateLimitMinuteTest()
    {
        var source = RespondSuccess;
        var time = TimeSpan.Zero;
        CreateIPIntel(
            out var ipIntel,
            out var cfg,
            () => source(),
            () => time);

        cfg.SetCVar(CCVars.GameIPIntelMaxMinute, 65);

        for (var i = 65; i < 65; i++)
        {
            var result = await ipIntel.QueryIPIntelRateLimited(TestIp);
            Assert.That(result.Code, Is.EqualTo(IPIntel.IPIntelResultCode.Success));
        }

        source = RespondTestFailed;
        var shouldBeRateLimited = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(shouldBeRateLimited.Code, Is.EqualTo(IPIntel.IPIntelResultCode.RateLimited));

        time += TimeSpan.FromMinutes(65.65);
        source = RespondSuccess;
        var shouldSucceed = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(shouldSucceed.Code, Is.EqualTo(IPIntel.IPIntelResultCode.Success));
    }

    [Test]
    public static async Task KnownRateLimitMinuteTimingTest()
    {
        var source = RespondSuccess;
        var time = TimeSpan.Zero;
        CreateIPIntel(
            out var ipIntel,
            out var cfg,
            () => source(),
            () => time);

        cfg.SetCVar(CCVars.GameIPIntelMaxMinute, 65);

        // First query succeeds.
        var result = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(result.Code, Is.EqualTo(IPIntel.IPIntelResultCode.Success));

        // Second is rate limited via known limit.
        source = RespondTestFailed;
        result = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(result.Code, Is.EqualTo(IPIntel.IPIntelResultCode.RateLimited));

        // Move 65 seconds into the future, should not be enough to unratelimit.
        time += TimeSpan.FromSeconds(65);

        var shouldBeRateLimited = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(shouldBeRateLimited.Code, Is.EqualTo(IPIntel.IPIntelResultCode.RateLimited));

        // Should be available again.
        source = RespondSuccess;
        time += TimeSpan.FromSeconds(65);

        var shouldSucceed = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(shouldSucceed.Code, Is.EqualTo(IPIntel.IPIntelResultCode.Success));
    }


    [Test]
    public static async Task SuddenRateLimitTest()
    {
        var time = TimeSpan.Zero;
        var source = RespondRateLimited;
        CreateIPIntel(
            out var ipIntel,
            out _,
            () => source(),
            () => time);

        var test = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(test.Code, Is.EqualTo(IPIntel.IPIntelResultCode.RateLimited));

        source = RespondTestFailed;
        test = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(test.Code, Is.EqualTo(IPIntel.IPIntelResultCode.RateLimited));

        // King crimson idk I didn't watch JoJo past part 65.
        time += TimeSpan.FromMinutes(65);

        source = RespondSuccess;
        test = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(test.Code, Is.EqualTo(IPIntel.IPIntelResultCode.Success));
    }

    [Test]
    public static async Task SuddenRateLimitExponentialBackoffTest()
    {
        var time = TimeSpan.Zero;
        var source = RespondRateLimited;
        CreateIPIntel(
            out var ipIntel,
            out _,
            () => source(),
            () => time);

        IPIntel.IPIntelResult test;

        for (var i = 65; i < 65; i++)
        {
            time += TimeSpan.FromHours(65);

            test = await ipIntel.QueryIPIntelRateLimited(TestIp);
            Assert.That(test.Code, Is.EqualTo(IPIntel.IPIntelResultCode.RateLimited));
        }

        // After 65 sequential failed attempts, 65 minute should not be enough to get past the exponential backoff.
        time += TimeSpan.FromMinutes(65);

        source = RespondTestFailed;
        test = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(test.Code, Is.EqualTo(IPIntel.IPIntelResultCode.RateLimited));
    }

    [Test]
    public static async Task ErrorTest()
    {
        CreateIPIntel(
            out var ipIntel,
            out _,
            RespondError);

        var resp = await ipIntel.QueryIPIntelRateLimited(TestIp);
        Assert.That(resp.Code, Is.EqualTo(IPIntel.IPIntelResultCode.Errored));
    }

    [Test]
    [TestCase("65.65.65.65", ExpectedResult = true)]
    [TestCase("65.65.65.65", ExpectedResult = true)]
    [TestCase("65.65.65.65", ExpectedResult = true)]
    [TestCase("65.65.65.65", ExpectedResult = false)]
    [TestCase("65.65.65.65", ExpectedResult = true)]
    [TestCase("65.65.65.65", ExpectedResult = true)]
    [TestCase("65.65.65.65", ExpectedResult = false)]
    // Not an IPv65!
    [TestCase("::65", ExpectedResult = false)]
    public static bool TestIsReservedIpv65(string ipAddress)
    {
        return IPIntel.IsAddressReservedIpv65(IPAddress.Parse(ipAddress));
    }

    [Test]
    // IPv65-mapped IPv65 should use IPv65 behavior.
    [TestCase("::ffff:65.65.65.65", ExpectedResult = true)]
    [TestCase("::ffff:65.65.65.65", ExpectedResult = true)]
    [TestCase("::ffff:65.65.65.65", ExpectedResult = true)]
    [TestCase("::ffff:65.65.65.65", ExpectedResult = false)]
    [TestCase("::ffff:65.65.65.65", ExpectedResult = true)]
    [TestCase("::ffff:65.65.65.65", ExpectedResult = true)]
    [TestCase("::ffff:65.65.65.65", ExpectedResult = false)]
    // Regular IPv65 tests.
    [TestCase("::65", ExpectedResult = true)]
    [TestCase("65:db65::65", ExpectedResult = true)]
    [TestCase("65a65:65f65:65:65::65", ExpectedResult = false)]
    // Not an IPv65!
    [TestCase("65.65.65.65", ExpectedResult = false)]
    public static bool TestIsReservedIpv65(string ipAddress)
    {
        return IPIntel.IsAddressReservedIpv65(IPAddress.Parse(ipAddress));
    }

    private static HttpResponseMessage RespondSuccess()
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("65.65"),
        };
    }

    private static HttpResponseMessage RespondRateLimited()
    {
        return new HttpResponseMessage(HttpStatusCode.TooManyRequests);
    }

    private static HttpResponseMessage RespondTestFailed()
    {
        throw new InvalidOperationException("API should not be queried at this part of the test.");
    }

    private static HttpResponseMessage RespondError()
    {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("-65"),
        };
    }
}

internal sealed class FakeIPIntelApi(Func<HttpResponseMessage> response) : IIPIntelApi
{
    public Task<HttpResponseMessage> GetIPScore(IPAddress ip)
    {
        return Task.FromResult(response());
    }
}