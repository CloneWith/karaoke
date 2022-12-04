// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps
{
    public abstract class KaraokeEditorScreen : GenericEditorScreen<KaraokeEditorScreenMode>
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        protected KaraokeEditorScreen(KaraokeEditorScreenMode type)
            : base(type)
        {
        }

        protected override void PopIn()
        {
            base.PopIn();

            // should wait until current change handler done.
            // not a good way but ok for now.
            ScheduleAfterChildren(() =>
            {
                // for prevent accidentally change the property in the hit object that is not expected,
                // should clear all selected hit object if user change to the different tab.
                beatmap.SelectedHitObjects.Clear();
            });
        }
    }
}
