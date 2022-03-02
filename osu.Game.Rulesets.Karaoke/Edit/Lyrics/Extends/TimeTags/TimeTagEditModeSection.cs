﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components.Description;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagEditModeSection : LyricEditorEditModeSection
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Orange;

        protected override Dictionary<LyricEditorMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    LyricEditorMode.CreateTimeTag,
                    new EditModeSelectionItem("Create", new DescriptionFormat
                    {
                        Text = "Use keyboard to control caret position, press [key](create_time_tag) to create new time-tag and press [key](remove_time_tag) to delete exist time-tag.",
                        Keys = new Dictionary<string, InputKey>
                        {
                            {
                                "create_time_tag", new InputKey
                                {
                                    AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.Create }
                                }
                            },
                            {
                                "remove_time_tag", new InputKey
                                {
                                    AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.Remove }
                                }
                            }
                        }
                    })
                },
                {
                    LyricEditorMode.RecordTimeTag, new EditModeSelectionItem("Recording", new DescriptionFormat
                    {
                        Text = "Press [key](set_time_tag_time) at the right time to set current time to time-tag. Press [key](clear_time_tag_time) to clear the time-tag time.",
                        Keys = new Dictionary<string, InputKey>
                        {
                            {
                                "set_time_tag_time", new InputKey
                                {
                                    AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.SetTime }
                                }
                            },
                            {
                                "clear_time_tag_time", new InputKey
                                {
                                    AdjustableActions = new List<KaraokeEditAction> { KaraokeEditAction.ClearTime }
                                }
                            }
                        }
                    })
                },
                {
                    LyricEditorMode.AdjustTimeTag, new EditModeSelectionItem("Adjust", "Drag to adjust time-tag time precisely.")
                }
            };

        protected override Color4 GetColour(OsuColour colours, LyricEditorMode mode, bool active)
        {
            return mode switch
            {
                LyricEditorMode.CreateTimeTag => active ? colours.Blue : colours.BlueDarker,
                LyricEditorMode.RecordTimeTag => active ? colours.Red : colours.RedDarker,
                LyricEditorMode.AdjustTimeTag => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
        }
    }
}
