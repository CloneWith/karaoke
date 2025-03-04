// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckClassicStageInfo;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

[Ignore("Disable this test until able to get the stage info from the resource file.")]
public class CheckClassicStageInfoTest : BaseCheckTest<CheckClassicStageInfo>
{
    #region stage definition

    [Test]
    public void TestCheckInvalidRowHeight()
    {
        var beatmap = createTestingBeatmap(Array.Empty<Lyric>());
        var stageInfo = createTestingStageInfo(stage =>
        {
            stage.StageDefinition.LineHeight = MIN_ROW_HEIGHT - 1;
        });
        AssertNotOk<IssueTemplateInvalidRowHeight>(getContext(beatmap, stageInfo));

        var beatmap2 = createTestingBeatmap(Array.Empty<Lyric>());
        var stageInfo2 = createTestingStageInfo(stage =>
        {
            stage.StageDefinition.LineHeight = MAX_ROW_HEIGHT + 1;
        });
        AssertNotOk<IssueTemplateInvalidRowHeight>(getContext(beatmap2, stageInfo2));
    }

    #endregion

    #region timing info

    [Test]
    public void TestCheckLessThanTwoTimingPoints()
    {
        var lyrics = new List<Lyric>
        {
            new(),
        };

        // test with 0 lyric and 0 timing points.
        var beatmap = createTestingBeatmap(null);
        var stageInfo = createTestingStageInfo(info => info.LyricTimingInfo.Timings.Clear());
        AssertNotOk<IssueTemplateLessThanTwoTimingPoints>(getContext(beatmap, stageInfo));

        // test with 0 lyric and 1 timing points.
        var beatmap2 = createTestingBeatmap(null);
        var stageInfo2 = createTestingStageInfo(timingInfos => timingInfos.AddTimingPoint());
        AssertNotOk<IssueTemplateLessThanTwoTimingPoints>(getContext(beatmap2, stageInfo2));

        // test with 1 lyric and 0 timing points.
        var beatmap3 = createTestingBeatmap(lyrics);
        var stageInfo3 = createTestingStageInfo(timingInfos => timingInfos.Timings.Clear());
        AssertNotOk<IssueTemplateLessThanTwoTimingPoints>(getContext(beatmap3, stageInfo3));

        // test with 1 lyric and 1 timing points.
        var beatmap4 = createTestingBeatmap(lyrics);
        var stageInfo4 = createTestingStageInfo(timingInfos => timingInfos.AddTimingPoint());
        AssertNotOk<IssueTemplateLessThanTwoTimingPoints>(getContext(beatmap4, stageInfo4));
    }

    [Test]
    public void TestCheckTimingIntervalTooShort()
    {
        var beatmap = createTestingBeatmap(null);
        var stageInfo = createTestingStageInfo(timingInfos =>
        {
            timingInfos.Timings.Clear();
            timingInfos.AddTimingPoint(x => x.Time = 0);
            timingInfos.AddTimingPoint(x => x.Time = MIN_TIMING_INTERVAL - 1);
        });
        AssertNotOk<BeatmapClassicLyricTimingPointIssue, IssueTemplateTimingIntervalTooShort>(getContext(beatmap, stageInfo));
    }

    [Test]
    public void TestCheckTimingIntervalTooLong()
    {
        var beatmap = createTestingBeatmap(null);
        var stageInfo = createTestingStageInfo(timingInfos =>
        {
            timingInfos.Timings.Clear();
            timingInfos.AddTimingPoint(x => x.Time = 0);
            timingInfos.AddTimingPoint(x => x.Time = MAX_TIMING_INTERVAL + 1);
        });
        AssertNotOk<BeatmapClassicLyricTimingPointIssue, IssueTemplateTimingIntervalTooLong>(getContext(beatmap, stageInfo));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestCheckTimingInfoHitObjectNotExist(bool hasHitObjectsInBeatmap)
    {
        var lyrics = new List<Lyric>
        {
            new(),
        };

        var beatmap = createTestingBeatmap(hasHitObjectsInBeatmap ? lyrics : null);
        var stageInfo = createTestingStageInfo(timingInfos =>
        {
            timingInfos.Timings.Clear();
            var timingPoint = timingInfos.AddTimingPoint(x => x.Time = 0);
            timingInfos.AddTimingPoint(x => x.Time = MIN_TIMING_INTERVAL + 1);

            var lyric = new Lyric();

            // should have error because lyric is not in the beatmap.
            timingInfos.AddToMapping(timingPoint, lyric);
        });
        AssertNotOk<IssueTemplateTimingInfoHitObjectNotExist>(getContext(beatmap, stageInfo));
    }

    [Test]
    public void TestCheckTimingInfoMappingHasNoTiming()
    {
        var lyrics = new List<Lyric>
        {
            new(),
        };

        var beatmap = createTestingBeatmap(lyrics);
        var stageInfo = createTestingStageInfo(timingInfos =>
        {
            timingInfos.Timings.Clear();
            timingInfos.AddTimingPoint(x => x.Time = 0);
            timingInfos.AddTimingPoint(x => x.Time = MIN_TIMING_INTERVAL + 1);

            // should have error because mapping value is empty.
            timingInfos.Mappings.Add(lyrics.First().ID, Array.Empty<ElementId>());
        });
        AssertNotOk<IssueTemplateTimingInfoMappingHasNoTiming>(getContext(beatmap, stageInfo));
    }

    [Test]
    public void TestCheckTimingInfoTimingNotExist()
    {
        var lyrics = new List<Lyric>
        {
            new(),
        };

        var beatmap = createTestingBeatmap(lyrics);
        var stageInfo = createTestingStageInfo(timingInfos =>
        {
            timingInfos.Timings.Clear();
            timingInfos.AddTimingPoint(x => x.Time = 0);
            timingInfos.AddTimingPoint(x => x.Time = MIN_TIMING_INTERVAL + 1);

            // should have error because mapping value is not exist.
            timingInfos.Mappings.Add(lyrics.First().ID, new[] { ElementId.NewElementId() });
        });
        AssertNotOk<IssueTemplateTimingInfoTimingNotExist>(getContext(beatmap, stageInfo));
    }

    [Test]
    public void TestCheckTimingInfoLyricNotHaveTwoTiming()
    {
        var lyrics = new List<Lyric>
        {
            new(),
        };

        var beatmap = createTestingBeatmap(lyrics);
        var stageInfo = createTestingStageInfo(timingInfos =>
        {
            timingInfos.Timings.Clear();
            timingInfos.AddTimingPoint(x => x.Time = 0);
            timingInfos.AddTimingPoint(x => x.Time = MIN_TIMING_INTERVAL + 1);
            timingInfos.AddTimingPoint(x => x.Time = MIN_TIMING_INTERVAL * 2 + 1);

            // should have error because mapping value is not exactly 2.
            timingInfos.Mappings.Add(lyrics.First().ID, timingInfos.Timings.Select(x => x.ID).ToArray());
        });
        AssertNotOk<IssueTemplateTimingInfoLyricNotHaveTwoTiming>(getContext(beatmap, stageInfo));
    }

    #endregion

    #region element

    [Test]
    public void TestCheckLyricLayoutInvalidLineNumber()
    {
        var beatmap = createTestingBeatmap(Array.Empty<Lyric>());
        var stageInfo = createTestingStageInfo(stage =>
        {
            var layoutElement = stage.LyricLayoutCategory.AvailableElements.First();
            layoutElement.Line = MIN_LINE_SIZE - 1;
        });
        AssertNotOk<IssueTemplateLyricLayoutInvalidLineNumber>(getContext(beatmap, stageInfo));

        var beatmap2 = createTestingBeatmap(Array.Empty<Lyric>());
        var stageInfo2 = createTestingStageInfo(stage =>
        {
            var layoutElement = stage.LyricLayoutCategory.AvailableElements.First();
            layoutElement.Line = MAX_LINE_SIZE + 1;
        });
        AssertNotOk<IssueTemplateLyricLayoutInvalidLineNumber>(getContext(beatmap2, stageInfo2));
    }

    #endregion

    private static IBeatmap createTestingBeatmap(IEnumerable<Lyric>? lyrics)
    {
        var karaokeBeatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            HitObjects = lyrics?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>(),
        };
        return new EditorBeatmap(karaokeBeatmap);
    }

    private static StageInfo createTestingStageInfo(Action<ClassicLyricTimingInfo>? editStageAction = null)
    {
        return createTestingStageInfo(info =>
        {
            // clear the timing info created in the base method.
            info.LyricTimingInfo.Timings.Clear();
            editStageAction?.Invoke(info.LyricTimingInfo);
        });
    }

    private static StageInfo createTestingStageInfo(Action<ClassicStageInfo>? editStageAction = null)
    {
        var stageInfo = new ClassicStageInfo();

        // add two elements to prevent no element error.
        stageInfo.LyricLayoutCategory.AddElement(x => x.Line = MIN_LINE_SIZE);
        stageInfo.LyricLayoutCategory.AddElement(x => x.Line = MIN_LINE_SIZE);
        stageInfo.StageDefinition.LineHeight = MIN_ROW_HEIGHT;

        // add default timing info to prevent the error.
        stageInfo.LyricTimingInfo.AddTimingPoint(x => x.Time = 0);
        stageInfo.LyricTimingInfo.AddTimingPoint(x => x.Time = MIN_TIMING_INTERVAL + 1);

        editStageAction?.Invoke(stageInfo);

        return stageInfo;
    }

    private static BeatmapVerifierContext getContext(IBeatmap beatmap, StageInfo stageInfo)
        => new(beatmap, new TestWorkingBeatmap(beatmap));
}
