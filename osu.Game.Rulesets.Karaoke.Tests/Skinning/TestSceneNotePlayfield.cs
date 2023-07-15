﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.UI;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning;

public partial class TestSceneNotePlayfield : KaraokeSkinnableColumnTestScene
{
    [BackgroundDependencyLoader]
    private void load()
    {
        SetContents(_ => new KaraokeInputManager(new KaraokeRuleset().RulesetInfo)
        {
            Child = new NotePlayfield(COLUMNS),
        });
    }
}
