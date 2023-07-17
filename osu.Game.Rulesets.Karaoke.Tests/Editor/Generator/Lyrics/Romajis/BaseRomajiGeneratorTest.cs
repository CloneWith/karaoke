﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Romajis;

public abstract class BaseRomajiGeneratorTest<TRomajiGenerator, TConfig> : BaseLyricGeneratorTest<TRomajiGenerator, RomajiGenerateResult[], TConfig>
    where TRomajiGenerator : RomajiGenerator<TConfig> where TConfig : RomajiGeneratorConfig, new()
{
    protected void CheckGenerateResult(Lyric lyric, string[] expectedRubies, TConfig config)
    {
        var expected = RomajiGenerateResultHelper.ParseRomajiGenerateResults(lyric.TimeTags, expectedRubies);
        CheckGenerateResult(lyric, expected, config);
    }

    protected override void AssertEqual(RomajiGenerateResult[] expected, RomajiGenerateResult[] actual)
    {
        TimeTagAssert.ArePropertyEqual(expected.Select(x => x.TimeTag).ToArray(), actual.Select(x => x.TimeTag).ToArray());
        Assert.AreEqual(expected.Select(x => x.InitialRomaji), actual.Select(x => x.InitialRomaji));
        Assert.AreEqual(expected.Select(x => x.RomajiText), actual.Select(x => x.RomajiText));
    }
}
