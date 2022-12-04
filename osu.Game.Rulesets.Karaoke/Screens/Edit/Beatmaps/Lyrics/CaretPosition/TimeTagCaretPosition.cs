﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition
{
    public readonly struct TimeTagCaretPosition : IIndexCaretPosition
    {
        public TimeTagCaretPosition(Lyric lyric, TimeTag timeTag, CaretGenerateType generateType = CaretGenerateType.Action)
        {
            Lyric = lyric;
            TimeTag = timeTag;
            GenerateType = generateType;
        }

        public Lyric Lyric { get; }

        public TimeTag TimeTag { get; }

        public CaretGenerateType GenerateType { get; }
    }
}
