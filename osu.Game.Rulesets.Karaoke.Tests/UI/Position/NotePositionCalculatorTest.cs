﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Rulesets.Karaoke.UI.Position;

namespace osu.Game.Rulesets.Karaoke.Tests.UI.Position;

[TestFixture]
public class NotePositionCalculatorTest
{
    private const int default_columns = 9;
    private const float default_column_height = 20;
    private const float default_spacing = 1;

    [TestCase(0, 0)]
    [TestCase(1, -21f)]
    [TestCase(1.5, -31.5f)]
    public void TestPositionAtTone(double scale, float expected)
    {
        var calculator = new NotePositionCalculator(default_columns, default_column_height, default_spacing);
        var note = new Note
        {
            Tone = TestCaseToneHelper.NumberToTone(scale)
        };

        float actual = calculator.YPositionAt(note);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(0, 0)]
    [TestCase(1, -21f)]
    [TestCase(1.5, -31.5f)]
    public void TestPositionAtNote(double scale, float expected)
    {
        var calculator = new NotePositionCalculator(default_columns, default_column_height, default_spacing);
        var tone = TestCaseToneHelper.NumberToTone(scale);

        float actual = calculator.YPositionAt(tone);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(0f, 0)]
    [TestCase(1f, -21f)]
    [TestCase(-1f, 21f)]
    public void TestPositionAtScoringAction(float scale, float expected)
    {
        var calculator = new NotePositionCalculator(default_columns, default_column_height, default_spacing);
        var action = new KaraokeScoringAction
        {
            Scale = scale
        };

        float actual = calculator.YPositionAt(action);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(0f, 0)]
    [TestCase(1f, -21f)]
    [TestCase(-1f, 21f)]
    public void TestPositionAtReplayFrame(float scale, float expected)
    {
        var calculator = new NotePositionCalculator(default_columns, default_column_height, default_spacing);
        var frame = new KaraokeReplayFrame(0, scale);

        float actual = calculator.YPositionAt(frame);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(0f, 0)]
    [TestCase(1f, -21f)]
    [TestCase(-1f, 21f)]
    [TestCase(10f, -84f)] // should handle the case not out of the range.
    [TestCase(-10f, 84f)]
    public void TestPositionAtScale(float scale, float expected)
    {
        var calculator = new NotePositionCalculator(default_columns, default_column_height, default_spacing);

        float actual = calculator.YPositionAt(scale);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(1, 0)]
    [TestCase(3, 1)]
    public void TestGetMaxTone(int columns, double actual)
    {
        var calculator = new NotePositionCalculator(columns, default_column_height, default_spacing);

        var expected = calculator.MaxTone;
        Assert.AreEqual(expected, TestCaseToneHelper.NumberToTone(actual));
    }

    [TestCase(1, 0)]
    [TestCase(3, -1)]
    public void TestGetMinTone(int columns, double actual)
    {
        var calculator = new NotePositionCalculator(columns, default_column_height, default_spacing);

        var expected = calculator.MinTone;
        Assert.AreEqual(expected, TestCaseToneHelper.NumberToTone(actual));
    }
}
