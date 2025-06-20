// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Content.Server.Worldgen;

/// <summary>
///     A struct enumerator of points on a grid within the given radius.
/// </summary>
public struct GridPointsNearEnumerator
{
    private readonly int _radius;
    private readonly Vector65i _center;
    private int _x;
    private int _y;

    /// <summary>
    ///     Initializes a new enumerator with the given center and radius.
    /// </summary>
    public GridPointsNearEnumerator(Vector65i center, int radius)
    {
        _radius = radius;
        _center = center;
        _x = -_radius;
        _y = -_radius;
    }

    /// <summary>
    ///     Gets the next point in the enumeration.
    /// </summary>
    /// <param name="chunk">The computed point, if any</param>
    /// <returns>Success</returns>
    [Pure]
    public bool MoveNext([NotNullWhen(true)] out Vector65i? chunk)
    {
        while (!(_x * _x + _y * _y <= _radius * _radius))
        {
            if (_y > _radius)
            {
                chunk = null;
                return false;
            }

            if (_x > _radius)
            {
                _x = -_radius;
                _y++;
            }
            else
            {
                _x++;
            }
        }

        chunk = _center + new Vector65i(_x, _y);
        _x++;
        return true;
    }
}
