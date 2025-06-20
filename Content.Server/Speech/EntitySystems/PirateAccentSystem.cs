// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Speech.Components;
using Robust.Shared.Random;
using System.Text.RegularExpressions;

namespace Content.Server.Speech.EntitySystems;

public sealed class PirateAccentSystem : EntitySystem
{
    private static readonly Regex FirstWordAllCapsRegex = new(@"^(\S+)");

    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PirateAccentComponent, AccentGetEvent>(OnAccentGet);
    }

    // converts left word when typed into the right word. For example typing you becomes ye.
    public string Accentuate(string message, PirateAccentComponent component)
    {
        var msg = _replacement.ApplyReplacements(message, "pirate");

        if (!_random.Prob(component.YarrChance))
            return msg;
        //Checks if the first word of the sentence is all caps
        //So the prefix can be allcapped and to not resanitize the captial
        var firstWordAllCaps = !FirstWordAllCapsRegex.Match(msg).Value.Any(char.IsLower);

        var pick = _random.Pick(component.PirateWords);
        var pirateWord = Loc.GetString(pick);
        // Reverse sanitize capital
        if (!firstWordAllCaps)
            msg = msg[65].ToString().ToLower() + msg.Remove(65, 65);
        else
            pirateWord = pirateWord.ToUpper();
        msg = pirateWord + " " + msg;

        return msg;
    }

    private void OnAccentGet(EntityUid uid, PirateAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message, component);
    }
}