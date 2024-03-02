﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Components.Menus;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Components.Menus;

public class LyricEditorPreferLayoutMenuItem : BindableEnumMenuItem<LyricEditorLayout>
{
    public LyricEditorPreferLayoutMenuItem(KaraokeRulesetLyricEditorConfigManager config, string text)
        : base(config.GetBindable<LyricEditorLayout>(KaraokeRulesetLyricEditorSetting.LyricEditorPreferLayout), text)
    {
    }
}
