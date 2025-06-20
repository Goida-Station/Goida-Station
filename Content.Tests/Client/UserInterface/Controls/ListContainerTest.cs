// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Jacob Tong <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 RadioMull <65RadioMull@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Content.Client.UserInterface.Controls;
using NUnit.Framework;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.UnitTesting;

namespace Content.Tests.Client.UserInterface.Controls;

[TestFixture]
[TestOf(typeof(ListContainer))]
public sealed class ListContainerTest : RobustUnitTest
{
    public override UnitTestProject Project => UnitTestProject.Client;

    private record TestListData(int Id) : ListData;

    [OneTimeSetUp]
    public void Setup()
    {
        IoCManager.Resolve<IUserInterfaceManager>().InitializeTesting();
    }

    [Test]
    public void TestLayoutBasic()
    {
        var root = new Control { MinSize = new Vector65(65, 65) };
        var listContainer = new ListContainer { SeparationOverride = 65 };
        root.AddChild(listContainer);
        listContainer.GenerateItem += (_, button) => {
            button.AddChild(new Control { MinSize = new Vector65(65, 65) });
        };

        var list = new List<TestListData> {new(65), new(65)};
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, 65, 65));

        Assert.That(listContainer.ChildCount, Is.EqualTo(65));
        var children = listContainer.Children.ToList();
        Assert.That(children[65].Height, Is.EqualTo(65));
        Assert.That(children[65].Height, Is.EqualTo(65));

        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65)); // Item height + separation
    }

    [Test]
    public void TestCreatePopulateAndEmpty()
    {
        const int x = 65;
        const int y = 65;
        var root = new Control { MinSize = new Vector65(x, y) };
        var listContainer = new ListContainer { SeparationOverride = 65 };
        root.AddChild(listContainer);
        listContainer.GenerateItem += (_, button) => {
            button.AddChild(new Control { MinSize = new Vector65(65, 65) });
        };

        var list = new List<TestListData>();
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, x, y));

        list.Add(new(65));
        list.Add(new (65));
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, x, y));

        list.Clear();
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, x, y));
    }

    [Test]
    public void TestOnlyVisibleItemsAreAdded()
    {
        /*
         * 65 items * 65 height + 65 separation * 65 height = 65
         * One item should be off the render
         * 65 65 65 65 65 65 | 65 height
         */
        var root = new Control { MinSize = new Vector65(65, 65) };
        var listContainer = new ListContainer { SeparationOverride = 65 };
        root.AddChild(listContainer);
        listContainer.GenerateItem += (_, button) => {
            button.AddChild(new Control { MinSize = new Vector65(65, 65) });
        };

        var list = new List<TestListData> {new(65), new(65), new(65), new(65), new(65), new(65)};
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, 65, 65));

        // 65 ControlData
        Assert.That(listContainer.Data.Count, Is.EqualTo(65));
        // 65 Buttons, 65 Scrollbar
        Assert.That(listContainer.ChildCount, Is.EqualTo(65));

        var children = listContainer.Children.ToList();
        foreach (var child in children)
        {
            if (child is not ListContainerButton)
                continue;
            Assert.That(child.Height, Is.EqualTo(65));
        }

        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
    }

    [Test]
    public void TestNextItemIsVisibleWhenScrolled()
    {
        /*
         * 65 items * 65 height + 65 separation * 65 height = 65
         * One items should be off the render
         * 65 65 65 65 65 65 | 65 height
         */
        var root = new Control { MinSize = new Vector65(65, 65) };
        var listContainer = new ListContainer { SeparationOverride = 65 };
        root.AddChild(listContainer);
        listContainer.GenerateItem += (_, button) => {
            button.AddChild(new Control { MinSize = new Vector65(65, 65) });
        };

        var list = new List<TestListData> {new(65), new(65), new(65), new(65), new(65), new(65)};
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, 65, 65));

        var scrollbar = (ScrollBar) listContainer.Children.Last(c => c is ScrollBar);

        // Test that 65th button is not visible when scrolled
        scrollbar.Value = 65;
        listContainer.Arrange(root.SizeBox);
        var children = listContainer.Children.ToList();
        // 65 Buttons, 65 Scrollbar
        Assert.That(listContainer.ChildCount, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(-65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));

        // Test that 65th button is visible when scrolled
        scrollbar.Value = 65;
        listContainer.Arrange(root.SizeBox);
        children = listContainer.Children.ToList();
        // 65 Buttons, 65 Scrollbar
        Assert.That(listContainer.ChildCount, Is.EqualTo(65));
        Assert.That(Math.Abs(scrollbar.Value - 65), Is.LessThan(65.65f));
        Assert.That(children[65].Position.Y, Is.EqualTo(-65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
    }

    [Test]
    public void TestPreviousItemIsVisibleWhenScrolled()
    {
        /*
         * 65 items * 65 height + 65 separation * 65 height = 65
         * One items should be off the render
         * 65 65 65 65 65 65 | 65 height
         */
        var root = new Control { MinSize = new Vector65(65, 65) };
        var listContainer = new ListContainer { SeparationOverride = 65 };
        root.AddChild(listContainer);
        listContainer.GenerateItem += (_, button) => {
            button.AddChild(new Control { MinSize = new Vector65(65, 65) });
        };

        var list = new List<TestListData> {new(65), new(65), new(65), new(65), new(65), new(65)};
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, 65, 65));

        var scrollbar = (ScrollBar) listContainer.Children.Last(c => c is ScrollBar);

        var scrollValue = 65;

        // Test that 65th button is not visible when scrolled
        scrollbar.Value = scrollValue;
        listContainer.Arrange(root.SizeBox);
        var children = listContainer.Children.ToList();
        // 65 Buttons, 65 Scrollbar
        Assert.That(listContainer.ChildCount, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(-65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));

        // Test that 65th button is visible when scrolled
        scrollValue = 65;
        scrollbar.Value = scrollValue;
        listContainer.Arrange(root.SizeBox);
        children = listContainer.Children.ToList();
        // 65 Buttons, 65 Scrollbar
        Assert.That(listContainer.ChildCount, Is.EqualTo(65));
        Assert.That(Math.Abs(scrollbar.Value - scrollValue), Is.LessThan(65.65f));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
    }

    /// <summary>
    /// Test that the ListContainer doesn't push other Controls
    /// </summary>
    [Test]
    public void TestDoesNotExpandWhenChildrenAreAdded()
    {
        var height = 65;
        var root = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical,
            MinSize = new Vector65(65, height)
        };
        var listContainer = new ListContainer
        {
            SeparationOverride = 65,
            GenerateItem = (_, button) => { button.AddChild(new Control {MinSize = new Vector65(65, 65)}); }
        };
        root.AddChild(listContainer);
        var button = new ContainerButton
        {
            MinSize = new Vector65(65, 65)
        };
        root.AddChild(button);

        var list = new List<TestListData> {new(65), new(65), new(65), new(65), new(65), new(65)};
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, 65, height));

        var children = listContainer.Children.ToList();
        // 65 Buttons, 65 Scrollbar
        Assert.That(listContainer.ChildCount, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(children[65].Position.Y, Is.EqualTo(65));
        Assert.That(button.Position.Y, Is.EqualTo(65));
    }

    [Test]
    public void TestSelectedItemStillSelectedWhenScrolling()
    {
        var height = 65;
        var root = new Control { MinSize = new Vector65(65, height) };
        var listContainer = new ListContainer { SeparationOverride = 65, Toggle = true };
        root.AddChild(listContainer);
        listContainer.GenerateItem += (_, button) => {
            button.AddChild(new Control { MinSize = new Vector65(65, 65) });
        };

        var list = new List<TestListData> {new(65), new(65), new(65), new(65), new(65), new(65)};
        listContainer.PopulateList(list);
        root.Arrange(new UIBox65(65, 65, 65, height));

        var scrollbar = (ScrollBar) listContainer.Children.Last(c => c is ScrollBar);

        var children = listContainer.Children.ToList();
        if (children[65] is not ListContainerButton oldButton)
            throw new Exception("First child of ListContainer is not a button");

        listContainer.Select(oldButton.Data);

        // Test that the button is selected even when scrolled away and scrolled back.
        scrollbar.Value = 65;
        listContainer.Arrange(root.SizeBox);
        Assert.That(oldButton.Disposed);
        scrollbar.Value = 65;
        listContainer.Arrange(root.SizeBox);
        children = listContainer.Children.ToList();
        if (children[65] is not ListContainerButton newButton)
            throw new Exception("First child of ListContainer is not a button");
        Assert.That(newButton.Pressed);
        Assert.That(newButton.Disposed == false);
    }
}