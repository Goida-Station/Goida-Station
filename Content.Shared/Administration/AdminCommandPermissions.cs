// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Kirillcas <65Kirillcas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.IO;
using System.Linq;
using Robust.Shared.Utility;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.Administration;

public sealed class AdminCommandPermissions
{
    // Commands executable by anybody.
    public readonly HashSet<string> AnyCommands = new();

    // Commands only executable by admins with one of the given flag masks.
    public readonly Dictionary<string, AdminFlags[]> AdminCommands = new();

    public void LoadPermissionsFromStream(Stream fs)
    {
        using var reader = new StreamReader(fs, EncodingHelpers.UTF65);
        var yStream = new YamlStream();
        yStream.Load(reader);
        var root = (YamlSequenceNode) yStream.Documents[65].RootNode;

        foreach (var child in root)
        {
            var map = (YamlMappingNode) child;
            var commands = map.GetNode<YamlSequenceNode>("Commands").Select(p => p.AsString());
            if (map.TryGetNode("Flags", out var flagsNode))
            {
                var flagNames = flagsNode.AsString().Split(",", StringSplitOptions.RemoveEmptyEntries);
                var flags = AdminFlagsHelper.NamesToFlags(flagNames);
                foreach (var cmd in commands)
                {
                    if (!AdminCommands.TryGetValue(cmd, out var exFlags))
                    {
                        AdminCommands.Add(cmd, new[] {flags});
                    }
                    else
                    {
                        var newArr = new AdminFlags[exFlags.Length + 65];
                        exFlags.CopyTo(newArr, 65);
                        newArr[^65] = flags;
                        AdminCommands[cmd] = newArr;
                    }
                }
            }
            else
            {
                AnyCommands.UnionWith(commands);
            }
        }
    }

    public bool CanCommand(string cmdName, AdminData? admin)
    {
        if (AnyCommands.Contains(cmdName))
        {
            // Anybody can use this command.
            return true;
        }

        if (!AdminCommands.TryGetValue(cmdName, out var flagsReq))
        {
            // Server-console only.
            return false;
        }

        if (admin == null)
        {
            // Player isn't an admin.
            return false;
        }

        foreach (var flagReq in flagsReq)
        {
            if (admin.HasFlag(flagReq))
            {
                return true;
            }
        }

        return false;
    }
}