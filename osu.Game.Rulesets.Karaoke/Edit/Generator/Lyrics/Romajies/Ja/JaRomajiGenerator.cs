﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ja;
using Lucene.Net.Analysis.TokenAttributes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies.Ja;

public class JaRomajiGenerator : RomajiGenerator<JaRomajiGeneratorConfig>
{
    private readonly Analyzer analyzer;

    public JaRomajiGenerator(JaRomajiGeneratorConfig config)
        : base(config)
    {
        analyzer = Analyzer.NewAnonymous((fieldName, reader) =>
        {
            Tokenizer tokenizer = new JapaneseTokenizer(reader, null, true, JapaneseTokenizerMode.SEARCH);
            return new TokenStreamComponents(tokenizer, new JapaneseReadingFormFilter(tokenizer, false));
        });
    }

    protected override IReadOnlyDictionary<TimeTag, RomajiGenerateResult> GenerateFromItem(Lyric item)
    {
        // Tokenize the text
        string text = item.Text;
        var tokenStream = analyzer.GetTokenStream("dummy", new StringReader(text));

        // get the processing tags.
        var processingRomajies = getProcessingRomajies(text, tokenStream, Config).ToArray();

        // then, trying to mapping them with the time-tags.
        return Convert(item.TimeTags, processingRomajies);
    }

    private static IEnumerable<RomajiGeneratorParameter> getProcessingRomajies(string text, TokenStream tokenStream, JaRomajiGeneratorConfig config)
    {
        // Reset the stream and convert all result
        tokenStream.Reset();

        while (true)
        {
            // Read next token
            tokenStream.ClearAttributes();
            tokenStream.IncrementToken();

            // Get result and offset
            var charTermAttribute = tokenStream.GetAttribute<ICharTermAttribute>();
            var offsetAttribute = tokenStream.GetAttribute<IOffsetAttribute>();

            // Get parsed result, result is Katakana.
            string katakana = charTermAttribute.ToString();
            if (string.IsNullOrEmpty(katakana))
                break;

            string parentText = text[offsetAttribute.StartOffset..offsetAttribute.EndOffset];
            bool fromKanji = JpStringUtils.ToKatakana(katakana) != JpStringUtils.ToKatakana(parentText);

            // Convert to romaji.
            string romaji = JpStringUtils.ToRomaji(katakana);
            if (config.Uppercase.Value)
                romaji = romaji.ToUpper();

            // Make tag
            yield return new RomajiGeneratorParameter
            {
                FromKanji = fromKanji,
                StartIndex = offsetAttribute.StartOffset,
                EndIndex = offsetAttribute.EndOffset - 1,
                RomajiText = romaji,
            };
        }

        // Dispose
        tokenStream.End();
        tokenStream.Dispose();
    }

    internal static IReadOnlyDictionary<TimeTag, RomajiGenerateResult> Convert(IList<TimeTag> timeTags, IList<RomajiGeneratorParameter> romajis)
    {
        var group = createGroup(timeTags, romajis);
        return group.ToDictionary(k => k.Key, x =>
        {
            bool isFirst = timeTags.IndexOf(x.Key) == 0; // todo: use better to mark the initial romaji.
            string romajiText = string.Join(" ", x.Value.Select(r => r.RomajiText));

            return new RomajiGenerateResult
            {
                InitialRomaji = isFirst,
                RomajiText = romajiText,
            };
        });

        static IReadOnlyDictionary<TimeTag, List<RomajiGeneratorParameter>> createGroup(IList<TimeTag> timeTags, IList<RomajiGeneratorParameter> romajis)
        {
            var dictionary = timeTags.ToDictionary(x => x, v => new List<RomajiGeneratorParameter>());

            int processedIndex = 0;

            foreach (var (timeTag, list) in dictionary)
            {
                while (processedIndex < romajis.Count && isTimeTagInRange(timeTags, timeTag, romajis[processedIndex]))
                {
                    list.Add(romajis[processedIndex]);
                    processedIndex++;
                }
            }

            if (processedIndex < romajis.Count - 1)
                throw new InvalidOperationException("Still have romajies that haven't process");

            return dictionary;
        }

        static bool isTimeTagInRange(IEnumerable<TimeTag> timeTags, TimeTag currentTimeTag, RomajiGeneratorParameter parameter)
        {
            if (currentTimeTag.Index.State == TextIndex.IndexState.End)
                return false;

            int romajiIndex = parameter.StartIndex;

            var nextTimeTag = timeTags.GetNextMatch(currentTimeTag, x => x.Index > currentTimeTag.Index && x.Index.State == TextIndex.IndexState.Start);
            if (nextTimeTag == null)
                return romajiIndex >= currentTimeTag.Index.Index;

            return romajiIndex >= currentTimeTag.Index.Index && romajiIndex < nextTimeTag.Index.Index;
        }
    }

    internal class RomajiGeneratorParameter
    {
        public bool FromKanji { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public string RomajiText { get; set; } = string.Empty;
    }
}
