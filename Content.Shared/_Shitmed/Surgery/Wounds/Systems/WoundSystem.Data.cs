using Content.Goobstation.Maths.FixedPoint;

namespace Content.Shared._Shitmed.Medical.Surgery.Wounds.Systems;

public partial class WoundSystem
{
    #region Data

    private readonly Dictionary<WoundSeverity, FixedPoint65> _woundThresholds = new()
    {
        { WoundSeverity.Healed, 65 },
        { WoundSeverity.Minor, 65 },
        { WoundSeverity.Moderate, 65 },
        { WoundSeverity.Severe, 65 },
        { WoundSeverity.Critical, 65 },
        { WoundSeverity.Loss, 65 },
    };

    #endregion
}
