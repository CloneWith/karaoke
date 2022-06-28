// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricAutoGenerateChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricAutoGenerateChangeHandler, Lyric>
    {
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            baseDependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
            return baseDependencies;
        }

        #region Language

        [Test]
        public void TestDetectLanguage()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ"
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.DetectLanguage));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(new CultureInfo("ja"), h.Language);
            });
        }

        [Test]
        public void TestDetectLanguageWithNonSupportedLyric()
        {
            PrepareHitObject(new Lyric
            {
                Text = "???"
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.DetectLanguage));

            AssertSelectedHitObject(h =>
            {
                Assert.IsNull(h.Language);
            });
        }

        #endregion

        #region Ruby

        [Test]
        public void TestAutoGenerateRubyTags()
        {
            PrepareHitObject(new Lyric
            {
                Text = "風",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.AutoGenerateRubyTags));

            AssertSelectedHitObject(h =>
            {
                var rubyTags = h.RubyTags;
                Assert.AreEqual(1, rubyTags.Count);
                Assert.AreEqual("かぜ", rubyTags[0].Text);
            });
        }

        [Test]
        public void TestAutoGenerateRubyTagsWithNonSupportedLyric()
        {
            PrepareHitObjects(new[]
            {
                new Lyric
                {
                    Text = "風",
                },
                new Lyric
                {
                    Text = string.Empty,
                },
                new Lyric
                {
                    Text = string.Empty,
                    Language = new CultureInfo(17)
                },
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.AutoGenerateRubyTags));

            AssertSelectedHitObject(h =>
            {
                // should not able to generate time-tag if lyric text is empty, or did not have language.
                Assert.IsEmpty(h.RubyTags);
            });
        }

        #endregion

        #region Romaji

        [Test]
        public void TestAutoGenerateRomajiTags()
        {
            PrepareHitObject(new Lyric
            {
                Text = "風",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.AutoGenerateRomajiTags));

            AssertSelectedHitObject(h =>
            {
                var romajiTags = h.RomajiTags;
                Assert.AreEqual(1, romajiTags.Count);
                Assert.AreEqual("kaze", romajiTags[0].Text);
            });
        }

        [Test]
        public void TestAutoGenerateRomajiTagsWithNonSupportedLyric()
        {
            PrepareHitObjects(new[]
            {
                new Lyric
                {
                    Text = "風",
                },
                new Lyric
                {
                    Text = string.Empty,
                },
                new Lyric
                {
                    Text = string.Empty,
                    Language = new CultureInfo(17)
                },
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.AutoGenerateRomajiTags));

            AssertSelectedHitObject(h =>
            {
                // should not able to generate time-tag if lyric text is empty, or did not have language.
                Assert.IsEmpty(h.RomajiTags);
            });
        }

        #endregion

        #region TimeTag

        [Test]
        public void TestAutoGenerateTimeTags()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.AutoGenerateTimeTags));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(5, h.TimeTags.Count);
            });
        }

        [Test]
        public void TestAutoGenerateTimeTagsWithNonSupportedLyric()
        {
            PrepareHitObjects(new[]
            {
                new Lyric
                {
                    Text = "カラオケ",
                },
                new Lyric
                {
                    Text = string.Empty,
                },
                new Lyric
                {
                    Text = string.Empty,
                    Language = new CultureInfo(17)
                },
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.AutoGenerateTimeTags));

            AssertSelectedHitObject(h =>
            {
                // should not able to generate time-tag if lyric text is empty, or did not have language.
                Assert.IsEmpty(h.TimeTags);
            });
        }

        #endregion

        #region Note

        [Test]
        public void TestAutoGenerateNotes()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                TimeTags = new[]
                {
                    new TimeTag(new TextIndex(0), 0),
                    new TimeTag(new TextIndex(1), 1000),
                    new TimeTag(new TextIndex(2), 2000),
                    new TimeTag(new TextIndex(3), 3000),
                    new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
                }
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.AutoGenerateNotes));

            AssertSelectedHitObject(h =>
            {
                var actualNotes = getMatchedNotes(h);
                Assert.AreEqual(4, actualNotes.Length);

                Assert.AreEqual("カ", actualNotes[0].Text);
                Assert.AreEqual("ラ", actualNotes[1].Text);
                Assert.AreEqual("オ", actualNotes[2].Text);
                Assert.AreEqual("ケ", actualNotes[3].Text);
            });
        }

        [Test]
        public void TestAutoGenerateNotesWithNonSupportedLyric()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
            });

            TriggerHandlerChanged(c => c.AutoGenerate(LyricAutoGenerateProperty.AutoGenerateNotes));

            AssertSelectedHitObject(h =>
            {
                Assert.IsEmpty(getMatchedNotes(h));
            });
        }

        private Note[] getMatchedNotes(Lyric lyric)
            => Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToArray();

        #endregion
    }
}