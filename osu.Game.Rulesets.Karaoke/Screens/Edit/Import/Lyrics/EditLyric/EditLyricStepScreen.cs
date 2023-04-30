﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics.EditLyric;

public partial class EditLyricStepScreen : LyricImporterStepScreenWithLyricEditor
{
    public override string Title => "Edit lyric";

    public override string ShortTitle => "Edit";

    public override LyricImporterStep Step => LyricImporterStep.EditLyric;

    public override IconUsage Icon => FontAwesome.Solid.Globe;

    [Cached(typeof(ILyricsChangeHandler))]
    private readonly LyricsChangeHandler lyricsChangeHandler;

    [Cached(typeof(ILyricTextChangeHandler))]
    private readonly LyricTextChangeHandler lyricTextChangeHandler;

    public EditLyricStepScreen()
    {
        AddInternal(lyricsChangeHandler = new LyricsChangeHandler());
        AddInternal(lyricTextChangeHandler = new LyricTextChangeHandler());
    }

    protected override TopNavigation CreateNavigation()
        => new EditLyricNavigation(this);

    protected override Drawable CreateContent()
        => base.CreateContent().With(_ =>
        {
            // todo : will cause text update because has ScheduleAfterChildren in lyric editor.
            SwitchLyricEditorMode(LyricEditorMode.Texting);
        });

    public override void Complete()
    {
        ScreenStack.Push(LyricImporterStep.AssignLanguage);
    }
}
