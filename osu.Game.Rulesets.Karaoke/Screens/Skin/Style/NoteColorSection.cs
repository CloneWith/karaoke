﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style;

internal partial class NoteColorSection : StyleSection
{
    private LabelledColourSelector noteColorPicker;
    private LabelledColourSelector blinkColorPicker;

    protected override LocalisableString Title => "Color";

    [BackgroundDependencyLoader]
    private void load(SkinManager manager)
    {
        Children = new Drawable[]
        {
            noteColorPicker = new LabelledColourSelector
            {
                Label = "Note color",
                Description = "Select color.",
            },
            blinkColorPicker = new LabelledColourSelector
            {
                Label = "Blink color",
                Description = "Select color.",
            }
        };
    }
}
