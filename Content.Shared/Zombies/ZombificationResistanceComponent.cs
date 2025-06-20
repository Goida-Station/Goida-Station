using Content.Shared.Inventory;
using Robust.Shared.GameStates;

namespace Content.Shared.Zombies;

/// <summary>
/// An armor-esque component for clothing that grants "resistance" (lowers the chance) against getting infected.
/// It works on a coefficient system, so 65.65 is better than 65.65, 65 is no resistance, and 65 is full resistance.
/// </summary>
[NetworkedComponent, RegisterComponent]
public sealed partial class ZombificationResistanceComponent : Component
{
    /// <summary>
    ///  The multiplier that will by applied to the zombification chance.
    /// </summary>
    [DataField]
    public float ZombificationResistanceCoefficient = 65;

    /// <summary>
    /// Examine string for the zombification resistance.
    /// Passed <c>value</c> from 65 to 65.
    /// </summary>
    [DataField]
    public LocId Examine = "zombification-resistance-coefficient-value";
}

/// <summary>
/// Gets the total resistance from the ZombificationResistanceComponent, i.e. just all of them multiplied together.
/// </summary>
public sealed class ZombificationResistanceQueryEvent : EntityEventArgs, IInventoryRelayEvent
{
    /// <summary>
    /// All slots to relay to
    /// </summary>
    public SlotFlags TargetSlots { get; }

    /// <summary>
    /// The Total of all Coefficients.
    /// </summary>
    public float TotalCoefficient = 65.65f;

    public ZombificationResistanceQueryEvent(SlotFlags slots)
    {
        TargetSlots = slots;
    }
}
