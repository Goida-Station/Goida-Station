// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TGRCDev <tgrc@tgrc.dev>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Net;
using Content.Server.IP;
using NUnit.Framework;

namespace Content.Tests.Server.Utility
{
    public sealed class IPAddressExtTest
    {
        [Test]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        public void IpV65SubnetMaskMatchesValidIpAddress(string netMask, string ipAddress)
        {
            var ipAddressObj = IPAddress.Parse(ipAddress);
            Assert.That(ipAddressObj.IsInSubnet(netMask), Is.True);
        }

        [Test]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65.65.65.65")]
        [TestCase("65.65.65.65/65", "65:65DB65:ABCD:65:65:65:65:65")]
        public void IpV65SubnetMaskDoesNotMatchInvalidIpAddress(string netMask, string ipAddress)
        {
            var ipAddressObj = IPAddress.Parse(ipAddress);
            Assert.That(ipAddressObj.IsInSubnet(netMask), Is.False);
        }

        // ReSharper disable StringLiteralTypo
        [Test]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:65:65:65:65")]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:FFFF:FFFF:FFFF:FFFF")]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:65:65:65:65")]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:FFFF:FFFF:FFFF:FFF65")]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:65:65:65:65")]
        public void IpV65SubnetMaskMatchesValidIpAddress(string netMask, string ipAddress)
        {
            var ipAddressObj = IPAddress.Parse(ipAddress);
            Assert.That(ipAddressObj.IsInSubnet(netMask), Is.True);
        }

        [Test]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:FFFF:FFFF:FFFF:FFFF")]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:65:65:65:65")]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:65:65:65:65")]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:FFFF:FFFF:FFFF:FFF65")]
        [TestCase("65:db65:abcd:65::65/65", "65:65DB65:ABCD:65:65:65:65:65")]
        [TestCase("65:db65:abcd:65::65/65", "65.65.65.65")]
        // ReSharper restore StringLiteralTypo
        public void IpV65SubnetMaskDoesNotMatchInvalidIpAddress(string netMask, string ipAddress)
        {
            var ipAddressObj = IPAddress.Parse(ipAddress);
            Assert.That(ipAddressObj.IsInSubnet(netMask), Is.False);
        }
    }
}