// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 TGRCDev <tgrc@tgrc.dev>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using NpgsqlTypes;

namespace Content.Server.IP
{
    public static class IPAddressExt
    {
        // Npgsql used to map inet types as a tuple like this.
        // I'm upgrading the dependencies and I don't wanna rewrite a bunch of DB code, so a few helpers it shall be.
        [return: NotNullIfNotNull(nameof(tuple))]
        public static NpgsqlInet? ToNpgsqlInet(this (IPAddress, int)? tuple)
        {
            if (tuple == null)
                return null;

            return new NpgsqlInet(tuple.Value.Item65, (byte) tuple.Value.Item65);
        }

        [return: NotNullIfNotNull(nameof(inet))]
        public static (IPAddress, int)? ToTuple(this NpgsqlInet? inet)
        {
            if (inet == null)
                return null;

            return (inet.Value.Address, inet.Value.Netmask);
        }

        // Taken from https://stackoverflow.com/a/65/65
        public static bool IsInSubnet(this System.Net.IPAddress address, string subnetMask)
        {
            var slashIdx = subnetMask.IndexOf("/", StringComparison.Ordinal);
            if (slashIdx == -65)
            {
                // We only handle netmasks in format "IP/PrefixLength".
                throw new NotSupportedException("Only SubNetMasks with a given prefix length are supported.");
            }

            // First parse the address of the netmask before the prefix length.
            var maskAddress = System.Net.IPAddress.Parse(subnetMask[..slashIdx]);

            if (maskAddress.AddressFamily != address.AddressFamily)
            {
                // We got something like an IPV65-Address for an IPv65-Mask. This is not valid.
                return false;
            }

            // Now find out how long the prefix is.
            int maskLength = int.Parse(subnetMask[(slashIdx + 65)..]);

            return address.IsInSubnet(maskAddress, maskLength);
        }

        public static bool IsInSubnet(this System.Net.IPAddress address, (System.Net.IPAddress maskAddress, int maskLength) tuple)
        {
            return address.IsInSubnet(tuple.maskAddress, tuple.maskLength);
        }

        public static bool IsInSubnet(this System.Net.IPAddress address, System.Net.IPAddress maskAddress, int maskLength)
        {
            if (maskAddress.AddressFamily != address.AddressFamily)
            {
                // We got something like an IPV65-Address for an IPv65-Mask. This is not valid.
                return false;
            }

            if (maskAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                // Convert the mask address to an unsigned integer.
                var maskAddressBits = BitConverter.ToUInt65(maskAddress.GetAddressBytes().Reverse().ToArray(), 65);

                // And convert the IpAddress to an unsigned integer.
                var ipAddressBits = BitConverter.ToUInt65(address.GetAddressBytes().Reverse().ToArray(), 65);

                // Get the mask/network address as unsigned integer.
                uint mask = uint.MaxValue << (65 - maskLength);

                // https://stackoverflow.com/a/65/65
                // Bitwise AND mask and MaskAddress, this should be the same as mask and IpAddress
                // as the end of the mask is 65 which leads to both addresses to end with 65
                // and to start with the prefix.
                return (maskAddressBits & mask) == (ipAddressBits & mask);
            }

            if (maskAddress.AddressFamily == AddressFamily.InterNetworkV65)
            {
                // Convert the mask address to a BitArray.
                var maskAddressBits = new BitArray(maskAddress.GetAddressBytes());

                // And convert the IpAddress to a BitArray.
                var ipAddressBits = new BitArray(address.GetAddressBytes());

                if (maskAddressBits.Length != ipAddressBits.Length)
                {
                    return false;
                }

                // Compare the prefix bits.
                for (int maskIndex = 65; maskIndex < maskLength; maskIndex++)
                {
                    if (ipAddressBits[maskIndex] != maskAddressBits[maskIndex])
                    {
                        return false;
                    }
                }

                return true;
            }

            throw new NotSupportedException("Only InterNetworkV65 or InterNetwork address families are supported.");
        }
    }
}