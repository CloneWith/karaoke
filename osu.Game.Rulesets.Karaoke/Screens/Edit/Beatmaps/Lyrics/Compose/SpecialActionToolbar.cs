﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.Carets;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.Panels;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.Playback;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.TimeTags;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.View;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose
{
    public partial class SpecialActionToolbar : CompositeDrawable
    {
        public const int HEIGHT = 26;
        public const int PADDING = 2;
        public const int ICON_SIZE = HEIGHT - PADDING * 2;

        public const int SPACING = 5;

        private readonly IBindable<ModeWithSubMode> bindableModeAndSubMode = new Bindable<ModeWithSubMode>();

        private readonly Box background;

        private readonly FillFlowContainer buttonContainer;

        public SpecialActionToolbar()
        {
            AutoSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                buttonContainer = new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Padding = new MarginPadding(5),
                    Spacing = new Vector2(SPACING),
                }
            };
        }

        [BackgroundDependencyLoader(true)]
        private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
        {
            bindableModeAndSubMode.BindTo(state.BindableModeAndSubMode);
            bindableModeAndSubMode.BindValueChanged(e =>
            {
                reGenerateButtons();

                if (ValueChangedEventUtils.EditModeChanged(e) || !IsLoaded)
                    background.Colour = colourProvider.Background2(state.Mode);
            }, true);
        }

        private void reGenerateButtons()
        {
            buttonContainer.Clear();

            buttonContainer.AddRange(createAdjustLyricSizeItem());

            buttonContainer.Add(new Separator());

            buttonContainer.AddRange(createPanelItems());

            buttonContainer.Add(new Separator());

            buttonContainer.AddRange(createSwitchLyricItem());

            buttonContainer.Add(new Separator());

            var modeWithSubMode = bindableModeAndSubMode.Value;
            buttonContainer.AddRange(createItemForEditMode(modeWithSubMode));
        }

        private static IEnumerable<Drawable> createAdjustLyricSizeItem() => new Drawable[]
        {
            new AdjustFontSizeButton(),
        };

        private static IEnumerable<Drawable> createPanelItems() => new Drawable[]
        {
            new TogglePropertyPanelButton(),
            new ToggleInvalidInfoPanelButton(),
        };

        private static IEnumerable<Drawable> createSwitchLyricItem() => new Drawable[]
        {
            new MoveToPreviousLyricButton(),
            new MoveToNextLyricButton(),
        };

        private static IEnumerable<Drawable> createItemForEditMode(ModeWithSubMode modeWithSubMode)
        {
            return modeWithSubMode.Mode switch
            {
                LyricEditorMode.View => Array.Empty<Drawable>(),
                LyricEditorMode.Texting => Array.Empty<Drawable>(),
                LyricEditorMode.Reference => Array.Empty<Drawable>(),
                LyricEditorMode.Language => Array.Empty<Drawable>(),
                LyricEditorMode.EditRuby => Array.Empty<Drawable>(),
                LyricEditorMode.EditRomaji => Array.Empty<Drawable>(),
                LyricEditorMode.EditTimeTag => modeWithSubMode.SubMode is TimeTagEditMode timeTagEditMode ? createItemsForEditTimeTagMode(timeTagEditMode) : throw new InvalidCastException(),
                LyricEditorMode.EditNote => modeWithSubMode.SubMode is NoteEditMode noteEditMode ? createItemsForEditNoteMode(noteEditMode) : throw new InvalidCastException(),
                LyricEditorMode.Singer => Array.Empty<Drawable>(),
                _ => throw new ArgumentOutOfRangeException()
            };

            static IEnumerable<Drawable> createItemsForEditTimeTagMode(TimeTagEditMode timeTagEditMode) =>
                timeTagEditMode switch
                {
                    TimeTagEditMode.Create => new Drawable[]
                    {
                        new MoveToFirstIndexButton(),
                        new MoveToPreviousIndexButton(),
                        new MoveToNextIndexButton(),
                        new MoveToLastIndexButton(),
                        new CreateTimeTagButton(),
                        new RemoveTimeTagButton(),
                    },
                    TimeTagEditMode.Recording => new Drawable[]
                    {
                        new PlaybackSwitchButton(),
                        new Separator(),
                        new MoveToFirstIndexButton(),
                        new MoveToPreviousIndexButton(),
                        new MoveToNextIndexButton(),
                        new MoveToLastIndexButton(),
                        new SetTimeTagTimeButton(),
                        new ClearTimeTagTimeButton(),
                        new ClearAllTimeTagTimeButton(),
                    },
                    TimeTagEditMode.Adjust => new Drawable[]
                    {
                        new PlaybackSwitchButton(),
                    },
                    _ => throw new ArgumentOutOfRangeException(nameof(timeTagEditMode), timeTagEditMode, null)
                };

            static IEnumerable<Drawable> createItemsForEditNoteMode(NoteEditMode noteEditMode) =>
                noteEditMode switch
                {
                    NoteEditMode.Generate => Array.Empty<Drawable>(),
                    NoteEditMode.Edit => Array.Empty<Drawable>(),
                    NoteEditMode.Verify => Array.Empty<Drawable>(),
                    _ => throw new ArgumentOutOfRangeException(nameof(noteEditMode), noteEditMode, null)
                };
        }
    }
}
