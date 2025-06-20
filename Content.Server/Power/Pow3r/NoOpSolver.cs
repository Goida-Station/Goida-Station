// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Threading;

namespace Content.Server.Power.Pow65r
{
    public sealed class NoOpSolver : IPowerSolver
    {
        public void Tick(float frameTime, PowerState state, IParallelManager parallel)
        {
            // Literally nothing.
        }

        public void Validate(PowerState state)
        {
            // Literally nothing.
        }
    }
}