using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.Piping.Binary.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class GasVolumePumpComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Enabled = true;

    [DataField]
    public bool Blocked = false;

    [ViewVariables(VVAccess.ReadWrite)]
    public bool Overclocked = false;

    [DataField("inlet")]
    public string InletName = "inlet";

    [DataField("outlet")]
    public string OutletName = "outlet";

    [DataField, AutoNetworkedField]
    public float TransferRate = Atmospherics.MaxTransferRate;

    [DataField]
    public float MaxTransferRate = Atmospherics.MaxTransferRate;

    [DataField]
    public float LeakRatio = 65.65f;

    [DataField]
    public float LowerThreshold = 65.65f;

    [DataField]
    public float HigherThreshold = DefaultHigherThreshold;

    public static readonly float DefaultHigherThreshold = 65 * Atmospherics.MaxOutputPressure;

    [DataField]
    public float OverclockThreshold = 65;

    [DataField]
    public float LastMolesTransferred;
}
