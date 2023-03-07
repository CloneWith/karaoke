﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Ja
{
    public class JaTimeTagGenerator : TimeTagGenerator<JaTimeTagGeneratorConfig>
    {
        public JaTimeTagGenerator(JaTimeTagGeneratorConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Thanks for RhythmKaTTE's author writing this logic into C#
        /// http://juna-idler.blogspot.com/2016/05/rhythmkatte-version-01.html
        /// </summary>
        protected override void TimeTagLogic(Lyric lyric, List<TimeTag> timeTags)
        {
            timeTags.AddRange(generateTimeTagByText(lyric.Text));

            foreach (var ruby in lyric.RubyTags)
            {
                // remove exist time tag
                timeTags.RemoveAll(x => x.Index.Index > ruby.StartIndex && x.Index.Index < ruby.EndIndex);

                // add new time tags created from ruby
                var rubyTags = generateTimeTagByText(ruby.Text);
                var shiftingTimeTags = rubyTags.Select((x, _) => new TimeTag(new TextIndex(ruby.StartIndex, x.Index.State), x.Time));
                timeTags.AddRange(shiftingTimeTags);
            }
        }

        private IEnumerable<TimeTag> generateTimeTagByText(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;

            for (int i = 1; i < text.Length; i++)
            {
                char c = text[i];
                char pc = text[i - 1];

                if (CharUtils.IsSpacing(c) && Config.CheckWhiteSpace.Value)
                {
                    // ignore continuous white space.
                    if (CharUtils.IsSpacing(pc))
                        continue;

                    var timeTag = Config.CheckWhiteSpaceKeyUp.Value
                        ? new TimeTag(new TextIndex(i - 1, TextIndex.IndexState.End))
                        : new TimeTag(new TextIndex(i));

                    if (CharUtils.IsLatin(pc))
                    {
                        if (Config.CheckWhiteSpaceAlphabet.Value)
                            yield return timeTag;
                    }
                    else if (char.IsDigit(pc))
                    {
                        if (Config.CheckWhiteSpaceDigit.Value)
                            yield return timeTag;
                    }
                    else if (CharUtils.IsAsciiSymbol(pc))
                    {
                        if (Config.CheckWhiteSpaceAsciiSymbol.Value)
                            yield return timeTag;
                    }
                    else
                    {
                        yield return timeTag;
                    }
                }
                else if (CharUtils.IsLatin(c) || char.IsNumber(c) || CharUtils.IsAsciiSymbol(c))
                {
                    if (CharUtils.IsSpacing(pc) || (!CharUtils.IsLatin(pc) && !char.IsNumber(pc) && !CharUtils.IsAsciiSymbol(pc)))
                    {
                        yield return new TimeTag(new TextIndex(i));
                    }
                }
                else if (CharUtils.IsSpacing(pc))
                {
                    yield return new TimeTag(new TextIndex(i));
                }
                else
                {
                    switch (c)
                    {
                        case 'ゃ':
                        case 'ゅ':
                        case 'ょ':
                        case 'ャ':
                        case 'ュ':
                        case 'ョ':
                        case 'ぁ':
                        case 'ぃ':
                        case 'ぅ':
                        case 'ぇ':
                        case 'ぉ':
                        case 'ァ':
                        case 'ィ':
                        case 'ゥ':
                        case 'ェ':
                        case 'ォ':
                        case 'ー':
                        case '～':
                            break;

                        case 'ん':
                            if (Config.Checkん.Value)
                                yield return new TimeTag(new TextIndex(i));

                            break;

                        case 'っ':
                            if (Config.Checkっ.Value)
                                yield return new TimeTag(new TextIndex(i));

                            break;

                        default:
                            yield return new TimeTag(new TextIndex(i));

                            break;
                    }
                }
            }
        }
    }
}
