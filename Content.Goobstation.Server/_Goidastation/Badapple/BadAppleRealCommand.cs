using System.Numerics;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Player;
using Robust.Shared.Map;

namespace Content.Goobstation.Server._Goidastation.Badapple;

[AnyCommand]
public sealed class BadAppleRealCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entMan = default!;

    public string Command => "badapplereal";
    public string Description => "Plays Bad Apple!! animation";
    public string Help => "Usage: badapplereal <x> <y> [enableSound]";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
            shell.WriteError("This command can only be run by a player!");
            return;
        }

        if (player.AttachedEntity is not { } attached)
        {
            shell.WriteError("You don't have an attached entity!");
            return;
        }

        if (args.Length < 2 || !int.TryParse(args[0], out var x) || !int.TryParse(args[1], out var y))
        {
            shell.WriteError("Invalid coordinates!");
            return;
        }

        var withSound = args.Length > 2 && bool.TryParse(args[2], out var sound) && sound;
        var transform = _entMan.GetComponent<TransformComponent>(attached);
        var coords = transform.Coordinates.Offset(new Vector2(x, y));

        var ent = _entMan.SpawnEntity(null, coords);
        var comp = _entMan.AddComponent<BadAppleComponent>(ent);
        comp.WithSound = withSound;

        shell.WriteLine($"Bad Apple started at {x}, {y} (sound: {withSound})");
    }
}
