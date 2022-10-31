﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public struct RubyTagCaretPosition : ITextTagCaretPosition
    {
        public RubyTagCaretPosition(Lyric lyric, RubyTag rubyTag, CaretGenerateType generateType = CaretGenerateType.Action)
        {
            Lyric = lyric;
            RubyTag = rubyTag;
            GenerateType = generateType;
        }

        public Lyric Lyric { get; }

        public RubyTag RubyTag { get; }

        public CaretGenerateType GenerateType { get; }
    }
}
