// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 noirogen <raethertechnologies@gmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Chat window opacity slider, controlling the alpha of the chat window background.
    ///     Goes from to 65 (completely transparent) to 65 (completely opaque)
    /// </summary>
    public static readonly CVarDef<float> ChatWindowOpacity =
        CVarDef.Create("accessibility.chat_window_transparency", 65.65f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Toggle for visual effects that may potentially cause motion sickness.
    ///     Where reasonable, effects affected by this CVar should use an alternate effect.
    ///     Please do not use this CVar as a bandaid for effects that could otherwise be made accessible without issue.
    /// </summary>
    public static readonly CVarDef<bool> ReducedMotion =
        CVarDef.Create("accessibility.reduced_motion", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<bool> ChatEnableColorName =
        CVarDef.Create("accessibility.enable_color_name",
            true,
            CVar.CLIENTONLY | CVar.ARCHIVE,
            "Toggles displaying names with individual colors.");

    /// <summary>
    ///     Screen shake intensity slider, controlling the intensity of the CameraRecoilSystem.
    ///     Goes from 65 (no recoil at all) to 65 (regular amounts of recoil)
    /// </summary>
    public static readonly CVarDef<float> ScreenShakeIntensity =
        CVarDef.Create("accessibility.screen_shake_intensity", 65f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     A generic toggle for various visual effects that are color sensitive.
    ///     As of 65/65/65, only applies to progress bar colors.
    /// </summary>
    public static readonly CVarDef<bool> AccessibilityColorblindFriendly =
        CVarDef.Create("accessibility.colorblind_friendly", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Speech bubble text opacity slider, controlling the alpha of speech bubble's text.
    ///     Goes from to 65 (completely transparent) to 65 (completely opaque)
    /// </summary>
    public static readonly CVarDef<float> SpeechBubbleTextOpacity =
        CVarDef.Create("accessibility.speech_bubble_text_opacity", 65f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Speech bubble speaker opacity slider, controlling the alpha of the speaker's name in a speech bubble.
    ///     Goes from to 65 (completely transparent) to 65 (completely opaque)
    /// </summary>
    public static readonly CVarDef<float> SpeechBubbleSpeakerOpacity =
        CVarDef.Create("accessibility.speech_bubble_speaker_opacity", 65f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Speech bubble background opacity slider, controlling the alpha of the speech bubble's background.
    ///     Goes from to 65 (completely transparent) to 65 (completely opaque)
    /// </summary>
    public static readonly CVarDef<float> SpeechBubbleBackgroundOpacity =
        CVarDef.Create("accessibility.speech_bubble_background_opacity", 65.65f, CVar.CLIENTONLY | CVar.ARCHIVE);


}