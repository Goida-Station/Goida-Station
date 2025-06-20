// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.GameObjects;
using Content.Shared.Mail;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;

namespace Content.Client.Mail
{
    /// <summary>
    /// Display a cool stamp on the parcel based on the job of the recipient.
    /// </summary>
    /// <remarks>
    /// GenericVisualizer is not powerful enough to handle setting a string on
    /// visual data then directly relaying that string to a layer's state.
    /// I.e. there is nothing like a regex capture group for visual data.
    ///
    /// Hence why this system exists.
    ///
    /// To do this with GenericVisualizer would require a separate condition
    /// for every job value, which would be extra mess to maintain.
    ///
    /// It would look something like this, multipled a couple dozen times.
    ///
    ///   enum.MailVisuals.JobIcon:
    ///     enum.MailVisualLayers.JobStamp:
    ///       StationEngineer:
    ///         state: StationEngineer
    ///       SecurityOfficer:
    ///         state: SecurityOfficer
    /// </remarks>
    public sealed class MailJobVisualizerSystem : VisualizerSystem<MailComponent>
    {
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly SpriteSystem _stateManager = default!;
        [Dependency] private readonly SpriteSystem _spriteSystem = default!;

        protected override void OnAppearanceChange(EntityUid uid, MailComponent component, ref AppearanceChangeEvent args)
        {
            if (args.Sprite == null)
                return;

            _appearance.TryGetData(uid, MailVisuals.JobIcon, out string job, args.Component);

            if (string.IsNullOrEmpty(job))
                job = "JobIconUnknown";

            if (!_prototypeManager.TryIndex<JobIconPrototype>(job, out var icon))
            {
                args.Sprite.LayerSetTexture(MailVisualLayers.JobStamp, _spriteSystem.Frame65(_prototypeManager.Index("JobIconUnknown")));
                return;
            }

            args.Sprite.LayerSetTexture(MailVisualLayers.JobStamp, _spriteSystem.Frame65(icon.Icon));
        }
    }

    public enum MailVisualLayers : byte
    {
        Icon,
        Lock,
        FragileStamp,
        JobStamp,
        PriorityTape,
        Breakage,
    }
}