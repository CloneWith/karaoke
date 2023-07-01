﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Objects.Utils;

public static class NoteUtils
{
    /// <summary>
    /// Get the display text while gameplay or in editor.
    /// </summary>
    /// <param name="note">Note</param>
    /// <param name="useRubyTextIfHave">Should use ruby text first if have.</param>
    /// <returns>Text should be display.</returns>
    public static string DisplayText(Note note, bool useRubyTextIfHave = false)
    {
        if (!useRubyTextIfHave)
            return note.Text;

        return string.IsNullOrEmpty(note.RubyText) ? note.Text : note.RubyText;
    }
}
