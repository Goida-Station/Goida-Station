// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Materials;
using Robust.Client.UserInterface.Controllers;

namespace Content.Client.Materials.UI;

public sealed class MaterialStorageUIController : UIController
{
    public void SendLatheEjectMessage(EntityUid uid, string material, int sheetsToEject)
    {
        EntityManager.RaisePredictiveEvent(new EjectMaterialMessage(EntityManager.GetNetEntity(uid), material, sheetsToEject));
    }
}