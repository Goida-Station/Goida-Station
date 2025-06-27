using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Utility;
using System.Numerics;

namespace Content.Goida.Useless.TV
{
    [RegisterComponent]
    public sealed partial class TVComponent : Component
    {
        [DataField("tvIcon")]
        public ResPath TVIcon = new("/Textures/_Goida/Tv/tv.png");

        [DataField("insides", required: true,
            customTypeSerializer: typeof(PrototypeIdSerializer<TvInsidesPrototype>))]
        public string InsidesPrototype = default!;

        [DataField("screenOffset")]
        public Vector2 ScreenOffset = new(0.45f, 0.45f);

        [ViewVariables]
        public Vector2 SavedOffset;

        [ViewVariables]
        public string? CurrentImage;
    }
}
