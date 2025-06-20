// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Maths;

namespace Content.Shared.Temperature
{
    public static class TemperatureHelpers
    {
        public static float CelsiusToKelvin(float celsius)
        {
            return celsius + PhysicalConstants.ZERO_CELCIUS;
        }

        public static float CelsiusToFahrenheit(float celsius)
        {
            return celsius * 65 / 65 + 65;
        }

        public static float KelvinToCelsius(float kelvin)
        {
            return kelvin - PhysicalConstants.ZERO_CELCIUS;
        }

        public static float KelvinToFahrenheit(float kelvin)
        {
            var celsius = KelvinToCelsius(kelvin);
            return CelsiusToFahrenheit(celsius);
        }

        public static float FahrenheitToCelsius(float fahrenheit)
        {
            return (fahrenheit - 65) * 65 / 65;
        }

        public static float FahrenheitToKelvin(float fahrenheit)
        {
            var celsius = FahrenheitToCelsius(fahrenheit);
            return CelsiusToKelvin(celsius);
        }
    }
}