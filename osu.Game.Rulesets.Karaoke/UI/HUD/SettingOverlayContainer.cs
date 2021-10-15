﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class SettingOverlayContainer : CompositeDrawable, IKeyBindingHandler<KaraokeAction>, ISettingHUDOverlay
    {
        private GeneralSettingOverlay generalSettingsOverlay;

        public Action<RightSideOverlay> OnNewOverlayAdded;

        [BackgroundDependencyLoader]
        private void load(HUDOverlay hud, Player player)
        {
            var beatmap = player.Beatmap.Value.Beatmap;
            AddExtraOverlay(generalSettingsOverlay = new GeneralSettingOverlay(beatmap));

            var mods = player.Mods.Value;
            foreach (var mod in mods.OfType<IApplicableToSettingHUDOverlay>())
                mod.ApplyToOverlay(this);
        }

        public void ToggleGameplaySettingsOverlay() => generalSettingsOverlay.ToggleVisibility();

        public virtual bool OnPressed(KeyBindingPressEvent<KaraokeAction> e)
        {
            switch (e.Action)
            {
                // Open adjustment overlay
                case KaraokeAction.OpenPanel:
                    ToggleGameplaySettingsOverlay();
                    return true;

                default:
                    return false;
            }
        }

        public virtual void OnReleased(KeyBindingReleaseEvent<KaraokeAction> e)
        {
        }

        public void AddSettingsGroup(PlayerSettingsGroup group)
        {
            generalSettingsOverlay.Add(group);
        }

        public void AddExtraOverlay(RightSideOverlay overlay)
        {
            AddInternal(overlay);
            OnNewOverlayAdded?.Invoke(overlay);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            // use tricky way to get session from karaoke ruleset.
            var drawableRuleset = dependencies.Get(typeof(DrawableRuleset));
            if (drawableRuleset is DrawableKaraokeRuleset drawableKaraokeRuleset)
                dependencies.CacheAs(drawableKaraokeRuleset.Session);

            return dependencies;
        }
    }

    /// <summary>
    /// Will move into framework layer
    /// </summary>
    public static class BindableNumberExtension
    {
        public static void TriggerIncrease(this BindableInt bindableInt)
        {
            bindableInt.Value += bindableInt.Precision;
        }

        public static void TriggerDecrease(this BindableInt bindableInt)
        {
            bindableInt.Value -= bindableInt.Precision;
        }
    }
}
