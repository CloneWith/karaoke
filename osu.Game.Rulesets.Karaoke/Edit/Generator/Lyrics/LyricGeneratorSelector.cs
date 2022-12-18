﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Types;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics
{
    public abstract class LyricGeneratorSelector<TProperty, TBaseConfig> : ILyricPropertyGenerator<TProperty>
    {
        protected Dictionary<CultureInfo, Lazy<ILyricPropertyGenerator<TProperty>>> Generator { get; } = new();

        private readonly KaraokeRulesetEditGeneratorConfigManager generatorConfigManager;

        protected LyricGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        {
            this.generatorConfigManager = generatorConfigManager;
        }

        protected void RegisterGenerator<TGenerator, TConfig>(CultureInfo info) where TGenerator : ILyricPropertyGenerator<TProperty> where TConfig : TBaseConfig, new()
        {
            Generator.Add(info, new Lazy<ILyricPropertyGenerator<TProperty>>(() =>
            {
                var generatorSetting = GetGeneratorConfigSetting(info);
                var config = generatorConfigManager.Get<TConfig>(generatorSetting);
                if (Activator.CreateInstance(typeof(TGenerator), config) is not ILyricPropertyGenerator<TProperty> generator)
                    throw new InvalidCastException();

                return generator;
            }));
        }

        protected abstract KaraokeRulesetEditGeneratorSetting GetGeneratorConfigSetting(CultureInfo info);

        public LocalisableString? GetInvalidMessage(Lyric lyric)
        {
            if (lyric.Language == null)
                return "Oops, language is missing.";

            var generator = Generator.FirstOrDefault(g => EqualityComparer<CultureInfo>.Default.Equals(g.Key, lyric.Language));
            if (generator.Key == null)
                return "Sorry, the language of lyric is not supported yet.";

            return generator.Value.Value.GetInvalidMessage(lyric);
        }

        public abstract TProperty Generate(Lyric lyric);
    }
}
