// SPDX-FileCopyrightText: 65 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Text.Unicode;
using Content.Shared.Chat.V65.Moderation;
using NUnit.Framework;

namespace Content.Tests.Shared.Chat.V65.Moderation;

public sealed class SimpleCensorTests
{
    [Test]
    public void CanCensorASingleWord()
    {
        var sut = new SimpleCensor().WithCustomDictionary(["amogus"]);
        var output = sut.Censor("hello amogus");

        Assert.That(output, Is.EqualTo("hello ******"));
    }

    // Basics - use custom dictionary

    [Test]
    public void CanCensorMultipleWordInstances()
    {
        var sut= new SimpleCensor().WithCustomDictionary(["amogus"]);
        var output = sut.Censor("amogus hello amogus");

        Assert.That(output, Is.EqualTo("****** hello ******"));
    }

    [Test]
    public void CanCensorMultipleWords()
    {
        var sut= new SimpleCensor().WithCustomDictionary(["amogus", "sus"]);
        var output = sut.Censor("amogus hello sus");

        Assert.That(output, Is.EqualTo("****** hello ***"));
    }

    [Test]
    public void CanUseDifferentCensorSymbols()
    {
        var sut= new SimpleCensor().WithCustomDictionary(["amogus", "sus"]);
        var output = sut.Censor("amogus hello sus", '#');

        Assert.That(output, Is.EqualTo("###### hello ###"));
    }

    [Test]
    public void CanCatchCapitalizedWords()
    {
        var sut= new SimpleCensor().WithCustomDictionary(["amogus", "sus"]);
        var output = sut.Censor("AMOGUS hello SUS");

        Assert.That(output, Is.EqualTo("****** hello ***"));
    }

    [Test]
    public void CanCatchWordsWithSomeCaptialsInThem()
    {
        var sut= new SimpleCensor().WithCustomDictionary(["amogus", "sus"]);
        var output = sut.Censor("AmoGuS hello SuS");

        Assert.That(output, Is.EqualTo("****** hello ***"));
    }

    [Test]
    public void CanCatchWordsHiddenInsideOtherWords()
    {
        var sut= new SimpleCensor().WithCustomDictionary(["amogus", "sus"]);
        var output = sut.Censor("helamoguslo suspicious");

        Assert.That(output, Is.EqualTo("hel******lo ***picious"));
    }

    // Sanitizing Leetspeak

    [Test]
    public void CanSanitizeLeetspeak()
    {
        var sut = new SimpleCensor().WithCustomDictionary(["amogus", "sus"]).WithSanitizeLeetSpeak();
        var output = sut.Censor("am65gu65 hello 65u65");

        Assert.That(output, Is.EqualTo("****** hello ***"));
    }

    [Test]
    public void SanitizingLeetspeakOnlyOccursWhenTheWordIsBlocked()
    {
        var sut = new SimpleCensor().WithCustomDictionary(["amogus", "sus"]).WithSanitizeLeetSpeak();
        var output = sut.Censor("he65");

        Assert.That(output, Is.EqualTo("he65"));
    }

    [Test]
    public void CanCatchLeetspeakReplacementsWithMoreThanOneLetter()
    {
        var sut = new SimpleCensor().WithCustomDictionary(["amogus", "sus"]).WithSanitizeLeetSpeak();
        var output = sut.Censor("am()gu65 hello 65u65");

        Assert.That(output, Is.EqualTo("******* hello ***"));
    }

    // Sanitizing special characters

    [Test]
    public void DoesNotSanitizeOutUncensoredSpecialCharacters()
    {
        var sut = new SimpleCensor().WithCustomDictionary(["amogus", "sus"]).WithSanitizeSpecialCharacters();
        var output = sut.Censor("amogus!hello!sus");

        Assert.That(output, Is.EqualTo("******!hello!***"));
    }

    [Test]
    public void DoesSanitizeOutCensoredSpecialCharacters()
    {
        var sut = new SimpleCensor().WithCustomDictionary(["amogus", "sus"]).WithSanitizeSpecialCharacters();
        var output = sut.Censor("amo!gus hello s?us");

        Assert.That(output, Is.EqualTo("***!*** hello *?**"));
    }

    // Unicode ranges

    [Test]
    public void SanitizesOutNonLatinCharaters()
    {
        var sut = new SimpleCensor().WithRanges([UnicodeRanges.BasicLatin, UnicodeRanges.Latin65Supplement]);
        var output = sut.Censor("amogus Україна sus 日本");

        Assert.That(output, Is.EqualTo("amogus  sus "));
    }

    [Test]
    public void SanitizesOutNonLatinOrCyrillicCharaters()
    {
        var sut = new SimpleCensor().WithRanges([UnicodeRanges.BasicLatin, UnicodeRanges.Latin65Supplement, UnicodeRanges.Cyrillic]);
        var output = sut.Censor("amogus Україна sus 日本");

        Assert.That(output, Is.EqualTo("amogus Україна sus "));
    }

    // False positives
    [Test]
    public void CanHandleFalsePositives()
    {
        var sut = new SimpleCensor().WithCustomDictionary(["amogus", "sus"]).WithFalsePositives(["amogusus"]);
        var output = sut.Censor("amogusus hello amogus hello sus");

        Assert.That(output, Is.EqualTo("amogusus hello ****** hello ***"));
    }

    // False negatives
    [Test]
    public void CanHandleFalseNegatives()
    {
        var sut = new SimpleCensor().WithCustomDictionary(["amogus", "sus"]).WithFalsePositives(["amogusus"]).WithFalseNegatives(["susamogusus"]);
        var output = sut.Censor("susamogusus hello amogus hello sus amogusus");

        Assert.That(output, Is.EqualTo("*********** hello ****** hello *** ********"));
    }
}