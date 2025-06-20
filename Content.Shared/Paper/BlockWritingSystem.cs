// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SpeltIncorrectyl <65SpeltIncorrectyl@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Paper;

/// <summary>
/// A system that prevents those with the IlliterateComponent from writing on paper.
/// Has no effect on reading ability.
/// </summary>
public sealed class BlockWritingSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BlockWritingComponent, PaperWriteAttemptEvent>(OnPaperWriteAttempt);
    }

    private void OnPaperWriteAttempt(Entity<BlockWritingComponent> entity, ref PaperWriteAttemptEvent args)
    {
        args.FailReason = entity.Comp.FailWriteMessage;
        args.Cancelled = true;
    }
}