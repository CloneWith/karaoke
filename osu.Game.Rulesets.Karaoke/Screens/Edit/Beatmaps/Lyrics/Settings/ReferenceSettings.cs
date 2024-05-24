// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Reference;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class ReferenceSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Right;

    public override float SettingsWidth => 300;

    private readonly IBindable<ReferenceLyricEditStep> bindableEditStep = new Bindable<ReferenceLyricEditStep>();

    [BackgroundDependencyLoader]
    private void load(IEditReferenceLyricModeState editReferenceLyricModeState)
    {
        bindableEditStep.BindTo(editReferenceLyricModeState.BindableEditStep);
        bindableEditStep.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableEditStep.Value switch
    {
        ReferenceLyricEditStep.Edit => new Drawable[]
        {
            new ReferenceLyricSettingsHeader(),
            new ReferenceLyricAutoGenerateSection(),
            new ReferenceLyricSection(),
            new ReferenceLyricConfigSection(),
        },
        ReferenceLyricEditStep.Verify => new Drawable[]
        {
            new ReferenceLyricSettingsHeader(),
            new ReferenceLyricIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
