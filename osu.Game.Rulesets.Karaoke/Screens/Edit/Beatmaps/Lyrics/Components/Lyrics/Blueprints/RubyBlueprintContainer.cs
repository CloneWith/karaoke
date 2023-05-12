﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Blueprints;

public partial class RubyBlueprintContainer : TextTagBlueprintContainer<RubyTag>
{
    [UsedImplicitly]
    private readonly BindableList<RubyTag> rubyTags;

    public RubyBlueprintContainer(Lyric lyric)
        : base(lyric)
    {
        rubyTags = lyric.RubyTagsBindable.GetBoundCopy();
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        // Add ruby tag into blueprint container
        RegisterBindable(rubyTags);
    }

    protected override SelectionHandler<RubyTag> CreateSelectionHandler()
        => new RubyTagSelectionHandler();

    protected override SelectionBlueprint<RubyTag> CreateBlueprintFor(RubyTag item)
        => new RubyTagSelectionBlueprint(item);

    protected partial class RubyTagSelectionHandler : TextTagSelectionHandler
    {
        [Resolved]
        private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load(IEditRubyModeState editRubyModeState)
        {
            SelectedItems.BindTo(editRubyModeState.SelectedItems);
        }

        protected override void DeleteItems(IEnumerable<RubyTag> items)
            => rubyTagsChangeHandler.RemoveRange(items);

        protected override void SetTextTagShifting(IEnumerable<RubyTag> textTags, int offset)
            => rubyTagsChangeHandler.ShiftingIndex(textTags, offset);

        protected override void SetTextTagIndex(RubyTag textTag, int? startPosition, int? endPosition)
            => rubyTagsChangeHandler.SetIndex(textTag, startPosition, endPosition);
    }
}
