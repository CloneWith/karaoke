﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Placing the properties that set by <see cref="KaraokeBeatmapProcessor"/> or being calculated.
/// Those properties will not be saved into the beatmap.
/// </summary>
public partial class Note : IHasWorkingProperty<NoteWorkingProperty>
{
    [JsonIgnore]
    private readonly NoteWorkingPropertyValidator workingPropertyValidator;

    public bool InvalidateWorkingProperty(NoteWorkingProperty workingProperty)
        => workingPropertyValidator.Invalidate(workingProperty);

    private void updateStateByWorkingProperty(NoteWorkingProperty workingProperty)
        => workingPropertyValidator.UpdateStateByWorkingProperty(workingProperty);

    public NoteWorkingProperty[] GetAllInvalidWorkingProperties()
        => workingPropertyValidator.GetAllInvalidFlags();

    [JsonIgnore]
    public readonly Bindable<int?> PageIndexBindable = new();

    /// <summary>
    /// Order
    /// </summary>
    [JsonIgnore]
    public int? PageIndex
    {
        get => PageIndexBindable.Value;
        set
        {
            PageIndexBindable.Value = value;
            updateStateByWorkingProperty(NoteWorkingProperty.Page);
        }
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
            updateStateByWorkingProperty(NoteWorkingProperty.ReferenceLyric);
        }
    }

    public TimeTag? StartReferenceTimeTag => ReferenceLyric?.TimeTags.ElementAtOrDefault(ReferenceTimeTagIndex);

    public TimeTag? EndReferenceTimeTag => ReferenceLyric?.TimeTags.ElementAtOrDefault(ReferenceTimeTagIndex + 1);
}
