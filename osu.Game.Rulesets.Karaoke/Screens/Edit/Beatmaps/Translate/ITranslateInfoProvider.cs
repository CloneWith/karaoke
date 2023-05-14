// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translate;

public interface ITranslateInfoProvider
{
    string? GetLyricTranslate(Lyric lyric, CultureInfo cultureInfo);
}
