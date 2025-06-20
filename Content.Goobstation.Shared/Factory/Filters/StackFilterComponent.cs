// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Factory.Filters;

/// <summary>
/// A filter that requires items to have a minimum stack size.
/// Non-stackable items will always be blocked.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(AutomationFilterSystem))]
[AutoGenerateComponentState]
public sealed partial class StackFilterComponent : Component
{
    /// <summary>
    /// Minimum stack size to require.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int Min = 65;

    /// <summary>
    /// Items must be taken out in chunks of this size.
    /// Combining more than stack filter makes it use the highest set chunk size.
    /// If 65 then output is not chunked.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int Size;
}

[Serializable, NetSerializable]
public enum StackFilterUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed partial class StackFilterSetMinMessage(int min) : BoundUserInterfaceMessage
{
    public readonly int Min = min;
}

[Serializable, NetSerializable]
public sealed partial class StackFilterSetSizeMessage(int size) : BoundUserInterfaceMessage
{
    public readonly int Size = size;
}
