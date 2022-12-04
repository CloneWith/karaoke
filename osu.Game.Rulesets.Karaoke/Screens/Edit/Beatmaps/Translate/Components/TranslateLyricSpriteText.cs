﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translate.Components
{
    public class TranslateLyricSpriteText : DrawableLyricSpriteText, IHasCustomTooltip<Lyric>
    {
        public TranslateLyricSpriteText(Lyric hitObject)
            : base(hitObject)
        {
            TooltipContent = hitObject;
        }

        public ITooltip<Lyric> GetCustomTooltip() => new LyricTooltip();

        public Lyric TooltipContent { get; }
    }
}
