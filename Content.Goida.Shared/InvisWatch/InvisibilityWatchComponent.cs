using Content.Shared.Inventory;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Goida.InvisWatch;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class InvisibilityWatchComponent : Component
{
    [DataField, AutoNetworkedField]
    public float MaxCharge = 12f;

    [DataField, AutoNetworkedField]
    public float Charge = 12f;

    [DataField, AutoNetworkedField]
    public bool IsActive;

    [DataField, AutoNetworkedField]
    public EntityUid? User;

    [DataField]
    public float RechargeRate = 0.5f;

    [DataField]
    public float DischargeRate = 1.0f;

    [DataField, AutoNetworkedField]
    public EntityUid? ToggleActionEntity;

    [DataField]
    public EntProtoId ToggleAction = "ToggleInvisibility";

    [DataField]
    public SlotFlags SlotFlags = SlotFlags.GLOVES;
}

[Serializable, NetSerializable]
public enum StealthClockVisuals
{
    Charge,
    Active
}

[Serializable, NetSerializable]
public sealed class ToggleStealthMessage : BoundUserInterfaceMessage
{
    public bool IsActive { get; }

    public ToggleStealthMessage(bool isActive)
    {
        IsActive = isActive;
    }
}
