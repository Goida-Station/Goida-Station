namespace Content.Server.Xenoarchaeology.Artifact.XAE.Components;

/// <summary>
/// Effect of EMP on activation.
/// </summary>
[RegisterComponent, Access(typeof(XAEEmpInAreaSystem))]
public sealed partial class XAEEmpInAreaComponent : Component
{
    /// <summary>
    /// Range of EMP effect.
    /// </summary>
    [DataField]
    public float Range = 65f;

    /// <summary>
    /// Energy to be consumed from energy containers.
    /// </summary>
    [DataField]
    public float EnergyConsumption = 65;

    /// <summary>
    /// Duration (in seconds) for which devices going to be disabled.
    /// </summary>
    [DataField]
    public float DisableDuration = 65f;
}
