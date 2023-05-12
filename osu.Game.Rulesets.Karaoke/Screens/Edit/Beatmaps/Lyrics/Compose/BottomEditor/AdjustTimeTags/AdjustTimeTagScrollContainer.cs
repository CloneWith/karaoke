﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.BottomEditor.AdjustTimeTags;

[Cached]
public partial class AdjustTimeTagScrollContainer : TimeTagScrollContainer, IPositionSnapProvider
{
    public const float TIMELINE_HEIGHT = 38;

    [Resolved]
    private EditorClock editorClock { get; set; } = null!;

    private CurrentTimeMarker? currentTimeMarker;

    public AdjustTimeTagScrollContainer()
    {
        Padding = new MarginPadding { Top = 10 };
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours, ITimeTagModeState timeTagModeState, KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
    {
        BindableZoom.BindTo(timeTagModeState.BindableAdjustZoom);

        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowWaveform, ShowWaveformGraph);
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AdjustTimeTagWaveformOpacity, WaveformOpacity);
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AdjustTimeTagShowTick, ShowTick);
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.AdjustTimeTagTickOpacity, TickOpacity);

        AddInternal(new Box
        {
            Name = "Background",
            Depth = 1,
            RelativeSizeAxes = Axes.X,
            Height = TIMELINE_HEIGHT,
            Colour = colours.Gray3,
        });
    }

    protected override void PostProcessContent(Container content)
    {
        content.Height = TIMELINE_HEIGHT;
        content.AddRange(new Drawable[]
        {
            new AdjustTimeTagBlueprintContainer(),
            currentTimeMarker = new CurrentTimeMarker(),
        });
    }

    protected override void OnLyricChanged(Lyric newLyric)
    {
        // add the little bit delay to make sure that content width is not zero.
        this.FadeOut(1).OnComplete(x =>
        {
            const float preempt_time = 200;
            float position = PositionAtTime(newLyric.LyricStartTime - preempt_time);
            ScrollTo(position, false);

            this.FadeIn(100);
        });
    }

    protected override void UpdateAfterChildren()
    {
        base.UpdateAfterChildren();

        float position = PositionAtTime(editorClock.CurrentTime);
        currentTimeMarker?.MoveToX(position);
    }

    public SnapResult FindSnappedPositionAndTime(Vector2 screenSpacePosition, SnapType snapType = SnapType.All)
    {
        double time = TimeAtPosition(Content.ToLocalSpace(screenSpacePosition).X);
        return new SnapResult(screenSpacePosition, time);
    }
}
