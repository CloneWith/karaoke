﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class Note : KaraokeHitObject, IHasPage, IHasDuration, IHasText, IDeepCloneable<Note>
    {
        [JsonIgnore]
        public readonly Bindable<int?> PageIndexBindable = new();

        /// <summary>
        /// Order
        /// </summary>
        [JsonIgnore]
        public int? PageIndex
        {
            get => PageIndexBindable.Value;
            set => PageIndexBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<string> TextBindable = new();

        /// <summary>
        /// Text display on the note.
        /// </summary>
        /// <example>
        /// 花
        /// </example>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<string?> RubyTextBindable = new();

        /// <summary>
        /// Ruby text.
        /// Should placing something like ruby, 拼音 or ふりがな.
        /// Will be display only if <see cref="KaraokeRulesetSetting.DisplayNoteRubyText"/> is true.
        /// </summary>
        /// <example>
        /// はな
        /// </example>
        public string? RubyText
        {
            get => RubyTextBindable.Value;
            set => RubyTextBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<bool> DisplayBindable = new();

        /// <summary>
        /// Display this note
        /// </summary>
        public bool Display
        {
            get => DisplayBindable.Value;
            set => DisplayBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<Tone> ToneBindable = new();

        /// <summary>
        /// Tone of this note
        /// </summary>
        public virtual Tone Tone
        {
            get => ToneBindable.Value;
            set => ToneBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<double> StartTimeOffsetBindable = new BindableDouble();

        /// <summary>
        /// Offset time relative to the start time.
        /// </summary>
        public double StartTimeOffset
        {
            get => StartTimeOffsetBindable.Value;
            set => StartTimeOffsetBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<double> EndTimeOffsetBindable = new BindableDouble();

        /// <summary>
        /// Offset time relative to the end time.
        /// Negative value means the adjusted time is smaller than actual.
        /// </summary>
        public double EndTimeOffset
        {
            get => EndTimeOffsetBindable.Value;
            set => EndTimeOffsetBindable.Value = value;
        }

        /// <summary>
        /// Start time.
        /// There's no need to save the time because it's calculated by the <see cref="TimeTag"/>
        /// </summary>
        [JsonIgnore]
        public override double StartTime
        {
            get => base.StartTime;
            set => throw new NotSupportedException($"The time will auto-sync via {nameof(ReferenceLyric)} and {nameof(ReferenceTimeTagIndex)}.");
        }

        [JsonIgnore]
        public readonly Bindable<double> DurationBindable = new BindableDouble();

        /// <summary>
        /// Duration.
        /// There's no need to save the time because it's calculated by the <see cref="TimeTag"/>
        /// </summary>
        [JsonIgnore]
        public double Duration
        {
            get => DurationBindable.Value;
            set => throw new NotSupportedException($"The time will auto-sync via {nameof(ReferenceLyric)} and {nameof(ReferenceTimeTagIndex)}.");
        }

        /// <summary>
        /// End time.
        /// There's no need to save the time because it's calculated by the <see cref="TimeTag"/>
        /// </summary>
        [JsonIgnore]
        public double EndTime => StartTime + Duration;

        private int? referenceLyricId;

        public int? ReferenceLyricId
        {
            get => referenceLyricId;
            set
            {
                referenceLyricId = value;

                if (referenceLyricId != ReferenceLyric?.ID)
                {
                    ReferenceLyric = null;
                }
            }
        }

        [JsonIgnore]
        public readonly Bindable<Lyric?> ReferenceLyricBindable = new();

        /// <summary>
        /// Relative lyric.
        /// Technically parent lyric will not change after assign, but should not restrict in model layer.
        /// </summary>
        [JsonIgnore]
        public Lyric? ReferenceLyric
        {
            get => ReferenceLyricBindable.Value;
            set
            {
                ReferenceLyricBindable.Value = value;

                if (value?.ID != ReferenceLyricId)
                {
                    ReferenceLyricId = value?.ID;
                }
            }
        }

        [JsonIgnore]
        public readonly Bindable<int> ReferenceTimeTagIndexBindable = new();

        public int ReferenceTimeTagIndex
        {
            get => ReferenceTimeTagIndexBindable.Value;
            set => ReferenceTimeTagIndexBindable.Value = value;
        }

        public TimeTag? StartReferenceTimeTag => ReferenceLyric?.TimeTags.ElementAtOrDefault(ReferenceTimeTagIndex);

        public TimeTag? EndReferenceTimeTag => ReferenceLyric?.TimeTags.ElementAtOrDefault(ReferenceTimeTagIndex + 1);

        public Note()
        {
            ReferenceLyricBindable.ValueChanged += e =>
            {
                if (e.OldValue != null)
                    e.OldValue.TimeTagsVersion.ValueChanged -= timeTagVersionChanged;

                if (e.NewValue != null)
                    e.NewValue.TimeTagsVersion.ValueChanged += timeTagVersionChanged;

                syncStartTimeAndDurationFromTimeTag();
            };

            StartTimeOffsetBindable.ValueChanged += _ => syncStartTimeAndDurationFromTimeTag();
            EndTimeOffsetBindable.ValueChanged += _ => syncStartTimeAndDurationFromTimeTag();
            ReferenceTimeTagIndexBindable.ValueChanged += _ => syncStartTimeAndDurationFromTimeTag();

            void timeTagVersionChanged(ValueChangedEvent<int> e) => syncStartTimeAndDurationFromTimeTag();

            void syncStartTimeAndDurationFromTimeTag()
            {
                var startTimeTag = StartReferenceTimeTag;
                var endTimeTag = EndReferenceTimeTag;

                double startTime = startTimeTag?.Time ?? 0;
                double endTime = endTimeTag?.Time ?? 0;
                double duration = endTime - startTime;

                StartTimeBindable.Value = startTimeTag == null ? 0 : startTime + StartTimeOffset;
                DurationBindable.Value = endTimeTag == null ? 0 : Math.Max(duration - StartTimeOffset + EndTimeOffset, 0);
            }
        }

        public override Judgement CreateJudgement() => new KaraokeNoteJudgement();

        protected override HitWindows CreateHitWindows() => new KaraokeNoteHitWindows();

        public Note DeepClone()
        {
            string serializeString = this.Serialize();
            var note = serializeString.Deserialize<Note>();
            note.ReferenceLyric = ReferenceLyric;

            return note;
        }
    }
}
