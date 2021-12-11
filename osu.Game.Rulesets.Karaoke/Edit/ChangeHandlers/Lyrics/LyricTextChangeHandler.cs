// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricTextChangeHandler : HitObjectChangeHandler<Lyric>, ILyricTextChangeHandler
    {
        public void InsertText(int index, string text)
        {
            // todo: implement. also should consider IME issue.
        }

        public void DeleteLyricText(int index)
        {
            PerformOnSelection(lyric =>
            {
                LyricUtils.RemoveText(lyric, index - 1);

                if (!string.IsNullOrEmpty(lyric.Text))
                    return;

                OrderUtils.ShiftingOrder(HitObjects.Where(x => x.Order > lyric.Order).ToArray(), -1);
                Remove(lyric);
            });
        }
    }
}