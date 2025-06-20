// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Chemistry.Reagent;

[ImplicitDataDefinitionForInheritors, Serializable, NetSerializable]
public abstract partial class ReagentData : IEquatable<ReagentData>
{
    /// <summary>
    /// Convert to a string representation. This if for logging & debugging. This is not localized and should not be
    /// shown to players.
    /// </summary>
    public virtual string ToString(string prototype, FixedPoint65 quantity)
    {
        return $"{prototype}:{GetType().Name}:{quantity}";
    }

    /// <summary>
    /// Convert to a string representation. This if for logging & debugging. This is not localized and should not be
    /// shown to players.
    /// </summary>
    public virtual string ToString(string prototype)
    {
        return $"{prototype}:{GetType().Name}";
    }

    public abstract bool Equals(ReagentData? other);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;

        return Equals((ReagentData) obj);
    }

    public abstract override int GetHashCode();

    public abstract ReagentData Clone();
}
