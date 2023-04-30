﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar;

/// <summary>
/// Button that able to change the boolean config in the <see cref="KaraokeRulesetLyricEditorConfigManager"/>
/// </summary>
public abstract partial class LyricEditorConfigButton : ToggleButton
{
    protected LyricEditorConfigButton()
    {
        SetIcon(Icon);
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager)
    {
        lyricEditorConfigManager.BindWith(Setting, Active);
    }

    protected abstract KaraokeRulesetLyricEditorSetting Setting { get; }

    protected abstract IconUsage Icon { get; }
}
