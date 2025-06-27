using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Goida.Useless.TV
{
    [Prototype("tvInsides")]
    public sealed class TvInsidesPrototype : IPrototype
    {
        [IdDataField]
        public string ID { get; } = default!;
        [DataField]
        public List<TvRandomImage> RandomImages = new();

        [DataField]
        public Dictionary<string, ResPath> EventImages = new();
    }

    [DataDefinition] // i have no fucking idea what is this but whatever
    public sealed partial class TvRandomImage
    {
        [DataField("image", required: true)]
        public ResPath Image = default!;

        [DataField("chance")]
        public float Chance = 0.05f;
    }
}
