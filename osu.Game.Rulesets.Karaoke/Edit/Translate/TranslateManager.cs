﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    /// <summary>
    /// Handle create or delete language.
    /// </summary>
    public class TranslateManager : Component
    {
        public readonly BindableList<CultureInfo> Languages = new BindableList<CultureInfo>();

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(canBeNull: true)]
        private IEditorChangeHandler changeHandler { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (beatmap?.PlayableBeatmap is KaraokeBeatmap karaokeBeatmap)
            {
                Languages.AddRange(karaokeBeatmap.AvailableTranslates);
                Languages.BindCollectionChanged((a, b) => { karaokeBeatmap.AvailableTranslates = Languages.ToArray(); });
            }
        }

        public void AddLanguage(CultureInfo language)
        {
            changeHandler?.BeginChange();
            Languages.Add(language);
            changeHandler?.EndChange();
        }

        public void RemoveLanguage(CultureInfo cultureInfo)
        {
            changeHandler?.BeginChange();
            Languages.Remove(cultureInfo);
            changeHandler?.EndChange();
        }

        public string GetTranslate(Lyric lyric, CultureInfo cultureInfo)
        {
            if (lyric.Translates.TryGetValue(cultureInfo, out string translate))
                return translate;

            return null;
        }

        public void SaveTranslate(Lyric lyric, CultureInfo cultureInfo, string translate)
        {
            changeHandler?.BeginChange();

            // should not save translate if is null or empty or whitespace
            if (string.IsNullOrWhiteSpace(translate))
            {
                if (lyric.Translates.ContainsKey(cultureInfo))
                    lyric.Translates.Remove(cultureInfo);
            }
            else
            {
                if (!lyric.Translates.TryAdd(cultureInfo, translate))
                    lyric.Translates[cultureInfo] = translate;
            }

            changeHandler?.EndChange();
        }

        public bool IsLanguageContainsTranslate(CultureInfo cultureInfo)
            => LanguageContainsTranslateAmount(cultureInfo) > 0;

        public int LanguageContainsTranslateAmount(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                return 0;

            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            return lyrics.Count(x => x.Translates.ContainsKey(cultureInfo));
        }
    }
}
