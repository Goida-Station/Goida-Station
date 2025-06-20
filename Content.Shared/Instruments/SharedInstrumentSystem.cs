// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Instruments;

public abstract class SharedInstrumentSystem : EntitySystem
{
    public abstract bool ResolveInstrument(EntityUid uid, ref SharedInstrumentComponent? component);

    public virtual void SetupRenderer(EntityUid uid, bool fromStateChange, SharedInstrumentComponent? instrument = null)
    {
    }

    public virtual void EndRenderer(EntityUid uid, bool fromStateChange, SharedInstrumentComponent? instrument = null)
    {
    }

    public void SetInstrumentProgram(EntityUid uid, SharedInstrumentComponent component, byte program, byte bank)
    {
        component.InstrumentBank = bank;
        component.InstrumentProgram = program;
        Dirty(uid, component);
    }
}