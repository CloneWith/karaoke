﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.RomajiTags.Ja
{
    public partial class JaRomajiTagGeneratorConfigDialog : RomajiTagGeneratorConfigDialog<JaRomajiTagGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.JaRomajiTagGeneratorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<JaRomajiTagGeneratorConfig> current)
        {
            return new GeneratorConfigSection[]
            {
                new GenericSection(current)
            };
        }
    }
}
