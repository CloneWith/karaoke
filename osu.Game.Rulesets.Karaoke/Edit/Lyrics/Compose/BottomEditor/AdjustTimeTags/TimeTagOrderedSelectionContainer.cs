﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.AdjustTimeTags
{
    /// <summary>
    /// A container for <see cref="SelectionBlueprint{T}"/> ordered by their <see cref="TimeTag"/> start times.
    /// </summary>
    public class TimeTagOrderedSelectionContainer : Container<SelectionBlueprint<TimeTag>>
    {
        public override void Add(SelectionBlueprint<TimeTag> drawable)
        {
            base.Add(drawable);
            bindStartTime(drawable);
        }

        public override bool Remove(SelectionBlueprint<TimeTag> drawable, bool disposeImmediately)
        {
            if (!base.Remove(drawable, disposeImmediately))
                return false;

            unbindStartTime(drawable);
            return true;
        }

        public override void Clear(bool disposeChildren)
        {
            base.Clear(disposeChildren);
            unbindAllStartTimes();
        }

        private readonly Dictionary<SelectionBlueprint<TimeTag>, IBindable> startTimeMap = new();

        private void bindStartTime(SelectionBlueprint<TimeTag> blueprint)
        {
            var bindable = blueprint.Item.TimeBindable.GetBoundCopy();

            bindable.BindValueChanged(_ =>
            {
                if (LoadState >= LoadState.Ready)
                    SortInternal();
            });

            startTimeMap[blueprint] = bindable;
        }

        private void unbindStartTime(SelectionBlueprint<TimeTag> blueprint)
        {
            startTimeMap[blueprint].UnbindAll();
            startTimeMap.Remove(blueprint);
        }

        private void unbindAllStartTimes()
        {
            foreach (var kvp in startTimeMap)
                kvp.Value.UnbindAll();
            startTimeMap.Clear();
        }

        protected override int Compare(Drawable x, Drawable y)
        {
            var xObj = ((SelectionBlueprint<TimeTag>)x).Item;
            var yObj = ((SelectionBlueprint<TimeTag>)y).Item;

            double xTime = xObj.Time ?? 0;
            double yTime = yObj.Time ?? 0;

            // Put earlier blueprints towards the end of the list, so they handle input first
            int result = yTime.CompareTo(xTime);
            if (result != 0) return result;

            return CompareReverseChildID(x, y);
        }
    }
}
