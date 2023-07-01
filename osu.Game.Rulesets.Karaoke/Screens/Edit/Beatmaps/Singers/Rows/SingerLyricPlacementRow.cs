﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Utils;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Detail;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows.Components;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows.Components.Timeline;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows;

public partial class SingerLyricPlacementColumn : LyricPlacementColumn
{
    public SingerLyricPlacementColumn(Singer singer)
        : base(singer)
    {
    }

    protected override Drawable CreateSingerInfo(Singer singer)
        => new DrawableSingerInfo(singer);

    protected override Drawable CreateTimeLinePart(Singer singer)
        => new SingerLyricTimeline(singer);

    internal partial class DrawableSingerInfo : CompositeDrawable, IHasCustomTooltip<Singer>, IHasContextMenu, IHasPopover
    {
        private const int avatar_size = 48;
        private const int main_text_size = 24;
        private const int sub_text_size = 12;

        [Resolved]
        private IBeatmapSingersChangeHandler beatmapSingersChangeHandler { get; set; } = null!;

        [Resolved]
        private IDialogOverlay dialogOverlay { get; set; } = null!;

        private readonly IBindable<int> bindableOrder = new Bindable<int>();
        private readonly IBindable<float> bindableHue = new Bindable<float>();
        private readonly IBindable<string> bindableSingerName = new Bindable<string>();
        private readonly IBindable<string> bindableEnglishName = new Bindable<string>();

        private readonly Singer singer;

        public DrawableSingerInfo(Singer singer)
        {
            this.singer = singer;

            bindableOrder.BindTo(singer.OrderBindable);
            bindableHue.BindTo(singer.HueBindable);
            bindableSingerName.BindTo(singer.NameBindable);
            bindableEnglishName.BindTo(singer.EnglishNameBindable);

            Box background;
            OsuSpriteText singerName;
            OsuSpriteText singerEnglishName;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(10) { Right = 0 },
                    Child = new GridContainer
                    {
                        Name = "Basic info",
                        RelativeSizeAxes = Axes.X,
                        Height = avatar_size,
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.Absolute, avatar_size),
                            new Dimension(),
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new SingerAvatar(singer)
                                {
                                    Name = "Avatar",
                                    Size = new Vector2(avatar_size),
                                },
                                new FillFlowContainer
                                {
                                    Name = "Singer name",
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding { Left = 5 },
                                    Spacing = new Vector2(1),
                                    Children = new Drawable[]
                                    {
                                        singerName = new TruncatingSpriteText
                                        {
                                            Name = "Singer name",
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: main_text_size),
                                            RelativeSizeAxes = Axes.X,
                                        },
                                        singerEnglishName = new TruncatingSpriteText
                                        {
                                            Name = "English name",
                                            Font = OsuFont.GetFont(weight: FontWeight.Bold, size: sub_text_size),
                                            RelativeSizeAxes = Axes.X,
                                        }
                                    }
                                }
                            }
                        }
                    },
                }
            };

            bindableOrder.BindValueChanged(_ =>
            {
                singerName.Text = $"#{singer.Order} {singer.Name}";
            }, true);

            bindableHue.BindValueChanged(_ =>
            {
                // background
                background.Colour = SingerUtils.GetBackgroundColour(singer);
            }, true);

            bindableSingerName.BindValueChanged(_ =>
            {
                singerName.Text = $"#{singer.Order} {singer.Name}";
            }, true);

            bindableEnglishName.BindValueChanged(_ =>
            {
                singerEnglishName.Text = singer.EnglishName;
            }, true);
        }

        public ITooltip<Singer> GetCustomTooltip() => new SingerToolTip();

        public Singer TooltipContent => singer;

        public MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem("Edit singer info", MenuItemType.Standard, this.ShowPopover),
            new OsuMenuItem("Delete", MenuItemType.Destructive, () =>
            {
                dialogOverlay.Push(new DeleteSingerDialog(isOk =>
                {
                    if (isOk)
                        beatmapSingersChangeHandler.Remove(singer);
                }));
            }),
        };

        protected override bool OnClick(ClickEvent e)
        {
            this.ShowPopover();
            return base.OnClick(e);
        }

        public Popover GetPopover()
            => new SingerEditPopover(singer);
    }
}
