﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    public class KaraokeLegacySkinTransformer : LegacySkinTransformer
    {
        private readonly Lazy<bool> isLegacySkin;
        private readonly KaraokeBeatmapSkin karaokeSkin;

        public KaraokeLegacySkinTransformer(ISkin source, IBeatmap beatmap)
            : base(source)
        {
            // we should get config by default karaoke skin.
            // if has resource or texture, then try to get from legacy skin.
            karaokeSkin = new KaraokeBeatmapSkin(new SkinInfo(), new InternalSkinStorageResourceProvider("Default"));
            isLegacySkin = new Lazy<bool>(() => GetConfig<SkinConfiguration.LegacySetting, decimal>(SkinConfiguration.LegacySetting.Version) != null);
        }

        public override Drawable? GetDrawableComponent(ISkinComponentLookup lookup)
        {
            switch (lookup)
            {
                case SkinComponentsContainerLookup targetComponent:
                    switch (targetComponent.Target)
                    {
                        case SkinComponentsContainerLookup.TargetArea.MainHUDComponents:
                            var components = base.GetDrawableComponent(lookup) as SkinnableTargetComponentsContainer ?? getTargetComponentsContainerFromOtherPlace();
                            components?.Add(new SettingButtonsDisplay
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                            });
                            return components;

                        default:
                            return base.GetDrawableComponent(lookup);
                    }

                case GameplaySkinComponentLookup<HitResult> resultComponent:
                    return getResult(resultComponent.Component);

                case KaraokeSkinComponentLookup karaokeComponent:
                    if (!isLegacySkin.Value)
                        return null;

                    return karaokeComponent.Component switch
                    {
                        KaraokeSkinComponents.ColumnBackground => new LegacyColumnBackground(),
                        KaraokeSkinComponents.StageBackground => new LegacyStageBackground(),
                        KaraokeSkinComponents.JudgementLine => new LegacyJudgementLine(),
                        KaraokeSkinComponents.Note => new LegacyNotePiece(),
                        KaraokeSkinComponents.HitExplosion => new LegacyHitExplosion(),
                        _ => throw new InvalidEnumArgumentException(nameof(karaokeComponent.Component))
                    };

                default:
                    return base.GetDrawableComponent(lookup);
            }

            SkinnableTargetComponentsContainer? getTargetComponentsContainerFromOtherPlace() =>
                Skin switch
                {
                    LegacySkin legacySkin => new TempLegacySkin(legacySkin.SkinInfo.Value).GetDrawableComponent(lookup) as SkinnableTargetComponentsContainer,
                    _ => throw new InvalidCastException()
                };
        }

        private Drawable? getResult(HitResult result)
        {
            // todo : get real component
            return null;
        }

        public override IBindable<TValue>? GetConfig<TLookup, TValue>(TLookup lookup)
            => karaokeSkin.GetConfig<TLookup, TValue>(lookup);

        // it's a temp class for just getting SkinnableTarget.MainHUDComponents
        private class TempLegacySkin : LegacySkin
        {
            public TempLegacySkin(SkinInfo skin)
                : base(skin, null, null)
            {
            }
        }
    }
}
