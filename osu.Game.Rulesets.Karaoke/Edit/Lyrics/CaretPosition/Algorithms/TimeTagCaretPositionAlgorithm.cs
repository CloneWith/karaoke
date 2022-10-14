﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class TimeTagCaretPositionAlgorithm : IndexCaretPositionAlgorithm<TimeTagCaretPosition>
    {
        public MovingTimeTagCaretMode Mode { get; set; }

        public TimeTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override void Validate(TimeTagCaretPosition input)
        {
            var timeTag = input.TimeTag;
            var lyric = input.Lyric;

            Debug.Assert(lyric.TimeTags.Contains(timeTag));
        }

        protected override bool PositionMovable(TimeTagCaretPosition position)
        {
            var lyric = position.Lyric;
            var timeTag = position.TimeTag;

            return lyric.TimeTags.Contains(timeTag)
                   && timeTagMovable(timeTag);
        }

        protected override TimeTagCaretPosition? MoveToPreviousLyric(TimeTagCaretPosition currentPosition)
        {
            var currentTimeTag = currentPosition.TimeTag;

            var currentLyric = currentPosition.Lyric;
            var previousLyric = Lyrics.GetPreviousMatch(currentLyric, l => l.TimeTags.Any(timeTagMovable));
            if (previousLyric == null)
                return null;

            var timeTags = previousLyric.TimeTags.Where(timeTagMovable).ToArray();
            var upTimeTag = timeTags.FirstOrDefault(x => x.Index >= currentTimeTag.Index)
                            ?? timeTags.LastOrDefault();

            if (upTimeTag == null)
                return null;

            return timeTagToPosition(upTimeTag);
        }

        protected override TimeTagCaretPosition? MoveToNextLyric(TimeTagCaretPosition currentPosition)
        {
            var currentTimeTag = currentPosition.TimeTag;

            var currentLyric = currentPosition.Lyric;
            var nextLyric = Lyrics.GetNextMatch(currentLyric, l => l.TimeTags.Any(timeTagMovable));
            if (nextLyric == null)
                return null;

            var timeTags = nextLyric.TimeTags.Where(timeTagMovable).ToArray();
            var downTimeTag = timeTags.FirstOrDefault(x => x.Index >= currentTimeTag.Index)
                              ?? timeTags.LastOrDefault();

            if (downTimeTag == null)
                return null;

            return timeTagToPosition(downTimeTag);
        }

        protected override TimeTagCaretPosition? MoveToFirstLyric()
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            var firstTimeTag = timeTags.FirstOrDefault(timeTagMovable);
            if (firstTimeTag == null)
                return null;

            return timeTagToPosition(firstTimeTag);
        }

        protected override TimeTagCaretPosition? MoveToLastLyric()
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            var lastTimeTag = timeTags.LastOrDefault(timeTagMovable);
            if (lastTimeTag == null)
                return null;

            return timeTagToPosition(lastTimeTag);
        }

        protected override TimeTagCaretPosition? MoveToTargetLyric(Lyric lyric)
        {
            var targetTimeTag = lyric.TimeTags.FirstOrDefault(timeTagMovable);

            // should not move to lyric if contains no time-tag.
            return targetTimeTag == null ? null : new TimeTagCaretPosition(lyric, targetTimeTag, CaretGenerateType.TargetLyric);
        }

        protected override TimeTagCaretPosition? MoveToPreviousIndex(TimeTagCaretPosition currentPosition)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            var previousTimeTag = timeTags.GetPreviousMatch(currentPosition.TimeTag, timeTagMovable);
            if (previousTimeTag == null)
                return null;

            return timeTagToPosition(previousTimeTag);
        }

        protected override TimeTagCaretPosition? MoveToNextIndex(TimeTagCaretPosition currentPosition)
        {
            var timeTags = Lyrics.SelectMany(x => x.TimeTags).ToArray();
            var nextTimeTag = timeTags.GetNextMatch(currentPosition.TimeTag, timeTagMovable);
            if (nextTimeTag == null)
                return null;

            return timeTagToPosition(nextTimeTag);
        }

        private TimeTagCaretPosition? timeTagToPosition(TimeTag timeTag)
        {
            var lyric = timeTagInLyric(timeTag);
            if (lyric == null)
                return null;

            return new TimeTagCaretPosition(lyric, timeTag);
        }

        private Lyric? timeTagInLyric(TimeTag timeTag)
        {
            return Lyrics.FirstOrDefault(x => x.TimeTags.Contains(timeTag));
        }

        private bool timeTagMovable(TimeTag timeTag)
        {
            return Mode switch
            {
                MovingTimeTagCaretMode.None => true,
                MovingTimeTagCaretMode.OnlyStartTag => timeTag.Index.State == TextIndex.IndexState.Start,
                MovingTimeTagCaretMode.OnlyEndTag => timeTag.Index.State == TextIndex.IndexState.End,
                _ => throw new InvalidOperationException(nameof(MovingTimeTagCaretMode))
            };
        }
    }
}
