// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    public abstract class KaraokeEditorScreenTestScene<T> : EditorClockTestScene where T : KaraokeEditorScreen
    {
        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        protected KaraokeEditorScreenTestScene()
        {
            editorBeatmap = new EditorBeatmap(new KaraokeBeatmap());
        }

        [Test]
        public void TestKaraoke() => runForRuleset(new KaraokeRuleset().RulesetInfo);

        private void runForRuleset(RulesetInfo rulesetInfo)
        {
            AddStep("create screen", () =>
            {
                editorBeatmap.BeatmapInfo.Ruleset = rulesetInfo;

                Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

                Child = CreateEditorScreen().With(x =>
                {
                    x.State.Value = Visibility.Visible;
                });
            });
        }

        protected abstract T CreateEditorScreen();
    }
}
