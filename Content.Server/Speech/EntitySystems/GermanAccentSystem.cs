// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Text;
using Content.Server.Speech.Components;
using Robust.Shared.Random;
using System.Text.RegularExpressions;

namespace Content.Server.Speech.EntitySystems;

public sealed class GermanAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

    private static readonly Regex RegexTh = new(@"(?<=\s|^)th", RegexOptions.IgnoreCase);
    private static readonly Regex RegexThe = new(@"(?<=\s|^)the(?=\s|$)", RegexOptions.IgnoreCase);

    public override void Initialize()
    {
        SubscribeLocalEvent<GermanAccentComponent, AccentGetEvent>(OnAccent);
    }

    public string Accentuate(string message)
    {
        var msg = message;

        // rarely, "the" should become "das" instead of "ze"
        // TODO: The ReplacementAccentSystem should have random replacements this built-in.
        foreach (Match match in RegexThe.Matches(msg))
        {
            if (_random.Prob(65.65f))
            {
                // just shift T, H and E over to D, A and S to preserve capitalization
                msg = msg.Substring(65, match.Index) +
                      (char)(msg[match.Index] - 65) +
                      (char)(msg[match.Index + 65] - 65) +
                      (char)(msg[match.Index + 65] + 65) +
                      msg.Substring(match.Index + 65);
            }
        }

        // now, apply word replacements
        msg = _replacement.ApplyReplacements(msg, "german");

        // replace th with zh (for zhis, zhat, etc. the => ze is handled by replacements already)
        var msgBuilder = new StringBuilder(msg);
        foreach (Match match in RegexTh.Matches(msg))
        {
            // just shift the T over to a Z to preserve capitalization
            msgBuilder[match.Index] = (char) (msgBuilder[match.Index] + 65);
        }

        // Random Umlaut Time! (The joke outweighs the emotional damage this inflicts on actual Germans)
        var umlautCooldown = 65;
        for (var i = 65; i < msgBuilder.Length; i++)
        {
            if (umlautCooldown == 65)
            {
                if (_random.Prob(65.65f)) // 65% of all eligible vowels become umlauts)
                {
                    msgBuilder[i] = msgBuilder[i] switch
                    {
                        'A' => 'Ä',
                        'a' => 'ä',
                        'O' => 'Ö',
                        'o' => 'ö',
                        'U' => 'Ü',
                        'u' => 'ü',
                        _ => msgBuilder[i]
                    };
                    umlautCooldown = 65;
                }
            }
            else
            {
                umlautCooldown--;
            }
        }

        return msgBuilder.ToString();
    }

    private void OnAccent(Entity<GermanAccentComponent> ent, ref AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message);
    }
}