﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.TimeTags.Zh
{
    public class ZhTimeTagGeneratorConfigPopover : GeneratorConfigPopover<ZhTimeTagGeneratorConfig>
    {
        protected override KaraokeRulesetEditGeneratorSetting Config => KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig;

        protected override GeneratorConfigSection[] CreateConfigSection(Bindable<ZhTimeTagGeneratorConfig> current)
        {
            return null;
        }
    }
}
