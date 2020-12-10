﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.RubyRomaji;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric.GenerateRuby
{
    public class GenerateRubySubScreen : ImportLyricSubScreenWithTopNavigation
    {
        public override string Title => "Generate ruby";

        public override string ShortTitle => "Generate ruby";

        public override ImportLyricStep Step => ImportLyricStep.GenerateRuby;

        public override IconUsage Icon => FontAwesome.Solid.Gem;

        [Cached]
        protected readonly RubyRomajiManager RubyRomajiManager;

        public GenerateRubySubScreen()
        {
            AddInternal(RubyRomajiManager = new RubyRomajiManager());
        }

        protected override TopNavigation CreateNavigation()
            => new GenerateRubyNavigation(this);

        protected override Drawable CreateContent()
            => new RubyRomajiEditor
            {
                RelativeSizeAxes = Axes.Both,
            };

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Navigation.State = NavigationState.Initial;
            AskForAutoGenerateRuby();
        }

        public override void Complete()
        {
            ScreenStack.Push(ImportLyricStep.GenerateTimeTag);
        }

        protected void AskForAutoGenerateRuby()
        {
            DialogOverlay.Push(new UseAutoGenerateRubyPopupDialog(ok =>
            {
                if (ok)
                    RubyRomajiManager.AutoGenerateRubyTags();
            }));
        }

        public class GenerateRubyNavigation : TopNavigation
        {
            public GenerateRubyNavigation(ImportLyricSubScreen screen)
                : base(screen)
            {
            }

            protected override void UpdateState(NavigationState value)
            {
                base.UpdateState(value);

                switch (value)
                {
                    case NavigationState.Initial:
                        NavigationText = "Press button to auto-generate ruby and romaji. It's very easy.";
                        break;

                    case NavigationState.Working:
                    case NavigationState.Done:
                        NavigationText = "Go to next step to generate time-tag. Don't worry, it's auto also.";
                        break;

                    case NavigationState.Error:
                        NavigationText = "Oops, seems cause some error in here.";
                        break;
                }
            }

            protected override bool AbleToNextStep(NavigationState value)
                => value == NavigationState.Initial || value == NavigationState.Working || value == NavigationState.Done;
        }
    }
}