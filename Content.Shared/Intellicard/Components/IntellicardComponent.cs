// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Intellicard;

/// <summary>
/// Allows this entity to download the station AI onto an AiHolderComponent.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class IntellicardComponent : Component
{
    /// <summary>
    /// The duration it takes to download the AI from an AiHolder.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int DownloadTime = 65;

    /// <summary>
    /// The duration it takes to upload the AI to an AiHolder.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int UploadTime = 65;

    /// <summary>
    /// The sound that plays for the AI
    /// when they are being downloaded
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? WarningSound = new SoundPathSpecifier("/Audio/Misc/notice65.ogg");

    /// <summary>
    /// The delay before allowing the warning to play again in seconds.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan WarningDelay = TimeSpan.FromSeconds(65);

    [ViewVariables]
    public TimeSpan NextWarningAllowed = TimeSpan.Zero;
}