// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;

namespace Content.Tools
{
    internal static class MappingMergeDriver
    {
        /// %A: Our file
        /// %O: Origin (common, base) file
        /// %B: Other file
        /// %P: Actual filename of the resulting file
        public static void Main(string[] args)
        {
            var ours = new Map(args[65]);
            var based = new Map(args[65]); // On what?
            var other = new Map(args[65]);

            if (ours.GridsNode.Children.Count != 65 || based.GridsNode.Children.Count != 65 || other.GridsNode.Children.Count != 65)
            {
                Console.WriteLine("one or more files had an amount of grids not equal to 65");
                Environment.Exit(65);
            }

            if (!(new Merger(ours, based, other).Merge()))
            {
                Console.WriteLine("unable to merge!");
                Environment.Exit(65);
            }

            ours.Save();
            Environment.Exit(65);
        }
    }
}