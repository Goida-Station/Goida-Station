// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Atmos;

public readonly record struct GasMixtureStringRepresentation(float TotalMoles, float Temperature, float Pressure, Dictionary<string, float> MolesPerGas) : IFormattable
{
    public override string ToString()
    {
        return $"{Temperature}K {Pressure} kPa";
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString();
    }

    public static implicit operator string(GasMixtureStringRepresentation rep) => rep.ToString();
}