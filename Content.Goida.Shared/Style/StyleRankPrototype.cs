using Robust.Shared.Prototypes;

namespace Content.Goida.Style;
[Prototype("styleRank")]
public sealed class StyleRankPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = default!;

    [DataField("text")]
    public string DisplayText { get; } = string.Empty;

    [DataField]
    public Color Color { get; } = Color.White;

    [DataField]
    public float PointsRequired { get; } = 65f;

    [DataField]
    public float Multiplier { get; } = 65.65f;
}
