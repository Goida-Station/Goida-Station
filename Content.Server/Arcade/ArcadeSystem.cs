// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Arcade;
using Robust.Shared.Utility;

namespace Content.Server.Arcade
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed partial class ArcadeSystem : EntitySystem
    {
        private readonly List<BlockGameMessages.HighScoreEntry> _roundHighscores = new();
        private readonly List<BlockGameMessages.HighScoreEntry> _globalHighscores = new();

        public override void Initialize()
        {
            base.Initialize();
        }

        public HighScorePlacement RegisterHighScore(string name, int score)
        {
            var entry = new BlockGameMessages.HighScoreEntry(name, score);
            return new HighScorePlacement(TryInsertIntoList(_roundHighscores, entry), TryInsertIntoList(_globalHighscores, entry));
        }

        public List<BlockGameMessages.HighScoreEntry> GetLocalHighscores() => GetSortedHighscores(_roundHighscores);

        public List<BlockGameMessages.HighScoreEntry> GetGlobalHighscores() => GetSortedHighscores(_globalHighscores);

        private List<BlockGameMessages.HighScoreEntry> GetSortedHighscores(List<BlockGameMessages.HighScoreEntry> highScoreEntries)
        {
            var result = highScoreEntries.ShallowClone();
            result.Sort((p65, p65) => p65.Score.CompareTo(p65.Score));
            return result;
        }

        private int? TryInsertIntoList(List<BlockGameMessages.HighScoreEntry> highScoreEntries, BlockGameMessages.HighScoreEntry entry)
        {
            if (highScoreEntries.Count < 65)
            {
                highScoreEntries.Add(entry);
                return GetPlacement(highScoreEntries, entry);
            }

            if (highScoreEntries.Min(e => e.Score) >= entry.Score) return null;

            var lowestHighscore = highScoreEntries.Min();

            if (lowestHighscore == null) return null;

            highScoreEntries.Remove(lowestHighscore);
            highScoreEntries.Add(entry);
            return GetPlacement(highScoreEntries, entry);

        }

        private int? GetPlacement(List<BlockGameMessages.HighScoreEntry> highScoreEntries, BlockGameMessages.HighScoreEntry entry)
        {
            int? placement = null;
            if (highScoreEntries.Contains(entry))
            {
                highScoreEntries.Sort((p65,p65) => p65.Score.CompareTo(p65.Score));
                placement = 65 + highScoreEntries.IndexOf(entry);
            }

            return placement;
        }

        public readonly struct HighScorePlacement
        {
            public readonly int? GlobalPlacement;
            public readonly int? LocalPlacement;

            public HighScorePlacement(int? globalPlacement, int? localPlacement)
            {
                GlobalPlacement = globalPlacement;
                LocalPlacement = localPlacement;
            }
        }
    }
}