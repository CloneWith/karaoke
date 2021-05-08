﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Components.Timelines.Summary.Parts;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays.Components.TimeTagEditor
{
    public class TimeTagEditorBlueprintContainer : BlueprintContainer<TimeTag>
    {
        [Resolved(CanBeNull = true)]
        private TimeTagEditor timeline { get; set; }

        [Resolved]
        private LyricManager lyricManager { get; set; }

        private DragEvent lastDragEvent;

        protected readonly Lyric Lyric;

        public TimeTagEditorBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add time-tag into blueprint container
            if (Lyric != null)
            {
                foreach (var obj in Lyric.TimeTags)
                    AddBlueprintFor(obj);
            }
        }

        protected override IEnumerable<SelectionBlueprint<TimeTag>> SortForMovement(IReadOnlyList<SelectionBlueprint<TimeTag>> blueprints)
            => blueprints.OrderBy(b => b.Item.Time);

        protected override bool ApplySnapResult(SelectionBlueprint<TimeTag>[] blueprints, SnapResult result)
        {
            if (!base.ApplySnapResult(blueprints, result))
                return false;

            var firstDragTimeTagTime = blueprints.First().Item.Time;
            if (firstDragTimeTagTime == null)
                return false;

            // main goal is applying delta time while dragging.
            if (result.Time.HasValue)
            {
                // Apply the start time at the newly snapped-to position
                double offset = result.Time.Value - firstDragTimeTagTime.Value;

                if (offset == 0)
                    return false;

                // todo : should not save separately.
                foreach (var blueprint in blueprints)
                {
                    // todo : fix logic error.
                    var timeTag = blueprint.Item;
                    timeTag.Time += offset;
                }
            }

            return true;
        }

        protected override Container<SelectionBlueprint<TimeTag>> CreateSelectionBlueprintContainer()
            => new TimeTagEditorSelectionBlueprintContainer { RelativeSizeAxes = Axes.Both };

        protected override SelectionHandler<TimeTag> CreateSelectionHandler()
            => new TimeTagEditorSelectionHandler();

        protected override SelectionBlueprint<TimeTag> CreateBlueprintFor(TimeTag item)
            => new TimeTagEditorHitObjectBlueprint(item);

        protected override DragBox CreateDragBox(Action<RectangleF> performSelect) => new TimelineDragBox(performSelect);

        protected class TimeTagEditorSelectionHandler : SelectionHandler<TimeTag>
        {
            [Resolved]
            private LyricManager lyricManager { get; set; }

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<TimeTag> moveEvent) => true;

            protected override void DeleteItems(IEnumerable<TimeTag> items)
            {
                // todo : delete time-line
                foreach (var item in items)
                {
                    lyricManager.RemoveTimeTag(item);
                }
            }
        }

        private class TimelineDragBox : DragBox
        {
            // the following values hold the start and end X positions of the drag box in the timeline's local space,
            // but with zoom unapplied in order to be able to compensate for positional changes
            // while the timeline is being zoomed in/out.
            private float? selectionStart;
            private float selectionEnd;

            [Resolved]
            private TimeTagEditor timeline { get; set; }

            public TimelineDragBox(Action<RectangleF> performSelect)
                : base(performSelect)
            {
            }

            protected override Drawable CreateBox() => new Box
            {
                RelativeSizeAxes = Axes.Y,
                Alpha = 0.3f
            };

            public override bool HandleDrag(MouseButtonEvent e)
            {
                selectionStart ??= e.MouseDownPosition.X / timeline.CurrentZoom;

                // only calculate end when a transition is not in progress to avoid bouncing.
                if (Precision.AlmostEquals(timeline.CurrentZoom, timeline.Zoom))
                    selectionEnd = e.MousePosition.X / timeline.CurrentZoom;

                updateDragBoxPosition();
                return true;
            }

            private void updateDragBoxPosition()
            {
                if (selectionStart == null)
                    return;

                float rescaledStart = selectionStart.Value * timeline.CurrentZoom;
                float rescaledEnd = selectionEnd * timeline.CurrentZoom;

                Box.X = Math.Min(rescaledStart, rescaledEnd);
                Box.Width = Math.Abs(rescaledStart - rescaledEnd);

                var boxScreenRect = Box.ScreenSpaceDrawQuad.AABBFloat;

                // we don't care about where the hitobjects are vertically. in cases like stacking display, they may be outside the box without this adjustment.
                boxScreenRect.Y -= boxScreenRect.Height;
                boxScreenRect.Height *= 2;

                PerformSelection?.Invoke(boxScreenRect);
            }

            public override void Hide()
            {
                base.Hide();
                selectionStart = null;
            }
        }

        protected class TimeTagEditorSelectionBlueprintContainer : Container<SelectionBlueprint<TimeTag>>
        {
            protected override Container<SelectionBlueprint<TimeTag>> Content { get; }

            public TimeTagEditorSelectionBlueprintContainer()
            {
                AddInternal(new TimelinePart<SelectionBlueprint<TimeTag>>(Content = new TimeTagOrderedSelectionContainer { RelativeSizeAxes = Axes.Both }) { RelativeSizeAxes = Axes.Both });
            }
        }
    }
}
