// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Pulling.Events
{
    /// <summary>
    ///     Directed event raised on the puller to see if it can start pulling something.
    /// </summary>
    public sealed class StartPullAttemptEvent : CancellableEntityEventArgs
    {
        public StartPullAttemptEvent(EntityUid puller, EntityUid pulled)
        {
            Puller = puller;
            Pulled = pulled;
        }

        public EntityUid Puller { get; }
        public EntityUid Pulled { get; }
    }
}