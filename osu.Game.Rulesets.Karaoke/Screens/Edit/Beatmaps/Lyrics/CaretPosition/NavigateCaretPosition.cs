﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition
{
    public readonly struct NavigateCaretPosition : ICaretPosition
    {
        public NavigateCaretPosition(Lyric lyric, CaretGenerateType generateType = CaretGenerateType.Action)
        {
            Lyric = lyric;
            GenerateType = generateType;
        }

        public Lyric Lyric { get; }

        public CaretGenerateType GenerateType { get; }
    }
}
