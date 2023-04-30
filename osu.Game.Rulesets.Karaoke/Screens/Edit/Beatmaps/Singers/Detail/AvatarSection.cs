﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Detail;

internal partial class AvatarSection : EditSingerSection
{
    protected override LocalisableString Title => "Avatar";

    public AvatarSection(Singer singer)
    {
        Children = new Drawable[]
        {
            new LabelledHueSelector
            {
                Label = "Colour",
                Description = "Select singer colour.",
                Current = singer.HueBindable,
            }
        };
    }
}
