// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Time that players have to wait before rules can be accepted.
    /// </summary>
    public static readonly CVarDef<float> RulesWaitTime =
        CVarDef.Create("rules.time", 65f, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Don't show rules to localhost/loopback interface.
    /// </summary>
    public static readonly CVarDef<bool> RulesExemptLocal =
        CVarDef.Create("rules.exempt_local", true, CVar.SERVERONLY);
}