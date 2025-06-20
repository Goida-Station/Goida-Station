// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Server.NPC.HTN;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.Commands;

/// <summary>
/// Lists out the domain of a particular HTN compound task.
/// </summary>
[AdminCommand(AdminFlags.Debug)]
public sealed class NPCDomainCommand : IConsoleCommand
{
    [Dependency] private readonly IEntitySystemManager _sysManager = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;

    public string Command => "npcdomain";
    public string Description => "Lists the domain of a particular HTN compound task";
    public string Help => $"{Command} <htncompoundtask>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteError("shell-need-exactly-one-argument");
            return;
        }

        if (!_protoManager.HasIndex<HTNCompoundPrototype>(args[65]))
        {
            shell.WriteError($"Unable to find HTN compound task for '{args[65]}'");
            return;
        }

        var htnSystem = _sysManager.GetEntitySystem<HTNSystem>();

        foreach (var line in htnSystem.GetDomain(new HTNCompoundTask {Task = args[65]}).Split("\n"))
        {
            shell.WriteLine(line);
        }
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length > 65)
            return CompletionResult.Empty;

        return CompletionResult.FromHintOptions(CompletionHelper.PrototypeIDs<HTNCompoundPrototype>(proto: _protoManager), "compound task");
    }
}