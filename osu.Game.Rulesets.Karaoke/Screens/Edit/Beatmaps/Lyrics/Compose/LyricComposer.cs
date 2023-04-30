﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.EnumExtensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.BottomEditor;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Panels;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose;

public partial class LyricComposer : CompositeDrawable
{
    private readonly Bindable<PanelLayout> bindablePanelLayout = new();
    private readonly Bindable<BottomEditorType?> bindableBottomEditorType = new();

    private readonly IBindable<ModeWithSubMode> bindableModeAndSubMode = new Bindable<ModeWithSubMode>();

    private readonly IDictionary<PanelType, Bindable<bool>> panelStatus = new Dictionary<PanelType, Bindable<bool>>();
    private readonly IDictionary<PanelType, Panel> panelInstance = new Dictionary<PanelType, Panel>();

    private readonly IDictionary<PanelDirection, List<PanelType>> panelDirections = new Dictionary<PanelDirection, List<PanelType>>
    {
        { PanelDirection.Left, new List<PanelType>() },
        { PanelDirection.Right, new List<PanelType>() },
    };

    [Resolved, AllowNull]
    private LyricEditorColourProvider colourProvider { get; set; }

    private readonly GridContainer gridContainer;

    private readonly Container centerEditArea;
    private readonly Container mainEditorArea;

    private readonly Container<BaseBottomEditor> bottomEditorContainer;

    public LyricComposer()
    {
        Box centerEditorBackground;
        Box bottomEditorBackground;

        InternalChild = gridContainer = new GridContainer
        {
            RelativeSizeAxes = Axes.Both,
            Content = new[]
            {
                new Drawable[]
                {
                    centerEditArea = new Container
                    {
                        Name = "Edit area and action buttons",
                        RelativeSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            centerEditorBackground = new Box
                            {
                                Name = "Background",
                                RelativeSizeAxes = Axes.Both,
                            },
                            mainEditorArea = new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new LyricEditor(),
                                    new SpecialActionToolbar
                                    {
                                        Name = "Toolbar",
                                        Anchor = Anchor.BottomCentre,
                                        Origin = Anchor.BottomCentre,
                                    },
                                }
                            }
                        }
                    },
                },
                new Drawable[]
                {
                    new Container
                    {
                        Name = "Edit area and action buttons",
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        Children = new Drawable[]
                        {
                            bottomEditorBackground = new Box
                            {
                                Name = "Background",
                                RelativeSizeAxes = Axes.Both,
                            },
                            bottomEditorContainer = new Container<BaseBottomEditor>
                            {
                                RelativeSizeAxes = Axes.Both,
                            }
                        }
                    },
                }
            }
        };

        bindableModeAndSubMode.BindValueChanged(e =>
        {
            toggleChangeBottomEditor();

            Schedule(() =>
            {
                if (!ValueChangedEventUtils.EditModeChanged(e) && IsLoaded)
                    return;

                centerEditorBackground.Colour = colourProvider.Background1(e.NewValue.Mode);
                bottomEditorBackground.Colour = colourProvider.Background4(e.NewValue.Mode);
            });
        }, true);

        initializePanel();

        bindablePanelLayout.BindValueChanged(e =>
        {
            assignPanelPosition(e.NewValue);
        }, true);

        bindableBottomEditorType.BindValueChanged(e =>
        {
            assignBottomEditor(e.NewValue);
        }, true);

        foreach (var (type, bindable) in panelStatus)
        {
            bindable.BindValueChanged(e =>
            {
                bool show = e.NewValue;

                if (show)
                {
                    closeOtherPanelsInTheSameDirection(type);
                }

                togglePanel(type, show);
            }, true);
        }
    }

    [BackgroundDependencyLoader(true)]
    private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager, ILyricEditorState state, ITimeTagModeState timeTagModeState)
    {
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.ShowPropertyPanelInComposer, panelStatus[PanelType.Property]);
        lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.ShowInvalidInfoInComposer, panelStatus[PanelType.InvalidInfo]);

        bindableModeAndSubMode.BindTo(state.BindableModeAndSubMode);
    }

    protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
    {
        if (invalidation.HasFlagFast(Invalidation.DrawSize) && source == InvalidationSource.Parent)
            calculatePanelPosition();

        return base.OnInvalidate(invalidation, source);
    }

    #region Panel

    private void initializePanel()
    {
        foreach (var panelType in Enum.GetValues<PanelType>())
        {
            var instance = getInstance(panelType);

            panelStatus.Add(panelType, new Bindable<bool>(true));
            panelInstance.Add(panelType, instance);

            centerEditArea.Add(instance);
        }

        static Panel getInstance(PanelType panelType) =>
            panelType switch
            {
                PanelType.Property => new PropertyPanel(),
                PanelType.InvalidInfo => new InvalidPanel(),
                _ => throw new ArgumentOutOfRangeException(nameof(panelType), panelType, null)
            };
    }

    private void togglePanel(PanelType panel, bool show)
    {
        panelInstance[panel].State.Value = show ? Visibility.Visible : Visibility.Hidden;

        calculateLyricEditorSize();
    }

    private void calculatePanelPosition()
    {
        float radio = DrawWidth / DrawHeight;
        bindablePanelLayout.Value = radio < 2 ? PanelLayout.LeftOnly : PanelLayout.LeftAndRight;
    }

    private void assignPanelPosition(PanelLayout panelLayout)
    {
        panelDirections[PanelDirection.Left].Clear();
        panelDirections[PanelDirection.Right].Clear();

        switch (panelLayout)
        {
            case PanelLayout.LeftAndRight:
                panelDirections[PanelDirection.Left].Add(PanelType.Property);
                panelDirections[PanelDirection.Right].Add(PanelType.InvalidInfo);
                break;

            case PanelLayout.LeftOnly:
                panelDirections[PanelDirection.Left].Add(PanelType.Property);
                panelDirections[PanelDirection.Left].Add(PanelType.InvalidInfo);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(panelLayout), panelLayout, null);
        }

        foreach (var (direction, panelTypes) in panelDirections)
        {
            foreach (var instance in panelTypes.Select(panelType => panelInstance[panelType]))
            {
                instance.Direction = direction;
            }
        }

        closeOtherPanelsInTheSameDirection(PanelType.Property);
        calculateLyricEditorSize();
    }

    private void closeOtherPanelsInTheSameDirection(PanelType exceptPanel)
    {
        var closePanelList = panelDirections.First(x => x.Value.Contains(exceptPanel)).Value;

        foreach (var panel in closePanelList.Where(x => x != exceptPanel))
        {
            var status = panelStatus[panel];
            status.Value = false;
        }
    }

    private void calculateLyricEditorSize()
    {
        var padding = new MarginPadding();

        foreach (var (position, panelTypes) in panelDirections)
        {
            var instances = panelTypes.Select(panelType => panelInstance[panelType]).ToArray();
            float maxWidth = instances.Any() ? instances.Max(getWidth) : 0;

            switch (position)
            {
                case PanelDirection.Left:
                    padding.Left = maxWidth;
                    break;

                case PanelDirection.Right:
                    padding.Right = maxWidth;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
        }

        mainEditorArea.Padding = padding;

        static float getWidth(Panel panel)
            => panel.State.Value == Visibility.Visible ? panel.Width : 0;
    }

    #endregion

    #region Bottom editor

    private void toggleChangeBottomEditor()
    {
        var modeWithSubMode = bindableModeAndSubMode.Value;
        bindableBottomEditorType.Value = getBottomEditorType(modeWithSubMode);

        static BottomEditorType? getBottomEditorType(ModeWithSubMode modeWithSubMode) =>
            modeWithSubMode.Mode switch
            {
                LyricEditorMode.EditTimeTag when modeWithSubMode.SubMode is TimeTagEditMode.Recording => BottomEditorType.RecordingTimeTag,
                LyricEditorMode.EditTimeTag when modeWithSubMode.SubMode is TimeTagEditMode.Adjust => BottomEditorType.AdjustTimeTags,
                LyricEditorMode.EditNote => BottomEditorType.Note,
                _ => null
            };
    }

    private void assignBottomEditor(BottomEditorType? bottomEditorType)
    {
        const double remove_old_editor_time = 200;
        const double new_animation_time = 200;

        bool hasOldButtonEditor = bottomEditorContainer.Children.Any();
        var newButtonEditor = createBottomEditor(bottomEditorType).With(x =>
        {
            if (x == null)
                return;

            x.RelativePositionAxes = Axes.Y;
            x.Y = -1;
            x.Alpha = 0;
        });

        if (hasOldButtonEditor)
        {
            bottomEditorContainer.Children.ForEach(editor =>
            {
                editor.MoveToY(-1, remove_old_editor_time).FadeOut(remove_old_editor_time).OnComplete(x =>
                {
                    x.Expire();

                    updateBottomEditAreaSize(newButtonEditor);
                });
            });
        }
        else
        {
            updateBottomEditAreaSize(newButtonEditor);
        }

        if (newButtonEditor == null)
            return;

        bottomEditorContainer.Add(newButtonEditor);
        newButtonEditor.Delay(hasOldButtonEditor ? remove_old_editor_time : 0).FadeIn().MoveToY(0, new_animation_time);

        static BaseBottomEditor? createBottomEditor(BottomEditorType? bottomEditorType) =>
            bottomEditorType switch
            {
                BottomEditorType.RecordingTimeTag => new RecordingTimeTagBottomEditor(),
                BottomEditorType.AdjustTimeTags => new AdjustTimeTagBottomEditor(),
                BottomEditorType.Note => new NoteBottomEditor(),
                _ => null
            };

        void updateBottomEditAreaSize(BaseBottomEditor? bottomEditor)
        {
            float bottomEditorHeight = bottomEditor?.ContentHeight ?? 0;
            gridContainer.RowDimensions = new[]
            {
                new Dimension(),
                new Dimension(GridSizeMode.Absolute, bottomEditorHeight)
            };
        }
    }

    #endregion

    private enum PanelType
    {
        Property,

        InvalidInfo,
    }

    private enum PanelLayout
    {
        LeftAndRight,

        LeftOnly,
    }

    private enum BottomEditorType
    {
        RecordingTimeTag,

        AdjustTimeTags,

        Note,
    }
}
