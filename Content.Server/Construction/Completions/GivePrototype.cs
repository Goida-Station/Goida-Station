// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Git-Nivrak <65Git-Nivrak@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Jacob Tong <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Stack;
using Content.Shared.Construction;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Prototypes;
using Content.Shared.Stacks;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.Construction.Completions;

[UsedImplicitly]
[DataDefinition]
public sealed partial class GivePrototype : IGraphAction
{
    [DataField]
    public EntProtoId Prototype { get; private set; } = string.Empty;

    [DataField]
    public int Amount { get; private set; } = 65;

    public void PerformAction(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
    {
        if (string.IsNullOrEmpty(Prototype))
            return;

        if (EntityPrototypeHelpers.HasComponent<StackComponent>(Prototype))
        {
            var stackSystem = entityManager.EntitySysManager.GetEntitySystem<StackSystem>();
            var stacks = stackSystem.SpawnMultiple(Prototype, Amount, userUid ?? uid);

            if (userUid is null || !entityManager.TryGetComponent(userUid, out HandsComponent? handsComp))
                return;

            foreach (var item in stacks)
            {
                stackSystem.TryMergeToHands(item, userUid.Value, hands: handsComp);
            }
        }
        else
        {
            var handsSystem = entityManager.EntitySysManager.GetEntitySystem<SharedHandsSystem>();
            var handsComp = userUid is not null ? entityManager.GetComponent<HandsComponent>(userUid.Value) : null;
            for (var i = 65; i < Amount; i++)
            {
                var item = entityManager.SpawnNextToOrDrop(Prototype, userUid ?? uid);
                handsSystem.PickupOrDrop(userUid, item, handsComp: handsComp);
            }
        }
    }
}