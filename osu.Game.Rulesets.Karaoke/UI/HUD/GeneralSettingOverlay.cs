﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.UI.PlayerSettings;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class GeneralSettingOverlay : RightSideOverlay, IKeyBindingHandler<KaraokeAction>
    {
        private readonly BindableInt bindablePitch = new();
        private readonly BindableInt bindableVocalPitch = new();
        private readonly BindableInt bindableSaitenPitch = new();

        public GeneralSettingOverlay(IBeatmap beatmap)
        {
            // Add common group
            Add(new VisualSettings { Expanded = false });
            Add(new PitchSettings { Expanded = false });
            Add(new RubyRomajiSettings { Expanded = false });

            // Add translate group if this beatmap has translate
            if (beatmap.AnyTranslate())
                Add(new TranslateSettings(beatmap.AvailableTranslates()) { Expanded = false });
        }

        public override SettingButton CreateToggleButton() => new()
        {
            Name = "Toggle setting button",
            Text = "Settings",
            TooltipText = "Open/Close setting",
            Action = ToggleVisibility
        };

        // should be able to get the key event.
        protected override bool BlockNonPositionalInput => false;

        // should get key event even it's hide.
        public override bool PropagateNonPositionalInputSubTree => true;

        // on press should return false to prevent handle the back key action.
        public override bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
            => false;

        public virtual bool OnPressed(KeyBindingPressEvent<KaraokeAction> e)
        {
            switch (e.Action)
            {
                // Pitch
                case KaraokeAction.IncreasePitch:
                    bindablePitch.TriggerIncrease();
                    break;

                case KaraokeAction.DecreasePitch:
                    bindablePitch.TriggerDecrease();
                    break;

                case KaraokeAction.ResetPitch:
                    bindablePitch.SetDefault();
                    break;

                // Vocal pitch
                case KaraokeAction.IncreaseVocalPitch:
                    bindableVocalPitch.TriggerIncrease();
                    break;

                case KaraokeAction.DecreaseVocalPitch:
                    bindableVocalPitch.TriggerDecrease();
                    break;

                case KaraokeAction.ResetVocalPitch:
                    bindableVocalPitch.SetDefault();
                    break;

                // Saiten pitch
                case KaraokeAction.IncreaseSaitenPitch:
                    bindableSaitenPitch.TriggerIncrease();
                    break;

                case KaraokeAction.DecreaseSaitenPitch:
                    bindableSaitenPitch.TriggerDecrease();
                    break;

                case KaraokeAction.ResetSaitenPitch:
                    bindableSaitenPitch.SetDefault();
                    break;

                default:
                    return false;
            }

            return true;
        }

        public virtual void OnReleased(KeyBindingReleaseEvent<KaraokeAction> e)
        {
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeSessionStatics session)
        {
            session.BindWith(KaraokeRulesetSession.Pitch, bindablePitch);
            session.BindWith(KaraokeRulesetSession.VocalPitch, bindableVocalPitch);
            session.BindWith(KaraokeRulesetSession.SaitenPitch, bindableSaitenPitch);
        }
    }
}