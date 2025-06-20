// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Wizard.MagicMirror;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._Shitcode.Wizard.MagicMirror;

[UsedImplicitly]
public sealed class WizardMirrorBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private WizardMirrorWindow? _window;

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<WizardMirrorWindow>();

        if (EntMan.TryGetComponent(Owner, out WizardMirrorComponent? mirror))
            _window.AllowedSpecies = new(mirror.AllowedSpecies);

        _window.Save += OnSave;
    }

    private void OnSave()
    {
        var profile = _window?.Profile;
        if (profile != null)
            SendMessage(new WizardMirrorMessage(profile));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not WizardMirrorUiState data)
            return;

        if (_window == null)
            return;

        _window.LoadedProfile = data.Profile.Clone();
        _window.SetProfile(data.Profile);
    }
}
