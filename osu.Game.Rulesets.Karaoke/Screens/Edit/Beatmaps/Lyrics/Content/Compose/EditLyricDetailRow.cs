﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Badge;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class EditLyricDetailRow : DetailRow
{
    public EditLyricDetailRow(Lyric lyric)
        : base(lyric)
    {
    }

    protected override Drawable CreateTimingInfo(Lyric lyric)
    {
        return new TimeTagInfo(lyric)
        {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Margin = new MarginPadding { Right = 10 },
        };
    }

    protected override Drawable CreateContent(Lyric lyric)
    {
        return new ViewOnlyLyric(lyric)
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
            Margin = new MarginPadding { Left = 10 },
            RelativeSizeAxes = Axes.X,
        };
    }
}
