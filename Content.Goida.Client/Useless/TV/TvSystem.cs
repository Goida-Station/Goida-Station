using System.Numerics;
using Content.Goida.Useless.TV;
using Robust.Client.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Goida.Client.Useless.TV
{
    public sealed class TvSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _prototype = default!;
        [Dependency] private readonly IRobustRandom _random = default!;

        private static readonly ResPath BlankTexture = new("/Textures/_Goida/Tv/shitpost/shitpost7.png"); // lazy bootleg fallback

        public override void Initialize()
        {
            SubscribeLocalEvent<TVComponent, ComponentStartup>(OnTVStart);
            SubscribeLocalEvent<TVComponent, ComponentShutdown>(OnTVShutdown);
        }

        private void OnTVStart(EntityUid uid, TVComponent tv, ComponentStartup args)
        {
            if (!TryComp<SpriteComponent>(uid, out var sprite) ||
                sprite.LayerMapTryGet("TVLayer", out _))
            {
                return;
            }

            var tvLayerIndex = sprite.AddLayer(new SpriteSpecifier.Texture(tv.TVIcon));
            sprite.LayerMapSet("TVLayer", tvLayerIndex);

            var offset = new Vector2(
                tv.ScreenOffset.X * (sprite.Bounds.Width / 2),
                tv.ScreenOffset.Y * (sprite.Bounds.Height / 2)
            );
            sprite.LayerSetOffset(tvLayerIndex, offset);

            tv.SavedOffset = offset;
        }

        private void OnTVShutdown(EntityUid uid, TVComponent tv, ComponentShutdown args)
        {
            if (!TryComp<SpriteComponent>(uid, out var sprite))
                return;

            if (sprite.LayerMapTryGet("TVLayer", out var tvLayerIndex))
            {
                sprite.RemoveLayer(tvLayerIndex);
            }

            if (sprite.LayerMapTryGet("ImageLayer", out var imageLayerIndex))
            {
                sprite.RemoveLayer(imageLayerIndex);
            }
        }

        public override void Update(float frameTime)
        {
            var query = EntityQueryEnumerator<TVComponent, SpriteComponent>();
            while (query.MoveNext(out var uid, out var tv, out var sprite))
            {
                if (tv.CurrentImage != null) continue;

                if (!_prototype.TryIndex<TvInsidesPrototype>(tv.InsidesPrototype, out var insides))
                    continue;

                foreach (var randomImage in insides.RandomImages)
                {
                    if (_random.Prob(randomImage.Chance * frameTime))
                    {
                        ShowImage(uid, randomImage.Image, tv, sprite);
                        break;
                    }
                }
            }
        }

        public void ShowImage(EntityUid uid, ResPath imagePath, TVComponent tv, SpriteComponent sprite)
        {
            if (!sprite.LayerMapTryGet("ImageLayer", out var layerId))
            {
                layerId = sprite.AddLayer(new SpriteSpecifier.Texture(imagePath));
                sprite.LayerMapSet("ImageLayer", layerId);

                sprite.LayerSetOffset(layerId, tv.SavedOffset);
            }
            else
            {
                sprite.LayerSetTexture(layerId, imagePath);
            }

            tv.CurrentImage = imagePath.ToString();

            Timer.Spawn(3000, () =>
            {
                if (Deleted(uid)) return;
                HideImage(uid, tv, sprite);
            });
        }

        public void HideImage(EntityUid uid, TVComponent tv, SpriteComponent sprite)
        {
            if (tv.CurrentImage == null) return;

            if (sprite.LayerMapTryGet("ImageLayer", out var layerId))
            {
                sprite.LayerSetTexture(layerId, BlankTexture);
            }

            tv.CurrentImage = null;
        }

        public void ShowEventImage(EntityUid uid, string eventId, TVComponent tv, SpriteComponent sprite)
        {
            if (!_prototype.TryIndex<TvInsidesPrototype>(tv.InsidesPrototype, out var insides) ||
                !insides.EventImages.TryGetValue(eventId, out var imagePath))
                return;

            ShowImage(uid, imagePath, tv, sprite);
        }
    }
}
