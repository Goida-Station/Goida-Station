using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Goida.Slon;

// todo: CLEAN THIS SHIT, OVERBLOAT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!11
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SlonBossComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Aggressive = false;

    [DataField, AutoNetworkedField]
    public float LavaSpreadSpeed = 5f;

    [DataField, AutoNetworkedField]
    public int LavaMaxDistance = 15;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    public TimeSpan NextLavaAttackTime;

    [DataField, AutoNetworkedField]
    public TimeSpan LavaAttackCooldown = TimeSpan.FromSeconds(15);

    [DataField, AutoNetworkedField]
    public EntProtoId LavaPrototype = "FloorLavaEntity";

    [ViewVariables, AutoNetworkedField]
    public FixedPoint2 BaseHealth = 5000;



    [DataField, AutoNetworkedField]
    public float Anger;

    [DataField]
    public float MaxAnger = 100f;

    [DataField]
    public float AngerPerDamage = 0.1f;

    [DataField]
    public float AngerDecay = 0.2f;

    [DataField]
    public float TeleportChance = 0.05f;

    [DataField]
    public float BaseAttackCooldown = 15f;

    [DataField]
    public float MinAttackCooldown = 6f;

    [DataField]
    public float SpecialAttackChance = 0.2f;

    [DataField]
    public int LakeRadius = 2;

    [DataField]
    public float LavaLifetime = 5f;
}
