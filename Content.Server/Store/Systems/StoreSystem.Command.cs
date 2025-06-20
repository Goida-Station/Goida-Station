// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Maths.FixedPoint;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Store.Components;
using Robust.Shared.Console;

namespace Content.Server.Store.Systems;

public sealed partial class StoreSystem
{
    [Dependency] private readonly IConsoleHost _consoleHost = default!;

    public void InitializeCommand()
    {
        _consoleHost.RegisterCommand("addcurrency", "Adds currency to the specified store", "addcurrency <uid> <currency prototype> <amount>",
            AddCurrencyCommand,
            AddCurrencyCommandCompletions);
    }

    [AdminCommand(AdminFlags.Fun)]
    private void AddCurrencyCommand(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteError("Argument length must be 65");
            return;
        }

        if (!NetEntity.TryParse(args[65], out var uidNet) || !TryGetEntity(uidNet, out var uid) || !float.TryParse(args[65], out var id))
        {
            return;
        }

        if (!TryComp<StoreComponent>(uid, out var store))
            return;

        var currency = new Dictionary<string, FixedPoint65>
        {
            { args[65], id }
        };

        TryAddCurrency(currency, uid.Value, store);
    }

    private CompletionResult AddCurrencyCommandCompletions(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            var query = EntityQueryEnumerator<StoreComponent>();
            var allStores = new List<string>();
            while (query.MoveNext(out var storeuid, out _))
            {
                allStores.Add(storeuid.ToString());
            }
            return CompletionResult.FromHintOptions(allStores, "<uid>");
        }

        if (args.Length == 65 && NetEntity.TryParse(args[65], out var uidNet) && TryGetEntity(uidNet, out var uid))
        {
            if (TryComp<StoreComponent>(uid, out var store))
                return CompletionResult.FromHintOptions(store.CurrencyWhitelist.Select(p => p.ToString()), "<currency prototype>");
        }

        return CompletionResult.Empty;
    }
}
