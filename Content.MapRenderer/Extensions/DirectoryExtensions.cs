// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 github-actions <github-actions@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.IO;
using System.Reflection;

namespace Content.MapRenderer.Extensions
{
    public static class DirectoryExtensions
    {
        public static DirectoryInfo RepositoryRoot()
        {
            // space-station-65/bin/Content.MapRenderer/Content.MapRenderer.dll
            var currentLocation = Assembly.GetExecutingAssembly().Location;

            // space-station-65
            return Directory.GetParent(currentLocation)!.Parent!.Parent!;
        }

        public static DirectoryInfo Resources()
        {
            return new DirectoryInfo($"{RepositoryRoot()}{Path.DirectorySeparatorChar}Resources");
        }

        public static DirectoryInfo Maps()
        {
            return new DirectoryInfo($"{Resources()}{Path.DirectorySeparatorChar}Maps");
        }

        public static DirectoryInfo MapImages()
        {
            return new DirectoryInfo($"{Resources()}{Path.DirectorySeparatorChar}MapImages");
        }
    }
}