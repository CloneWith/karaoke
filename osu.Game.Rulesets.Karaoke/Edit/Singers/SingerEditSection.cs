﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Singers.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerEditSection : CompositeDrawable
    {
        private SingerRearrangeableListContainer singerContainers;

        [BackgroundDependencyLoader]
        private void load(SingerManager singerManager)
        {
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 100),
                    new Dimension()
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new DefaultLyricPlacementColumn
                        {
                            Name = "Default",
                            RelativeSizeAxes = Axes.Both,
                        }
                    },
                    new Drawable[]
                    {
                        singerContainers = new SingerRearrangeableListContainer
                        {
                            Name = "List of singer",
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                }
            };

            singerManager.Singers.BindCollectionChanged((a, b) =>
            {
                var newSingers = singerManager.Singers.ToList();
                singerContainers.Items.Clear();
                singerContainers.Items.AddRange(newSingers);
            }, true);
        }
    }
}