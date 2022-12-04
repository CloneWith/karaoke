﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics
{
    public class TimeTagLayer : BaseLayer
    {
        [Resolved, AllowNull]
        private InteractableKaraokeSpriteText karaokeSpriteText { get; set; }

        private readonly IBindableList<TimeTag> timeTagsBindable = new BindableList<TimeTag>();

        public TimeTagLayer(Lyric lyric)
            : base(lyric)
        {
            timeTagsBindable.BindCollectionChanged((_, _) =>
            {
                ScheduleAfterChildren(updateTimeTags);
            });

            timeTagsBindable.BindTo(lyric.TimeTagsBindable);
        }

        private void updateTimeTags()
        {
            ClearInternal();

            foreach (var timeTag in timeTagsBindable)
            {
                var position = karaokeSpriteText.GetTimeTagPosition(timeTag);
                AddInternal(new DrawableTimeTag(timeTag)
                {
                    Position = position
                });
            }
        }

        public override void UpdateDisableEditState(bool editable)
        {
            this.FadeTo(editable ? 1 : 0.5f, 100);
        }
    }
}
