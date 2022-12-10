// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.ReferenceLyric;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Reference
{
    public partial class ReferenceLyricAutoGenerateSection : LyricEditorSection
    {
        protected override LocalisableString Title => "Auto generate";

        public ReferenceLyricAutoGenerateSection()
        {
            Children = new[]
            {
                new ReferenceLyricAutoGenerateSubsection()
            };
        }

        private partial class ReferenceLyricAutoGenerateSubsection : AutoGenerateSubsection
        {
            public ReferenceLyricAutoGenerateSubsection()
                : base(LyricAutoGenerateProperty.DetectReferenceLyric)
            {
            }

            protected override DescriptionFormat CreateInvalidLyricDescriptionFormat()
                => new()
                {
                    Text = "Seems every lyrics in the songs are unique. But don't worry, reference lyric can still link by hands."
                };

            protected override ConfigButton CreateConfigButton()
                => new ReferenceLyricAutoGenerateConfigButton();

            protected partial class ReferenceLyricAutoGenerateConfigButton : ConfigButton
            {
                public override Popover GetPopover()
                    => new ReferenceLyricDetectorConfigPopover();
            }
        }
    }
}
