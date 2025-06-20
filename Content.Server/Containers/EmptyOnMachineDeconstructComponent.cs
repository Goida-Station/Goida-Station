// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Containers
{
    /// <summary>
    /// Empties a list of containers when the machine is deconstructed via MachineDeconstructedEvent.
    /// </summary>
    [RegisterComponent]
    public sealed partial class EmptyOnMachineDeconstructComponent : Component
    {
        [DataField("containers")]
        public HashSet<string> Containers { get; set; } = new();
    }
}