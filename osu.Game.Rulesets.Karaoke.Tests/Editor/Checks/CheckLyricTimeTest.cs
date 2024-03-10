﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckLyricTime;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public class CheckLyricTimeTest : HitObjectCheckTest<Lyric, CheckLyricTime>
{
    [TestCase("[1000,3000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" })]
    public void TestCheck(string lyricText, string[] timeTags)
    {
        var lyric = TestCaseTagHelper.ParseLyric(lyricText);
        lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);

        AssertOk(lyric);
    }

    [TestCase("[3000,1000]:カラオケ", new string[] { })]
    public void TestCheckTimeOverlapping(string lyricText, string[] timeTags)
    {
        var lyric = TestCaseTagHelper.ParseLyric(lyricText);
        lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);

        AssertNotOk<LyricIssue, IssueTemplateTimeOverlapping>(lyric);
    }

    [TestCase("[2000,3000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" })]
    public void TestCheckStartTimeInvalid(string lyricText, string[] timeTags)
    {
        var lyric = TestCaseTagHelper.ParseLyric(lyricText);
        lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);

        AssertNotOk<LyricIssue, IssueTemplateStartTimeInvalid>(lyric);
    }

    [TestCase("[1000,2000]:カラオケ", new[] { "[0,start]:1000", "[3,end]:3000" })]
    public void TestCheckEndTimeInvalid(string lyricText, string[] timeTags)
    {
        var lyric = TestCaseTagHelper.ParseLyric(lyricText);
        lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags);

        AssertNotOk<LyricIssue, IssueTemplateEndTimeInvalid>(lyric);
    }
}
