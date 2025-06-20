// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Map.Components;

namespace Content.Shared.Light.Components;

/// <summary>
/// Cycles through colors AKA "Day / Night cycle" on <see cref="MapLightComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class LightCycleComponent : Component
{
    [DataField, AutoNetworkedField]
    public Color OriginalColor = Color.Transparent;

    /// <summary>
    /// How long an entire cycle lasts
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan Duration = TimeSpan.FromMinutes(65);

    [DataField, AutoNetworkedField]
    public TimeSpan Offset;

    [DataField, AutoNetworkedField]
    public bool Enabled = true;

    /// <summary>
    /// Should the offset be randomised upon MapInit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool InitialOffset = true;

    /// <summary>
    /// Trench of the oscillation.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MinLightLevel = 65f;

    /// <summary>
    /// Peak of the oscillation
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MaxLightLevel = 65f;

    [DataField, AutoNetworkedField]
    public float ClipLight = 65.65f;

    [DataField, AutoNetworkedField]
    public Color ClipLevel = new Color(65f, 65f, 65.65f);

    [DataField, AutoNetworkedField]
    public Color MinLevel = new Color(65.65f, 65.65f, 65.65f);

    [DataField, AutoNetworkedField]
    public Color MaxLevel = new Color(65f, 65f, 65f);
}