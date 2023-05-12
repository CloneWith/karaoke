﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;

public partial class RubyTagEditSection : TextTagEditSection<RubyTag>
{
    protected override LocalisableString Title => "Ruby";

    protected override LyricPropertiesEditor CreateLyricPropertiesEditor() => new RubyTagsEditor();

    protected override LockLyricPropertyBy? IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.GetLyricPropertyLockedBy(lyric, nameof(Lyric.RubyTags));

    protected override LocalisableString GetWriteLyricPropertyLockedDescription(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Ruby is sync to another ruby.",
            LockLyricPropertyBy.LockState => "Ruby is locked.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null)
        };

    protected override LocalisableString GetWriteLyricPropertyLockedTooltip(LockLyricPropertyBy lockLyricPropertyBy) =>
        lockLyricPropertyBy switch
        {
            LockLyricPropertyBy.ReferenceLyricConfig => "Cannot edit the ruby because it's sync to another lyric's text.",
            LockLyricPropertyBy.LockState => "The lyric is locked, so cannot edit the ruby.",
            _ => throw new ArgumentOutOfRangeException(nameof(lockLyricPropertyBy), lockLyricPropertyBy, null)
        };

    private partial class RubyTagsEditor : TextTagsEditor
    {
        [Resolved]
        private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; } = null!;

        protected override IBindableList<RubyTag> GetItems(Lyric lyric)
            => lyric.RubyTagsBindable;

        protected override LabelledTextTagTextBox<RubyTag> CreateLabelledTextTagTextBox(Lyric lyric, RubyTag textTag)
            => new LabelledRubyTagTextBox(lyric, textTag);

        protected override void AddTextTag(RubyTag textTag)
            => rubyTagsChangeHandler.Add(textTag);

        protected override LocalisableString CreateNewTextTagButtonText()
            => "Create new ruby";

        protected override LocalisableString CreateNewTextTagTitle()
            => "Ruby";

        protected override LocalisableString CreateNewTextTagDescription()
            => "Please enter the ruby.";
    }

    protected partial class LabelledRubyTagTextBox : LabelledTextTagTextBox<RubyTag>
    {
        [Resolved]
        private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; } = null!;

        [Resolved]
        private IEditRubyModeState editRubyModeState { get; set; } = null!;

        public LabelledRubyTagTextBox(Lyric lyric, RubyTag textTag)
            : base(lyric, textTag)
        {
            Debug.Assert(lyric.RubyTags.Contains(textTag));
        }

        protected override void TriggerSelect(RubyTag item)
            => editRubyModeState.Select(item);

        protected override void ApplyValue(RubyTag item, string value)
            => rubyTagsChangeHandler.SetText(item, value);

        protected override void SetIndex(RubyTag item, int? startIndex, int? endIndex)
            => rubyTagsChangeHandler.SetIndex(item, startIndex, endIndex);

        protected override void RemoveTextTag(RubyTag textTag)
            => rubyTagsChangeHandler.Remove(textTag);

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(editRubyModeState.SelectedItems);
        }
    }
}
