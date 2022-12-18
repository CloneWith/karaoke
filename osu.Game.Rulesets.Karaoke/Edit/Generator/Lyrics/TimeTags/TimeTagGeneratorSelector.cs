﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags
{
    public class TimeTagGeneratorSelector : LyricGeneratorSelector<TimeTag[], TimeTagGeneratorConfig>
    {
        public TimeTagGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
            : base(generatorConfigManager)
        {
            RegisterGenerator<JaTimeTagGenerator, JaTimeTagGeneratorConfig>(new CultureInfo(17));
            RegisterGenerator<JaTimeTagGenerator, JaTimeTagGeneratorConfig>(new CultureInfo(1041));
            RegisterGenerator<ZhTimeTagGenerator, ZhTimeTagGeneratorConfig>(new CultureInfo(1028));
        }

        public override TimeTag[] Generate(Lyric lyric)
        {
            if (lyric.Language == null)
                return Array.Empty<TimeTag>();

            if (string.IsNullOrEmpty(lyric.Text))
                return Array.Empty<TimeTag>();

            if (!Generator.TryGetValue(lyric.Language, out var generator))
                return Array.Empty<TimeTag>();

            return generator.Value.Generate(lyric);
        }

        protected override KaraokeRulesetEditGeneratorSetting GetGeneratorConfigSetting(CultureInfo info) =>
            info.LCID switch
            {
                17 => KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig,
                1041 => KaraokeRulesetEditGeneratorSetting.JaTimeTagGeneratorConfig,
                1028 => KaraokeRulesetEditGeneratorSetting.ZhTimeTagGeneratorConfig,
                _ => throw new ArgumentOutOfRangeException(nameof(info))
            };
    }
}
