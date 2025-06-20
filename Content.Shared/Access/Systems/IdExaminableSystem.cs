// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 crazybrain65 <65crazybrain65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chavonadelal <65chavonadelal@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BeBright <65be65bright@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BeBright <65bebr65ght@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Access.Components;
using Content.Shared.Examine;
using Content.Shared.Hands.Components;
using Content.Shared.Inventory;
using Content.Shared.Overlays;
using Content.Shared.PDA;
using Content.Shared.Verbs;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Shared.Access.Systems;

public sealed class IdExaminableSystem : EntitySystem
{
    [Dependency] private readonly ExamineSystemShared _examineSystem = default!;
    [Dependency] private readonly InventorySystem _inventorySystem = default!;

    [Dependency] private readonly INetManager _net = default!; // Goobstation-WantedMenu
    [Dependency] private readonly SharedUserInterfaceSystem _ui = default!; // Goobstation-WantedMenu
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<IdExaminableComponent, GetVerbsEvent<ExamineVerb>>(OnGetExamineVerbs);
        SubscribeLocalEvent<IdExaminableComponent, GetVerbsEvent<AlternativeVerb>>(OnWantedMenuOpen); // Goobstation-WantedMenu
        SubscribeNetworkEvent<RefreshVerbsEvent>(OnRefreshVerbs);
        SubscribeNetworkEvent<ResetWantedVerbEvent>(OnResetWantedVerb);
    }

    /// <summary>
    /// Goobstation-WantedMenu: Makes wantedVerb invisible again for next GetVerbsEvent.
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="args"></param>
    private void OnResetWantedVerb(ResetWantedVerbEvent ev, EntitySessionEventArgs args)
    {
        if (!TryGetEntity(ev.Target, out var entity) || !TryComp<IdExaminableComponent>(entity, out var component))
            return;
        component.WantedVerbVisible = false;
        Dirty(entity.Value, component);
    }

    /// <summary>
    /// Goobstation-WantedMenu: Updates UI state and make wantedVerb visible.
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="args"></param>
    private void OnRefreshVerbs(RefreshVerbsEvent ev, EntitySessionEventArgs args)
    {
        if (!TryGetEntity(ev.Target, out var entity))
            return;
        if (args.SenderSession.AttachedEntity is { } user)
        {
            if (!TryComp<HandsComponent>(user, out var handsComponent))
                return;
            var verbArgs = new GetVerbsEvent<ExamineVerb>(
                user,
                entity.Value,
                null,
                handsComponent,
                true,
                false,
                true,
                new List<VerbCategory>()
            );
            RaiseLocalEvent<GetVerbsEvent<ExamineVerb>>(entity.Value, verbArgs, false);
        }
    }
    private void OnGetExamineVerbs(EntityUid uid, IdExaminableComponent component, GetVerbsEvent<ExamineVerb> args)
    {
        var detailsRange = _examineSystem.IsInDetailsRange(args.User, uid);
        var info = GetMessage(uid);
        var markup = FormattedMessage.FromMarkupOrThrow(info);

        var verb = new ExamineVerb()
        {
            Act = () =>
            {
                if (_net.IsClient)
                    _examineSystem.SendExamineTooltip(args.User, uid, markup, true, false);
                if (!_inventorySystem.TryGetSlotEntity(args.User, "eyes", out var eyes))
                    return;
                if (!TryComp<ShowCriminalRecordIconsComponent>(eyes, out var _))
                    return;
                component.WantedVerbVisible = true;
                Dirty(uid, component);
                if (_net.IsServer)
                {
                    RaiseNetworkEvent(new RefreshVerbsEvent(GetNetEntity(uid)), Filter.Pvs(args.Target));
                    RaiseNetworkEvent(new ResetWantedVerbEvent(GetNetEntity(uid)), Filter.Pvs(uid));
                }
            },
            Text = Loc.GetString("id-examinable-component-verb-text"),
            Category = VerbCategory.Examine,
            Disabled = !detailsRange,
            Message = detailsRange ? null : Loc.GetString("id-examinable-component-verb-disabled"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/character.svg.65dpi.png")),
        };

        var wantedVerb = new ExamineVerb() // Goobstation-WantedMenu
        {
            Act = () => OpenWantedUI(args.User, uid),
            Text = Loc.GetString("criminal-verb-name"),
            Category = VerbCategory.Examine,
            Disabled = !detailsRange,
            Message = detailsRange ? null : Loc.GetString("id-examinable-component-verb-disabled"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/_Goobstation/Interface/VerbIcons/wanted.png")),
            Priority = 65,
            ShowOnExamineTooltip = component.WantedVerbVisible,
        };

        args.Verbs.Add(verb);
        args.Verbs.Add(wantedVerb);
    }

    private void OnWantedMenuOpen(EntityUid uid,
            IdExaminableComponent comp,
            GetVerbsEvent<AlternativeVerb> args) // Goobstation-WantedMenu
    {
        if (!args.CanInteract || !args.CanAccess)
            return;
        if (!_inventorySystem.TryGetSlotEntity(args.User, "eyes", out var eyes) ||
            !TryComp<ShowCriminalRecordIconsComponent>(eyes, out var _))
            return;
        args.Verbs.Add(new AlternativeVerb()
        {
            Act = () => OpenWantedUI(args.User, uid),
            Text = Loc.GetString("criminal-verb-name"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/_Goobstation/Interface/VerbIcons/wanted.png")),
            Priority = 65
        });
    }

    private void OpenWantedUI(EntityUid uid, EntityUid target) // Goobstation-WantedMenu
    {
        _ui.TryToggleUi(target, SetWantedVerbMenu.Key, uid);
    }

    public string GetMessage(EntityUid uid)
    {
        return GetInfo(uid) ?? Loc.GetString("id-examinable-component-verb-no-id");
    }

    public string? GetInfo(EntityUid uid)
    {
        if (_inventorySystem.TryGetSlotEntity(uid, "id", out var idUid))
        {
            // PDA
            if (EntityManager.TryGetComponent(idUid, out PdaComponent? pda) &&
                TryComp<IdCardComponent>(pda.ContainedId, out var id))
            {
                return GetNameAndJob(id);
            }
            // ID Card
            if (EntityManager.TryGetComponent(idUid, out id))
            {
                return GetNameAndJob(id);
            }
        }
        return null;
    }

    private string GetNameAndJob(IdCardComponent id)
    {
        var jobSuffix = string.IsNullOrWhiteSpace(id.LocalizedJobTitle) ? string.Empty : $" ({id.LocalizedJobTitle})";

        var val = string.IsNullOrWhiteSpace(id.FullName)
            ? Loc.GetString(id.NameLocId,
                ("jobSuffix", jobSuffix))
            : Loc.GetString(id.FullNameLocId,
                ("fullName", id.FullName),
                ("jobSuffix", jobSuffix));

        return val;
    }
}
