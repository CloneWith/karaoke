﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies.Ja;

public class JaRomajiGeneratorConfig : RomajiGeneratorConfig
{
    [ConfigSource("Uppercase", "Export romaji with uppercase.")]
    public Bindable<bool> Uppercase { get; } = new BindableBool();
}
