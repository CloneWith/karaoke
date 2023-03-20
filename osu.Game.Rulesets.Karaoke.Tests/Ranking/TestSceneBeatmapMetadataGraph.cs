﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Statistics;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Scoring;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Ranking;

public partial class TestSceneBeatmapMetadataGraph : OsuTestScene
{
    [Test]
    public void TestBeatmapMetadataGraph()
    {
        var ruleset = new KaraokeRuleset().RulesetInfo;
        var originBeatmap = new TestKaraokeBeatmap(ruleset);
        if (new KaraokeBeatmapConverter(originBeatmap, new KaraokeRuleset()).Convert() is not KaraokeBeatmap karaokeBeatmap)
            throw new InvalidCastException(nameof(karaokeBeatmap));

        karaokeBeatmap.SingerInfo = createSingerInfo();
        createTest(new ScoreInfo(), karaokeBeatmap);
    }

    private void createTest(ScoreInfo score, IBeatmap beatmap) => AddStep("create test", () =>
    {
        Children = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4Extensions.FromHex("#333")
            },
            new BeatmapMetadataGraph(beatmap)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(600, 200)
            }
        };
    });

    private static SingerInfo createSingerInfo()
    {
        var singerInfo = new SingerInfo();

        for (int i = 0; i < 10; i++)
        {
            int singerIndex = i;
            singerInfo.AddSinger(s =>
            {
                s.Name = $"Singer{singerIndex}";
                s.RomajiName = $"[Romaji]Singer{singerIndex}";
                s.EnglishName = $"[English]Singer{singerIndex}";
            });
        }

        return singerInfo;
    }
}
