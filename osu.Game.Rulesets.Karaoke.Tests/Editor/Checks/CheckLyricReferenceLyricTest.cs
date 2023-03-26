﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Objects;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricReferenceLyric;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public class CheckLyricReferenceLyricTest : HitObjectCheckTest<Lyric, CheckLyricReferenceLyric>
{
    [Test]
    public void TestCheck()
    {
        var referencedLyric = new Lyric { ID = 2 };
        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new ReferenceLyricConfig(),
        };

        AssertOk(new HitObject[] { referencedLyric, lyric });
    }

    [Test]
    public void TestCheckSelfReference()
    {
        var lyric = new Lyric
        {
            ReferenceLyricConfig = new ReferenceLyricConfig(),
        };

        lyric.ReferenceLyricId = lyric.ID;
        lyric.ReferenceLyric = lyric;

        AssertNotOk<LyricIssue, IssueTemplateLyricSelfReference>(lyric);
    }

    [Test]
    public void TestCheckInvalidReferenceLyric()
    {
        var referencedLyric = new Lyric { ID = 2 };
        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new ReferenceLyricConfig(),
        };

        AssertNotOk<LyricIssue, IssueTemplateLyricInvalidReferenceLyric>(lyric);
    }

    [Test]
    public void TestCheckNullReferenceLyricConfig()
    {
        var referencedLyric = new Lyric { ID = 2 };
        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
        };

        AssertNotOk<LyricIssue, IssueTemplateLyricNullReferenceLyricConfig>(new HitObject[] { referencedLyric, lyric });
    }

    [Test]
    public void TestCheckHasReferenceLyricConfigIfNoReferenceLyric()
    {
        var lyric = new Lyric
        {
            ReferenceLyric = null,
            ReferenceLyricConfig = new ReferenceLyricConfig(),
        };

        AssertNotOk<LyricIssue, IssueTemplateLyricHasReferenceLyricConfigIfNoReferenceLyric>(lyric);
    }
}
