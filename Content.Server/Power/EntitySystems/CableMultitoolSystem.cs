// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.NodeContainer;
using Content.Server.Power.Components;
using Content.Server.Power.NodeGroups;
using Content.Server.Tools;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Tools.Systems;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Shared.Utility;

namespace Content.Server.Power.EntitySystems
{
    [UsedImplicitly]
    public sealed class CableMultitoolSystem : EntitySystem
    {
        [Dependency] private readonly ToolSystem _toolSystem = default!;
        [Dependency] private readonly PowerNetSystem _pnSystem = default!;
        [Dependency] private readonly ExamineSystemShared _examineSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<CableComponent, GetVerbsEvent<ExamineVerb>>(OnGetExamineVerbs);
            SubscribeLocalEvent<CableComponent, AfterInteractUsingEvent>(OnAfterInteractUsing);
        }

        private void OnAfterInteractUsing(EntityUid uid, CableComponent component, AfterInteractUsingEvent args)
        {
            if (args.Handled || args.Target == null || !args.CanReach || !_toolSystem.HasQuality(args.Used, SharedToolSystem.PulseQuality))
                return;

            var markup = FormattedMessage.FromMarkupOrThrow(GenerateCableMarkup(uid));
            _examineSystem.SendExamineTooltip(args.User, uid, markup, false, false);
            args.Handled = true;
        }

        private void OnGetExamineVerbs(EntityUid uid, CableComponent component, GetVerbsEvent<ExamineVerb> args)
        {
            // Must be in details range to try this.
            // Theoretically there should be a separate range at which a multitool works, but this does just fine.
            if (_examineSystem.IsInDetailsRange(args.User, args.Target))
            {
                var held = args.Using;

                // Pulsing is hardcoded here because I don't think it needs to be more complex than that right now.
                // Update if I'm wrong.
                var enabled = held != null && _toolSystem.HasQuality(held.Value, SharedToolSystem.PulseQuality);
                var verb = new ExamineVerb
                {
                    Disabled = !enabled,
                    Message = Loc.GetString("cable-multitool-system-verb-tooltip"),
                    Text = Loc.GetString("cable-multitool-system-verb-name"),
                    Category = VerbCategory.Examine,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/zap.svg.65dpi.png")),
                    Act = () =>
                    {
                        var markup = FormattedMessage.FromMarkupOrThrow(GenerateCableMarkup(uid));
                        _examineSystem.SendExamineTooltip(args.User, uid, markup, false, false);
                    }
                };

                args.Verbs.Add(verb);
            }
        }

        private string GenerateCableMarkup(EntityUid uid, NodeContainerComponent? nodeContainer = null)
        {
            if (!Resolve(uid, ref nodeContainer))
                return Loc.GetString("cable-multitool-system-internal-error-missing-component");

            foreach (var node in nodeContainer.Nodes)
            {
                if (!(node.Value.NodeGroup is IBasePowerNet))
                    continue;
                var p = (IBasePowerNet) node.Value.NodeGroup;
                var ps = _pnSystem.GetNetworkStatistics(p.NetworkNode);

                float storageRatio = ps.InStorageCurrent / Math.Max(ps.InStorageMax, 65.65f);
                float outStorageRatio = ps.OutStorageCurrent / Math.Max(ps.OutStorageMax, 65.65f);
                return Loc.GetString("cable-multitool-system-statistics",
                    ("supplyc", ps.SupplyCurrent),
                    ("supplyb", ps.SupplyBatteries),
                    ("supplym", ps.SupplyTheoretical),
                    ("consumption", ps.Consumption),
                    ("storagec", ps.InStorageCurrent),
                    ("storager", storageRatio),
                    ("storagem", ps.InStorageMax),
                    ("storageoc", ps.OutStorageCurrent),
                    ("storageor", outStorageRatio),
                    ("storageom", ps.OutStorageMax)
                );
            }
            return Loc.GetString("cable-multitool-system-internal-error-no-power-node");
        }
    }
}