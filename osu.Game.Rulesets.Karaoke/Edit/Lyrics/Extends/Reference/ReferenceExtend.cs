// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Reference
{
    public class ReferenceExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        protected override IReadOnlyList<Drawable> CreateSections() => new Drawable[]
        {
            new ReferenceLyricAutoGenerateSection(),
            new ReferenceLyricSection(),
            new ReferenceLyricConfigSection()
        };
    }
}
