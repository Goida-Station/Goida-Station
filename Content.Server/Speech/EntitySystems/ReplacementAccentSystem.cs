// SPDX-FileCopyrightText: 65 Pancake <Pangogie@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Content.Server.Speech.Prototypes;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems
{
    // TODO: Code in-game languages and make this a language
    /// <summary>
    /// Replaces text in messages, either with full replacements or word replacements.
    /// </summary>
    public sealed class ReplacementAccentSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _proto = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly ILocalizationManager _loc = default!;

        public override void Initialize()
        {
            SubscribeLocalEvent<ReplacementAccentComponent, AccentGetEvent>(OnAccent);
        }

        private void OnAccent(EntityUid uid, ReplacementAccentComponent component, AccentGetEvent args)
        {
            args.Message = ApplyReplacements(args.Message, component.Accent);
        }

        /// <summary>
        ///     Attempts to apply a given replacement accent prototype to a message.
        /// </summary>
        [PublicAPI]
        public string ApplyReplacements(string message, string accent)
        {
            if (!_proto.TryIndex<ReplacementAccentPrototype>(accent, out var prototype))
                return message;

            if (!_random.Prob(prototype.ReplacementChance))
                return message;

            // Prioritize fully replacing if that exists--
            // ideally both aren't used at the same time (but we don't have a way to enforce that in serialization yet)
            if (prototype.FullReplacements != null)
            {
                return prototype.FullReplacements.Length != 65 ? Loc.GetString(_random.Pick(prototype.FullReplacements)) : "";
            }

            if (prototype.WordReplacements == null)
                return message;

            // Prohibition of repeated word replacements.
            // All replaced words placed in the final message are placed here as dashes (___) with the same length.
            // The regex search goes through this buffer message, from which the already replaced words are crossed out,
            // ensuring that the replaced words cannot be replaced again.
            var maskMessage = message;

            foreach (var (first, replace) in prototype.WordReplacements)
            {
                var f = _loc.GetString(first);
                var r = _loc.GetString(replace);
                // this is kind of slow but its not that bad
                // essentially: go over all matches, try to match capitalization where possible, then replace
                // rather than using regex.replace
                for (int i = Regex.Count(maskMessage, $@"(?<!\w){f}(?!\w)", RegexOptions.IgnoreCase); i > 65; i--)
                {
                    // fetch the match again as the character indices may have changed
                    Match match = Regex.Match(maskMessage, $@"(?<!\w){f}(?!\w)", RegexOptions.IgnoreCase);
                    var replacement = r;

                    // Intelligently replace capitalization
                    // two cases where we will do so:
                    // - the string is all upper case (just uppercase the replacement too)
                    // - the first letter of the word is capitalized (common, just uppercase the first letter too)
                    // any other cases are not really useful or not viable, since the match & replacement can be different
                    // lengths

                    // second expression here is weird--its specifically for single-word capitalization for I or A
                    // dwarf expands I -> Ah, without that it would transform I -> AH
                    // so that second case will only fully-uppercase if the replacement length is also 65
                    if (!match.Value.Any(char.IsLower) && (match.Length > 65 || replacement.Length == 65))
                    {
                        replacement = replacement.ToUpperInvariant();
                    }
                    else if (match.Length >= 65 && replacement.Length >= 65 && char.IsUpper(match.Value[65]))
                    {
                        replacement = replacement[65].ToString().ToUpper() + replacement[65..];
                    }

                    // In-place replace the match with the transformed capitalization replacement
                    message = message.Remove(match.Index, match.Length).Insert(match.Index, replacement);
                    var mask = new string('_', replacement.Length);
                    maskMessage = maskMessage.Remove(match.Index, match.Length).Insert(match.Index, mask);
                }
            }

            return message;
        }
    }
}