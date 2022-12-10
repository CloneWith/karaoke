﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags
{
    public partial class TimeTagCreateConfigSubsection : FillFlowContainer, IHasCurrentValue<CreateTimeTagEditMode>
    {
        private const int button_vertical_margin = 20;
        private const int horizontal_padding = 20;
        private const int corner_radius = 15;

        private readonly EditModeButton[] buttons;
        private readonly LyricEditorDescriptionTextFlowContainer lyricEditorDescription;

        private readonly BindableWithCurrent<CreateTimeTagEditMode> current = new();

        public Bindable<CreateTimeTagEditMode> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        [Resolved]
        private OsuColour colours { get; set; }

        private readonly Box background;

        public TimeTagCreateConfigSubsection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Spacing = new Vector2(10);

            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Masking = true,
                    CornerRadius = corner_radius,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            RowDimensions = new[]
                            {
                                new Dimension(GridSizeMode.AutoSize)
                            },
                            Content = new[]
                            {
                                buttons = EnumUtils.GetValues<CreateTimeTagEditMode>().Select(x => new EditModeButton(x)
                                {
                                    Text = getButtonTitle(x),
                                    Margin = new MarginPadding { Vertical = button_vertical_margin },
                                    Padding = new MarginPadding { Horizontal = horizontal_padding },
                                    Action = () => current.Value = x,
                                }).ToArray(),
                            }
                        }
                    }
                },
                lyricEditorDescription = new LyricEditorDescriptionTextFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding { Horizontal = horizontal_padding },
                }
            };

            current.BindValueChanged(e =>
            {
                Schedule(() =>
                {
                    updateEditMode(e.NewValue);
                });
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
        {
            background.Colour = colourProvider.Background4(state.Mode);
        }

        private void updateEditMode(CreateTimeTagEditMode mode)
        {
            // update button style.
            foreach (var button in buttons)
            {
                bool highLight = EqualityComparer<CreateTimeTagEditMode>.Default.Equals(button.Mode, mode);
                button.Alpha = highLight ? 0.8f : 0.4f;
                button.BackgroundColour = getButtonColour(button.Mode, highLight);
            }

            // update description text.
            lyricEditorDescription.Description = getDescription(mode);
        }

        private LocalisableString getButtonTitle(CreateTimeTagEditMode mode)
            => mode switch
            {
                CreateTimeTagEditMode.Create => "Create",
                CreateTimeTagEditMode.Modify => "Modify",
                _ => throw new InvalidOperationException(nameof(mode))
            };

        private Color4 getButtonColour(CreateTimeTagEditMode mode, bool active)
            => mode switch
            {
                CreateTimeTagEditMode.Create => active ? colours.Green : colours.GreenDarker,
                CreateTimeTagEditMode.Modify => active ? colours.Pink : colours.PinkDarker,
                _ => throw new InvalidOperationException(nameof(mode))
            };

        private DescriptionFormat getDescription(CreateTimeTagEditMode mode) =>
            mode switch
            {
                CreateTimeTagEditMode.Create => new DescriptionFormat
                {
                    Text =
                        $"Use keyboard to control caret position, press [{DescriptionFormat.LINK_KEY_ACTION}](create_time_tag) to create new time-tag and press [{DescriptionFormat.LINK_KEY_ACTION}](remove_time_tag) to delete exist time-tag.",
                    Actions = new Dictionary<string, IDescriptionAction>
                    {
                        { "create_time_tag", new InputKeyDescriptionAction { AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.CreateTimeTag } } },
                        { "remove_time_tag", new InputKeyDescriptionAction { AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.RemoveTimeTag } } }
                    }
                },
                CreateTimeTagEditMode.Modify => new DescriptionFormat
                {
                    Text =
                        $"Press [{DescriptionFormat.LINK_KEY_ACTION}](move_time_tag_position) to move the time-tag position. Press press [{DescriptionFormat.LINK_KEY_ACTION}](create_time_tag) to create new time-tag and [{DescriptionFormat.LINK_KEY_ACTION}](remove_time_tag) to delete exist time-tag.",
                    Actions = new Dictionary<string, IDescriptionAction>
                    {
                        {
                            "move_time_tag_position", new InputKeyDescriptionAction
                            {
                                Text = "Shifting Time-tag index keys.",
                                AdjustableActions = new List<KaraokeEditAction>
                                {
                                    KaraokeEditAction.ShiftTheTimeTagLeft,
                                    KaraokeEditAction.ShiftTheTimeTagRight,
                                    KaraokeEditAction.ShiftTheTimeTagStateLeft,
                                    KaraokeEditAction.ShiftTheTimeTagStateRight,
                                }
                            }
                        },
                        { "create_time_tag", new InputKeyDescriptionAction { AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.CreateTimeTag } } },
                        { "remove_time_tag", new InputKeyDescriptionAction { AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.RemoveTimeTag } } }
                    }
                },
                _ => throw new InvalidOperationException(nameof(mode))
            };

        private partial class EditModeButton : OsuButton
        {
            public CreateTimeTagEditMode Mode { get; }

            public EditModeButton(CreateTimeTagEditMode mode)
            {
                Mode = mode;
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;
            }
        }
    }
}
