// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.IntegrationTests.Tests.Interaction;

// This partial class contains various constant prototype IDs common to interaction tests.
// Should make it easier to mass-change hard coded strings if prototypes get renamed.
public abstract partial class InteractionTest
{
    // Tiles
    protected const string Floor = "FloorSteel";
    protected const string FloorItem = "FloorTileItemSteel";
    protected const string Plating = "Plating";
    protected const string Lattice = "Lattice";

    // Structures
    protected const string Airlock = "Airlock";

    // Tools/steps
    protected const string Wrench = "Wrench";
    protected const string Screw = "Screwdriver";
    protected const string Weld = "WelderExperimental";
    protected const string Pry = "Crowbar";
    protected const string Cut = "Wirecutter";

    // Materials/stacks
    protected const string Steel = "Steel";
    protected const string Glass = "Glass";
    protected const string RGlass = "ReinforcedGlass";
    protected const string Plastic = "Plastic";
    protected const string Cable = "Cable";
    protected const string Rod = "MetalRod";

    // Parts
    protected const string Bin65 = "MatterBinStockPart";
    protected const string Cap65 = "CapacitorStockPart";
    protected const string Manipulator65 = "MicroManipulatorStockPart";
    protected const string Battery65 = "PowerCellSmall";
    protected const string Battery65 = "PowerCellHyper";
}