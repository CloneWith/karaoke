// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes
{
    public partial class TimeTagModeState : ModeStateWithBlueprintContainer<TimeTag>, ITimeTagModeState
    {
        private readonly Bindable<TimeTagEditMode> bindableEditMode = new();

        public IBindable<TimeTagEditMode> BindableEditMode => bindableEditMode;

        public BindableFloat BindableRecordZoom { get; } = new();

        public BindableFloat BindableAdjustZoom { get; } = new();

        [BackgroundDependencyLoader]
        private void load(EditorClock editorClock)
        {
            BindableRecordZoom.MaxValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 800);
            BindableRecordZoom.MinValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 4000);
            BindableRecordZoom.Value = BindableRecordZoom.Default = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 2000);

            BindableAdjustZoom.MaxValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 1600);
            BindableAdjustZoom.MinValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 8000);
            BindableAdjustZoom.Value = BindableAdjustZoom.Default = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 4000);
        }

        public void ChangeEditMode(TimeTagEditMode mode)
            => bindableEditMode.Value = mode;

        protected override bool IsWriteLyricPropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.TimeTags));

        protected override bool SelectFirstProperty(Lyric lyric)
            => false;

        protected override IEnumerable<TimeTag> SelectableProperties(Lyric lyric)
            => Array.Empty<TimeTag>();
    }
}
