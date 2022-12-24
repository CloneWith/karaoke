﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.RubyTags
{
    public class RubyTagGeneratorSelectorTest : BaseLyricGeneratorSelectorTest<RubyTagGeneratorSelector, RubyTag[]>
    {
        [TestCase(17, "花火大会", new[] { "[0,2]:はなび", "[2,4]:たいかい" })] // Japanese
        [TestCase(1041, "はなび", new string[] { })] // Japanese
        [TestCase(1028, "はなび", new string[] { })] // Chinese(should not supported)
        public void TestGenerate(int lcid, string text, string[] expectedRubies)
        {
            var selector = CreateSelector();
            var lyric = new Lyric
            {
                Language = new CultureInfo(lcid),
                Text = text,
            };

            var expected = TestCaseTagHelper.ParseRubyTags(expectedRubies);
            CheckGenerateResult(lyric, expected, selector);
        }

        protected override void AssertEqual(RubyTag[] expected, RubyTag[] actual)
        {
            TextTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
