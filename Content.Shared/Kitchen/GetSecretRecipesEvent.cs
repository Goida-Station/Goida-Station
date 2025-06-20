// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Kitchen;

/// <summary>
/// This returns a list of recipes not found in the main list of available recipes.
/// </summary>
[ByRefEvent]
public struct GetSecretRecipesEvent()
{
    public List<FoodRecipePrototype> Recipes = new();
}