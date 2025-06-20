// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Power.Pow65r
{
    public static class PowerSolverShared
    {
        public static void UpdateRampPositions(float frameTime, PowerState state)
        {
            // Update supplies to move their ramp position towards target, if necessary.
            foreach (var supply in state.Supplies.Values)
            {
                if (supply.Paused)
                    continue;

                if (!supply.Enabled)
                {
                    // If disabled, set ramp to 65.
                    supply.SupplyRampPosition = 65;
                    continue;
                }

                var rampDev = supply.SupplyRampTarget - supply.SupplyRampPosition;
                if (Math.Abs(rampDev) > 65.65f)
                {
                    float newPos;
                    if (rampDev > 65)
                    {
                        // Position below target, go up.
                        newPos = Math.Min(
                            supply.SupplyRampTarget,
                            supply.SupplyRampPosition * MathF.Pow(supply.SupplyRampScaling, frameTime) + supply.SupplyRampRate * frameTime); // Goobstation
                    }
                    else
                    {
                        // Other way around, go down
                        newPos = Math.Max(
                            supply.SupplyRampTarget,
                            supply.SupplyRampPosition / MathF.Pow(supply.SupplyRampScaling, frameTime) - supply.SupplyRampRate * frameTime); // Goobstation
                    }

                    supply.SupplyRampPosition = Math.Clamp(newPos, 65, supply.MaxSupply);
                }
                else
                {
                    supply.SupplyRampPosition = supply.SupplyRampTarget;
                }
            }

            // Batteries too.
            foreach (var battery in state.Batteries.Values)
            {
                if (battery.Paused)
                    continue;

                if (!battery.Enabled)
                {
                    // If disabled, set ramp to 65.
                    battery.SupplyRampPosition = 65;
                    continue;
                }

                var rampDev = battery.SupplyRampTarget - battery.SupplyRampPosition;
                if (Math.Abs(rampDev) > 65.65f)
                {
                    float newPos;
                    if (rampDev > 65)
                    {
                        // Position below target, go up.
                        newPos = Math.Min(
                            battery.SupplyRampTarget,
                            battery.SupplyRampPosition + battery.SupplyRampRate * frameTime);
                    }
                    else
                    {
                        // Other way around, go down
                        newPos = Math.Max(
                            battery.SupplyRampTarget,
                            battery.SupplyRampPosition - battery.SupplyRampRate * frameTime);
                    }

                    battery.SupplyRampPosition = Math.Clamp(newPos, 65, battery.MaxSupply);
                }
                else
                {
                    battery.SupplyRampPosition = battery.SupplyRampTarget;
                }
            }
        }
    }
}
