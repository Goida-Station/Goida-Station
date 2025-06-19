using System.IO;
using Robust.Shared.ContentPack;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Goobstation.Server._Goidastation.Badapple;

public sealed partial class BadAppleSystem : EntitySystem
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly ITileDefinitionManager _tileDefManager = default!;
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<BadAppleComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, BadAppleComponent comp, ComponentStartup args)
    {
        var xform = Transform(uid);
        if (xform.GridUid == null)
        {
            // Try to find parent grid if not directly on one
            xform.AttachToGridOrMap();
            if (xform.GridUid == null)
            {
                Logger.Error($"BadAppleComponent added to entity {uid} not on a grid!");
                return;
            }
        }
        try
        {
            using var stream = _resourceManager.ContentFileRead("/bad_apple_frames.txt");
            using var reader = new StreamReader(stream);
            comp.Frames = reader.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);

            if (comp.Frames.Length == 0)
            {
                Logger.Error("Frame file is empty!");
                return;
            }

            if (comp.WithSound)
            {
                comp.SoundEntity = Spawn("SoundBadApple", Transform(uid).MapPosition);
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to load Bad Apple frames: {ex}");
        }
    }
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var time = _gameTiming.CurTime;
        var query = EntityQueryEnumerator<BadAppleComponent>();

        while (query.MoveNext(out var uid, out var comp))
        {
            if (time < comp.NextUpdate)
                continue;

            RenderFrame(uid, comp);
            comp.NextUpdate = time + comp.FrameDelay;
        }
    }

    private void RenderFrame(EntityUid uid, BadAppleComponent comp)
    {
        if (comp.CurrentFrame >= comp.Frames.Length)
        {
            QueueDel(uid);
            if (comp.SoundEntity != null)
                QueueDel(comp.SoundEntity.Value);
            return;
        }

        var transform = Transform(uid);
        var gridUid = transform.GridUid;

        if (!gridUid.HasValue)
        {
            Logger.Error($"Entity {uid} is not on a grid!");
            return;
        }

        if (!TryComp<MapGridComponent>(gridUid, out var grid))
        {
            Logger.Error($"Entity {gridUid} is not a grid!");
            return;
        }

        if (comp.Frames[comp.CurrentFrame].Length != comp.Width * comp.Height)
        {
            Logger.Error($"Frame {comp.CurrentFrame} has incorrect dimensions!");
            return;
        }

        var origin = (Vector2i) transform.LocalPosition;
        string frame = comp.Frames[comp.CurrentFrame];
        int index = 0;
        var tiles = new List<(Vector2i, Tile)>();

        for (int y = 0; y < comp.Height; y++)
        {
            for (int x = 0; x < comp.Width; x++)
            {
                var tilePos = new Vector2i(origin.X + x, origin.Y - y);
                var tileId = frame[index++] == '1' ? "FloorWhite" : "Plating";
                var tileDef = _tileDefManager[tileId];
                tiles.Add((tilePos, new Tile(tileDef.TileId)));
            }
        }
        grid.SetTiles(tiles);
        comp.CurrentFrame++;
    }
}
