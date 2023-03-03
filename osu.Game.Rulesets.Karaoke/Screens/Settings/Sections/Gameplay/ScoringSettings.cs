﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Gameplay
{
    public partial class ScoringSettings : KaraokeSettingsSubsection
    {
        protected override LocalisableString Header => "Scoring";

        [BackgroundDependencyLoader]
        private void load()
        {
            // todo : should separate scoring and pitch part?
            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Override pitch at gameplay",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.OverridePitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Pitch",
                    Current = Config.GetBindable<int>(KaraokeRulesetSetting.Pitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override vocal pitch at gameplay",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.OverrideVocalPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "Vocal pitch",
                    Current = Config.GetBindable<int>(KaraokeRulesetSetting.VocalPitch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Override scoring pitch at gameplay",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.OverrideScoringPitchAtGameplay)
                },
                new SettingsSlider<int, PitchSlider>
                {
                    LabelText = "scoring pitch",
                    Current = Config.GetBindable<int>(KaraokeRulesetSetting.ScoringPitch)
                },
            };
        }

        private partial class PitchSlider : RoundedSliderBar<int>
        {
            public override LocalisableString TooltipText => (Current.Value >= 0 ? "+" : string.Empty) + Current.Value.ToString("N0");
        }
    }
}
