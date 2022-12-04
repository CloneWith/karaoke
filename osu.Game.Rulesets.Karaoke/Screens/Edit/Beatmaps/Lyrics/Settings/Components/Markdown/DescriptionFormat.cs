// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown
{
    public struct DescriptionFormat
    {
        public const string LINK_KEY_INPUT = "input_key";
        public const string LINK_KEY_EDIT_MODE = "edit_mode";

        public LocalisableString Text { get; set; }

        public IDictionary<string, InputKey> Keys { get; set; }

        public IDictionary<string, SwitchMode> EditModes { get; set; }

        // todo: will be removed eventually.
        public static implicit operator DescriptionFormat(string text)
            => (LocalisableString)text;

        public static implicit operator DescriptionFormat(LocalisableString text) => new()
        {
            Text = text
        };
    }

    public struct SwitchMode
    {
        public LocalisableString Text { get; set; }

        public LyricEditorMode Mode { get; set; }
    }

    public struct InputKey
    {
        public LocalisableString Text { get; set; }

        public IList<KaraokeEditAction> AdjustableActions { get; set; }
    }
}
