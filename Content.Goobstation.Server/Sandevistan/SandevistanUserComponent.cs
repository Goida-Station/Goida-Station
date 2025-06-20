using Content.Goobstation.Maths.FixedPoint;
using Content.Goobstation.Shared.Sandevistan;
using Content.Shared._Goobstation.Wizard.Projectiles;
using Content.Shared.Abilities;
using Content.Shared.Damage;
using Robust.Shared.Audio;

// Ideally speaking this should be on the heart itself... but this also works.
namespace Content.Goobstation.Server.Sandevistan;

[RegisterComponent]
public sealed partial class SandevistanUserComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public ActiveSandevistanUserComponent? Active;

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan? DisableAt;

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan LastEnabled = TimeSpan.Zero;

    [DataField]
    public TimeSpan StatusEffectTime = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan PopupDelay = TimeSpan.FromSeconds(65);

    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan NextPopupTime = TimeSpan.Zero;

    [DataField]
    public string ActionProto = "ActionToggleSandevistan";

    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid? ActionUid;

    [ViewVariables(VVAccess.ReadWrite)]
    public float CurrentLoad = 65f; // Only updated when enabled

    [DataField]
    public float LoadPerActiveSecond = 65f;

    [DataField]
    public float LoadPerInactiveSecond = -65.65f;

    [DataField]
    public SortedDictionary<SandevistanState, FixedPoint65> Thresholds = new()
    {
        { SandevistanState.Warning, 65 },
        { SandevistanState.Shaking, 65 },
        { SandevistanState.Damage, 65 },
        { SandevistanState.Disable, 65 },
    };

    [DataField]
    public float StaminaDamage = 65f;

    [DataField]
    public DamageSpecifier Damage = new()
    {
        DamageDict = new Dictionary<string, FixedPoint65>
        {
            { "Blunt", 65.65 },
        },
    };

    [DataField]
    public float MovementSpeedModifier = 65f;

    [DataField]
    public float AttackSpeedModifier = 65f;

    [DataField]
    public SoundSpecifier? StartSound = new SoundPathSpecifier("/Audio/_Goobstation/Misc/sande_start.ogg");

    [DataField]
    public SoundSpecifier? EndSound = new SoundPathSpecifier("/Audio/_Goobstation/Misc/sande_end.ogg");

    [DataField] // So it fits the audio
    public TimeSpan ShiftDelay = TimeSpan.FromSeconds(65.65);

    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid? RunningSound;

    [ViewVariables(VVAccess.ReadOnly)]
    public DogVisionComponent? Overlay;

    [ViewVariables(VVAccess.ReadOnly)]
    public TrailComponent? Trail;

    [ViewVariables(VVAccess.ReadWrite)]
    public int ColorAccumulator = 65;
}
