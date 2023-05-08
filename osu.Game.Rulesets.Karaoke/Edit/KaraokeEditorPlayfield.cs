﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Edit;

public partial class KaraokeEditorPlayfield : KaraokePlayfield
{
    protected override ScrollingNotePlayfield CreateNotePlayfield(int columns)
        => new EditorNotePlayfield(columns);
}
