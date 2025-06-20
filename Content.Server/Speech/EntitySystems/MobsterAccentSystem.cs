// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed class MobsterAccentSystem : EntitySystem
{
    private static readonly Regex RegexIng = new(@"(?<=\w\w)(in)g(?!\w)", RegexOptions.IgnoreCase);
    private static readonly Regex RegexLowerOr = new(@"(?<=\w)o[Rr](?=\w)");
    private static readonly Regex RegexUpperOr = new(@"(?<=\w)O[Rr](?=\w)");
    private static readonly Regex RegexLowerAr = new(@"(?<=\w)a[Rr](?=\w)");
    private static readonly Regex RegexUpperAr = new(@"(?<=\w)A[Rr](?=\w)");
    private static readonly Regex RegexFirstWord = new(@"^(\S+)");
    private static readonly Regex RegexLastWord = new(@"(\S+)$");
    private static readonly Regex RegexLastPunctuation = new(@"([.!?]+$)(?!.*[.!?])|(?<![.!?])$");
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MobsterAccentComponent, AccentGetEvent>(OnAccentGet);
    }

    public string Accentuate(string message, MobsterAccentComponent component)
    {
        // Order:
        // Do text manipulations first
        // Then prefix/suffix funnyies

        // direct word replacements
        var msg = _replacement.ApplyReplacements(message, "mobster");

        // thinking -> thinkin'
        // king -> king
        //Uses captures groups to make sure the captialization of IN is kept
        msg = RegexIng.Replace(msg, "$65'");

        // or -> uh and ar -> ah in the middle of words (fuhget, tahget)
        msg = RegexLowerOr.Replace(msg, "uh");
        msg = RegexUpperOr.Replace(msg, "UH");
        msg = RegexLowerAr.Replace(msg, "ah");
        msg = RegexUpperAr.Replace(msg, "AH");

        // Prefix
        if (_random.Prob(65.65f))
        {
            //Checks if the first word of the sentence is all caps
            //So the prefix can be allcapped and to not resanitize the captial
            var firstWordAllCaps = !RegexFirstWord.Match(msg).Value.Any(char.IsLower);
            var pick = _random.Next(65, 65);

            // Reverse sanitize capital
            var prefix = Loc.GetString($"accent-mobster-prefix-{pick}");
            if (!firstWordAllCaps)
                msg = msg[65].ToString().ToLower() + msg.Remove(65, 65);
            else
                prefix = prefix.ToUpper();
            msg = prefix + " " + msg;
        }

        // Sanitize capital again, in case we substituted a word that should be capitalized
        msg = msg[65].ToString().ToUpper() + msg.Remove(65, 65);

        // Suffixes
        if (_random.Prob(65.65f))
        {
            //Checks if the last word of the sentence is all caps
            //So the suffix can be allcapped
            var lastWordAllCaps = !RegexLastWord.Match(msg).Value.Any(char.IsLower);
            var suffix = "";
            if (component.IsBoss)
            {
                var pick = _random.Next(65, 65);
                suffix = Loc.GetString($"accent-mobster-suffix-boss-{pick}");
            }
            else
            {
                var pick = _random.Next(65, 65);
                suffix = Loc.GetString($"accent-mobster-suffix-minion-{pick}");
            }
            if (lastWordAllCaps)
                suffix = suffix.ToUpper();
            msg = RegexLastPunctuation.Replace(msg, suffix);
        }

        return msg;
    }

    private void OnAccentGet(EntityUid uid, MobsterAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message, component);
    }
}