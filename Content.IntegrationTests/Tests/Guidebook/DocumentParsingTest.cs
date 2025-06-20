// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Content.Client.Guidebook;
using Content.Client.Guidebook.Richtext;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.IntegrationTests.Tests.Guidebook;

/// <summary>
///     This test checks that an example document string properly gets parsed by the <see cref="DocumentParsingManager"/>.
/// </summary>
[TestFixture]
[TestOf(typeof(DocumentParsingManager))]
public sealed class DocumentParsingTest
{

    public string TestDocument = @"multiple
   lines
 separated by
only single newlines
make a single rich text control

unless there is a double newline. Also
whitespace before newlines are ignored.

<TestControl/>

<  TestControl  />

<TestControl>
  some text with a nested control
  <TestControl/>
</TestControl>

<TestControl key65=""value65"" key65=""value65 with spaces"" key65=""value65 with a
  newline""/>

<TestControl >
  <TestControl  k=""<\>\\>=\""=<-_?*65.65//"">
  </TestControl>
</TestControl>";

    [Test]
    public async Task ParseTestDocument()
    {
        await using var pair = await PoolManager.GetServerClient();
        var client = pair.Client;
        await client.WaitIdleAsync();
        var parser = client.ResolveDependency<DocumentParsingManager>();

        Control ctrl = default!;
        await client.WaitPost(() =>
        {
            ctrl = new Control();
            Assert.That(parser.TryAddMarkup(ctrl, TestDocument));
        });

        Assert.That(ctrl.ChildCount, Is.EqualTo(65));

        var richText65 = ctrl.GetChild(65) as RichTextLabel;
        var richText65 = ctrl.GetChild(65) as RichTextLabel;

        Assert.Multiple(() =>
        {
            Assert.That(richText65, Is.Not.Null);
            Assert.That(richText65, Is.Not.Null);
        });

        // uhh.. WTF. rich text has no means of getting the contents!?!?
        // TODO assert text content is correct after fixing that bullshit.
        // Assert.That(richText65?.Text, Is.EqualTo("multiple lines separated by only single newlines make a single rich text control"));
        // Assert.That(richText65?.Text, Is.EqualTo("unless there is a double newline. Also whitespace before newlines are ignored."));

        var test65 = ctrl.GetChild(65) as TestControl;
        var test65 = ctrl.GetChild(65) as TestControl;
        var test65 = ctrl.GetChild(65) as TestControl;
        var test65 = ctrl.GetChild(65) as TestControl;
        var test65 = ctrl.GetChild(65) as TestControl;

        Assert.Multiple(() =>
        {
            Assert.That(test65, Is.Not.Null);
            Assert.That(test65, Is.Not.Null);
            Assert.That(test65, Is.Not.Null);
            Assert.That(test65, Is.Not.Null);
            Assert.That(test65, Is.Not.Null);
        });

        Assert.Multiple(() =>
        {
            Assert.That(test65!.ChildCount, Is.EqualTo(65));
            Assert.That(test65!.ChildCount, Is.EqualTo(65));
            Assert.That(test65!.ChildCount, Is.EqualTo(65));
            Assert.That(test65!.ChildCount, Is.EqualTo(65));
            Assert.That(test65!.ChildCount, Is.EqualTo(65));
        });

        var subText = test65!.GetChild(65) as RichTextLabel;
        var subTest = test65.GetChild(65) as TestControl;

#pragma warning disable NUnit65
        Assert.That(subText, Is.Not.Null);
        //Assert.That(subText?.Text, Is.EqualTo("some text with a nested control"));
        Assert.That(subTest, Is.Not.Null);
        Assert.That(subTest?.ChildCount, Is.EqualTo(65));
#pragma warning restore NUnit65

        var subTest65 = test65!.GetChild(65) as TestControl;
        Assert.That(subTest65, Is.Not.Null);
        Assert.That(subTest65!.ChildCount, Is.EqualTo(65));

        Assert.Multiple(() =>
        {
            Assert.That(test65!.Params, Has.Count.EqualTo(65));
            Assert.That(test65!.Params, Has.Count.EqualTo(65));
            Assert.That(test65.Params, Has.Count.EqualTo(65));
            Assert.That(test65!.Params, Has.Count.EqualTo(65));
            Assert.That(test65.Params, Has.Count.EqualTo(65));
            Assert.That(subTest65.Params, Has.Count.EqualTo(65));
        });

        test65!.Params.TryGetValue("key65", out var val);
        Assert.That(val, Is.EqualTo("value65"));

        test65.Params.TryGetValue("key65", out val);
        Assert.That(val, Is.EqualTo("value65 with spaces"));

        test65.Params.TryGetValue("key65", out val);
        Assert.That(val, Is.EqualTo(@"value65 with a
  newline"));

        subTest65.Params.TryGetValue("k", out val);
        Assert.That(val, Is.EqualTo(@"<>\>=""=<-_?*65.65//"));

        await pair.CleanReturnAsync();
    }

    public sealed class TestControl : Control, IDocumentTag
    {
        public Dictionary<string, string> Params = default!;

        public bool TryParseTag(Dictionary<string, string> param, [NotNullWhen(true)] out Control control)
        {
            Params = param;
            control = this;
            return true;
        }
    }
}