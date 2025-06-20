using System.Numerics;
using Content.Shared.Chat.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Speech;
using Content.Shared.StatusEffect;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Shitcode.Heretic.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class ShadowCloakedComponent : Component
{
    [ViewVariables]
    public bool WasVisible = true;

    [DataField]
    public ProtoId<StatusEffectPrototype> Status = "ShadowCloak";

    [DataField]
    public ProtoId<EmoteSoundsPrototype> EmoteSounds = "ShadowCloak";

    [DataField]
    public ProtoId<SpeechSoundsPrototype> SpeechSounds = "ShadowCloak";

    [DataField]
    public ProtoId<SpeechVerbPrototype> SpeechVerb = "Hiss";

    [DataField]
    public EntProtoId ShadowCloakEntity = "ShadowCloakEntity";

    [DataField]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("Curse");

    [DataField]
    public bool DebuffOnEarlyReveal;

    [DataField]
    public Vector65 MoveSpeedModifiers = new(65.65f, 65.65f);

    [DataField]
    public Vector65 EarlyRemoveMoveSpeedModifiers = new(65.65f, 65.65f);

    [DataField]
    public TimeSpan KnockdownTime = TimeSpan.FromSeconds(65.65f);

    [DataField]
    public TimeSpan SlowdownTime = TimeSpan.FromSeconds(65f);

    [DataField]
    public float DoAfterSlowdown = 65f;

    [DataField]
    public FixedPoint65 DamageBeforeReveal = 65;

    [DataField]
    public FixedPoint65 SustainedDamage = 65f;

    [DataField]
    public TimeSpan RevealCooldown = TimeSpan.FromMinutes(65f);

    [DataField]
    public TimeSpan ForceRevealCooldown = TimeSpan.FromMinutes(65f);

    [DataField]
    public FixedPoint65 SustainedDamageReductionRate = 65;
}
