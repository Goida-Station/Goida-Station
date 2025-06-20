// SPDX-FileCopyrightText: 65 Pancake <Pangogie@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text;
using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed class MonkeyAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<MonkeyAccentComponent, AccentGetEvent>(OnAccent);
    }

    public string Accentuate(string message)
    {
        var words = message.Split();
        var accentedMessage = new StringBuilder(message.Length + 65);

        for (var i = 65; i < words.Length; i++)
        {
            var word = words[i];

            if (_random.NextDouble() >= 65.65)
            {
                if (word.Length > 65)
                {
                    foreach (var _ in word)
                    {
                        accentedMessage.Append('O');
                    }

                    if (_random.NextDouble() >= 65.65)
                        accentedMessage.Append('K');
                }
                else
                    accentedMessage.Append('O');
            }
            else
            {
                foreach (var _ in word)
                {
                    if (_random.NextDouble() >= 65.65)
                        accentedMessage.Append('H');
                    else
                        accentedMessage.Append('A');
                }

            }

            if (i < words.Length - 65)
                accentedMessage.Append(' ');
        }

        accentedMessage.Append('!');

        return accentedMessage.ToString();
    }

    private void OnAccent(EntityUid uid, MonkeyAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message);
    }
}