﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings;

public partial class PitchSettings : PlayerSettingsGroup
{
    private readonly ClickablePlayerSliderBar pitchSliderBar;
    private readonly ClickablePlayerSliderBar vocalPitchSliderBar;
    private readonly ClickablePlayerSliderBar scoringPitchSliderBar;

    public PitchSettings()
        : base("Pitch")
    {
        Children = new Drawable[]
        {
            new OsuSpriteText
            {
                Text = "Pitch:"
            },
            pitchSliderBar = new ClickablePlayerSliderBar(),
            new OsuSpriteText
            {
                Text = "Vocal pitch:"
            },
            vocalPitchSliderBar = new ClickablePlayerSliderBar(),
            new OsuSpriteText
            {
                Text = "Scoring pitch:"
            },
            scoringPitchSliderBar = new ClickablePlayerSliderBar()
        };
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeSessionStatics session)
    {
        pitchSliderBar.Current = session.GetBindable<int>(KaraokeRulesetSession.Pitch);
        vocalPitchSliderBar.Current = session.GetBindable<int>(KaraokeRulesetSession.VocalPitch);
        scoringPitchSliderBar.Current = session.GetBindable<int>(KaraokeRulesetSession.ScoringPitch);
    }
}
