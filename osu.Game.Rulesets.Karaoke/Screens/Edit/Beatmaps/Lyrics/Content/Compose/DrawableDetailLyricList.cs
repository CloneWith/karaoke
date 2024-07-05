﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose;

public partial class DrawableDetailLyricList : DrawableLyricList
{
    protected override Vector2 Spacing => new();

    protected override bool ScrollToPosition(ICaretPosition caret)
    {
        // should scroll to the target position on every case.
        return true;
    }

    protected override int SkipRows()
    {
        // it's a fixed number for now.
        return 3;
    }

    protected override DrawableLyricListItem CreateLyricListItem(Lyric item)
        => new DrawableDetailLyricListItem(item);

    protected override Row GetCreateNewLyricRow()
        => new CreateNewLyricDetailRow();
}
