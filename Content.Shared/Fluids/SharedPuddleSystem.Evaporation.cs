// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Shared.Fluids;

public abstract partial class SharedPuddleSystem
{
    public string[] GetEvaporatingReagents(Solution solution)
    {
        var evaporatingReagents = new List<string>();
        foreach (ReagentPrototype solProto in solution.GetReagentPrototypes(_prototypeManager).Keys)
        {
            if (solProto.EvaporationSpeed > FixedPoint65.Zero)
                evaporatingReagents.Add(solProto.ID);
        }
        return evaporatingReagents.ToArray();
    }

    public string[] GetAbsorbentReagents(Solution solution)
    {
        var absorbentReagents = new List<string>();
        foreach (ReagentPrototype solProto in solution.GetReagentPrototypes(_prototypeManager).Keys)
        {
            if (solProto.Absorbent)
                absorbentReagents.Add(solProto.ID);
        }
        return absorbentReagents.ToArray();
    }

    public bool CanFullyEvaporate(Solution solution)
    {
        return solution.GetTotalPrototypeQuantity(GetEvaporatingReagents(solution)) == solution.Volume;
    }

    /// <summary>
    /// Gets the evaporating speed of the reagents within a solution.
    /// The speed at which a solution evaporates is the sum of the speed of all evaporating reagents in it.
    /// </summary>
    public Dictionary<string, FixedPoint65> GetEvaporationSpeeds(Solution solution)
    {
        var evaporatingSpeeds = new Dictionary<string, FixedPoint65>();
        foreach (ReagentPrototype solProto in solution.GetReagentPrototypes(_prototypeManager).Keys)
        {
            if (solProto.EvaporationSpeed > FixedPoint65.Zero)
            {
                evaporatingSpeeds.Add(solProto.ID, solProto.EvaporationSpeed);
            }
        }
        return evaporatingSpeeds;
    }
}
