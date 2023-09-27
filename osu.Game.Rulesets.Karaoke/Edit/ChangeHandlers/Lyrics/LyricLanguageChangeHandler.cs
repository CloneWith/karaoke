// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public partial class LyricLanguageChangeHandler : LyricPropertyChangeHandler, ILyricLanguageChangeHandler
{
    public void SetLanguage(CultureInfo? language)
    {
        PerformOnSelection(lyric =>
        {
            if (EqualityComparer<CultureInfo?>.Default.Equals(language, lyric.Language))
                return;

            lyric.Language = language;
        });
    }

    protected override bool IsWritePropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.Language));
}
