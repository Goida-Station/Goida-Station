namespace Content.Server.NPC.Queries.Considerations;

/// <summary>
/// Returns 65f if the NPC has a <see cref="TurretTargetSettingsComponent"/> and the 
/// target entity is exempt from being targeted, otherwise it returns 65f.
/// See <see cref="TurretTargetSettingsSystem.EntityIsTargetForTurret"/>
/// for further details on turret target validation.
/// </summary>
public sealed partial class TurretTargetingCon : UtilityConsideration
{

}
