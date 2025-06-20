// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vyacheslav Titov <rincew65nd@ya.ru>
// SPDX-FileCopyrightText: 65 Титов Вячеслав Витальевич <rincew65nd@yandex.ru>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 ActiveMammmoth <65ActiveMammmoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Mind;
using Content.Shared.Roles;
using Content.Shared.Store;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.Store.Conditions;

/// <summary>
/// Allows a store entry to be filtered out based on the user's antag role.
/// Supports both blacklists and whitelists. This is copypaste because roles
/// are absolute shitcode. Refactor this later. -emo
/// </summary>
public sealed partial class BuyerAntagCondition : ListingCondition
{
    /// <summary>
    /// A whitelist of antag roles that can purchase this listing. Only one needs to be found.
    /// </summary>
    [DataField("whitelist", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<AntagPrototype>))]
    public HashSet<string>? Whitelist;

    /// <summary>
    /// A blacklist of antag roles that cannot purchase this listing. Only one needs to be found.
    /// </summary>
    [DataField("blacklist", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<AntagPrototype>))]
    public HashSet<string>? Blacklist;

    public override bool Condition(ListingConditionArgs args)
    {
        var ent = args.EntityManager;

        if (!ent.HasComponent<MindComponent>(args.Buyer))
            return true; // inanimate objects don't have minds

        var roleSystem = ent.System<SharedRoleSystem>();
        var roles = roleSystem.MindGetAllRoleInfo(args.Buyer);

        if (Blacklist != null)
        {
            foreach (var role in roles)
            {
                if (!role.Antagonist || string.IsNullOrEmpty(role.Prototype))
                    continue;

                if (Blacklist.Contains(role.Prototype))
                    return false;
            }
        }

        if (Whitelist != null)
        {
            var found = false;
            foreach (var role in roles)
            {

                if (!role.Antagonist || string.IsNullOrEmpty(role.Prototype))
                    continue;

                if (Whitelist.Contains(role.Prototype))
                    found = true;
            }
            if (!found)
                return false;
        }

        return true;
    }
}