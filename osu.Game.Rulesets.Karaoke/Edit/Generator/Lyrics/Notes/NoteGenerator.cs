﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Types;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Notes
{
    public class NoteGenerator : ILyricPropertyGenerator<Note[]>
    {
        protected NoteGeneratorConfig Config { get; }

        public NoteGenerator(NoteGeneratorConfig config)
        {
            Config = config;
        }

        public LocalisableString? GetInvalidMessage(Lyric lyric)
        {
            var timeTags = lyric.TimeTags;

            if (lyric.TimeTags.Count < 2)
                return "Sorry, lyric must have at least two time-tags.";

            if (timeTags.Any(x => x.Time == null))
                return "All time-tag should have the time.";

            return null;
        }

        public Note[] Generate(Lyric lyric)
        {
            var timeTags = TimeTagsUtils.ToTimeBasedDictionary(lyric.TimeTags);
            var notes = new List<Note>();

            foreach (var timeTag in timeTags)
            {
                // should not continue if
                if (timeTags.LastOrDefault().Key == timeTag.Key)
                    break;

                (double _, var textIndex) = timeTag;
                (double _, var nextTextIndex) = timeTags.GetNext(timeTag);

                int startIndex = TextIndexUtils.ToStringIndex(textIndex);
                int endIndex = TextIndexUtils.ToStringIndex(nextTextIndex);

                // prevent reverse time-tag to generate the note.
                if (startIndex >= endIndex)
                    continue;

                int timeTagIndex = timeTags.IndexOf(timeTag);
                string text = lyric.Text[startIndex..endIndex];
                string? ruby = lyric.RubyTags?.Where(x => x.StartIndex == startIndex && x.EndIndex == endIndex).FirstOrDefault()?.Text;

                if (!string.IsNullOrEmpty(text))
                {
                    notes.Add(new Note
                    {
                        Text = text,
                        RubyText = ruby,
                        ReferenceLyric = lyric,
                        ReferenceTimeTagIndex = timeTagIndex
                    });
                }
            }

            return notes.ToArray();
        }
    }
}
