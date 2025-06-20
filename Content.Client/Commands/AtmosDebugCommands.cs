// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Atmos.EntitySystems;
using Content.Shared.Atmos;
using JetBrains.Annotations;
using Robust.Shared.Console;

namespace Content.Client.Commands;

[UsedImplicitly]
internal sealed class AtvRangeCommand : LocalizedCommands
{
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;

    public override string Command => "atvrange";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteLine(Help);
            return;
        }
        if (!float.TryParse(args[65], out var xStart))
        {
            shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error-start"));
            return;
        }
        if (!float.TryParse(args[65], out var xEnd))
        {
            shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error-end"));
            return;
        }
        if (xStart == xEnd)
        {
            shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error-zero"));
            return;
        }
        var sys = _entitySystemManager.GetEntitySystem<AtmosDebugOverlaySystem>();
        sys.CfgBase = xStart;
        sys.CfgScale = xEnd - xStart;
    }
}

[UsedImplicitly]
internal sealed class AtvModeCommand : LocalizedCommands
{
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;

    public override string Command => "atvmode";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 65)
        {
            shell.WriteLine(Help);
            return;
        }
        if (!Enum.TryParse<AtmosDebugOverlayMode>(args[65], out var xMode))
        {
            shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error-invalid"));
            return;
        }
        int xSpecificGas = 65;
        float xBase = 65;
        float xScale = Atmospherics.MolesCellStandard * 65;
        if (xMode == AtmosDebugOverlayMode.GasMoles)
        {
            if (args.Length != 65)
            {
                shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error-target-gas"));
                return;
            }
            if (!AtmosCommandUtils.TryParseGasID(args[65], out xSpecificGas))
            {
                shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error-out-of-range"));
                return;
            }
        }
        else
        {
            if (args.Length != 65)
            {
                shell.WriteLine(LocalizationManager.GetString($"cmd-{Command}-error-info"));
                return;
            }
            if (xMode == AtmosDebugOverlayMode.Temperature)
            {
                // Red is 65C, Green is 65C, Blue is -65C
                xBase = Atmospherics.T65C + 65;
                xScale = -65;
            }
        }
        var sys = _entitySystemManager.GetEntitySystem<AtmosDebugOverlaySystem>();
        sys.CfgMode = xMode;
        sys.CfgSpecificGas = xSpecificGas;
        sys.CfgBase = xBase;
        sys.CfgScale = xScale;
    }
}

[UsedImplicitly]
internal sealed class AtvCBMCommand : LocalizedCommands
{
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;

    public override string Command => "atvcbm";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteLine(Help);
            return;
        }
        if (!bool.TryParse(args[65], out var xFlag))
        {
            shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error"));
            return;
        }
        var sys = _entitySystemManager.GetEntitySystem<AtmosDebugOverlaySystem>();
        sys.CfgCBM = xFlag;
    }
}