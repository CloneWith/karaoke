﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.Containers;

/// <summary>
/// Implement most feature for searchable text container.
/// </summary>
/// <typeparam name="TModel"></typeparam>
public partial class RearrangeableTextFlowListContainer<TModel> : OsuRearrangeableListContainer<TModel>
{
    public readonly Bindable<TModel> SelectedSet = new();

    public Action<TModel>? RequestSelection;

    private SearchContainer<RearrangeableListItem<TModel>> searchContainer = null!;

    protected sealed override FillFlowContainer<RearrangeableListItem<TModel>> CreateListFillFlowContainer() => searchContainer = new SearchContainer<RearrangeableListItem<TModel>>
    {
        Spacing = new Vector2(0, 3),
        LayoutDuration = 200,
        LayoutEasing = Easing.OutQuint,
    };

    public void Filter(string text)
    {
        searchContainer.SearchTerm = text;
    }

    protected sealed override OsuRearrangeableListItem<TModel> CreateOsuDrawable(TModel item)
        => CreateDrawable(item).With(d =>
        {
            d.SelectedSet.BindTarget = SelectedSet;
            d.RequestSelection = set => RequestSelection?.Invoke(set);
        });

    protected new virtual DrawableTextListItem CreateDrawable(TModel item)
        => new(item);

    public partial class DrawableTextListItem : OsuRearrangeableListItem<TModel>, IFilterable
    {
        public readonly Bindable<TModel> SelectedSet = new();

        public Action<TModel>? RequestSelection;

        private TextFlowContainer text = null!;

        private Color4 selectedColour;

        public DrawableTextListItem(TModel item)
            : base(item)
        {
            Padding = new MarginPadding { Left = 5 };
            ShowDragHandle.Value = false;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            selectedColour = colours.Yellow;
            HandleColour = colours.Gray5;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            SelectedSet.BindValueChanged(set =>
            {
                bool oldValueMatched = EqualityComparer<TModel>.Default.Equals(set.OldValue, Model);
                bool newValueMatched = EqualityComparer<TModel>.Default.Equals(set.NewValue, Model);
                if (!oldValueMatched && !newValueMatched)
                    return;

                text.FadeColour(newValueMatched ? selectedColour : Color4.White, FADE_DURATION);
            }, true);
        }

        protected sealed override Drawable CreateContent() => text = new OsuTextFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
        }.With(x =>
        {
            Schedule(() =>
            {
                // should create the text after BDL loaded.
                CreateDisplayContent(x, Model);
            });
        });

        protected override bool OnClick(ClickEvent e)
        {
            RequestSelection?.Invoke(Model);
            return true;
        }

        public virtual IEnumerable<LocalisableString> FilterTerms => new[]
        {
            new LocalisableString(Model?.ToString() ?? string.Empty),
        };

        protected virtual void CreateDisplayContent(OsuTextFlowContainer textFlowContainer, TModel model)
            => textFlowContainer.AddText(model?.ToString() ?? string.Empty);

        private bool matchingFilter = true;

        public bool MatchingFilter
        {
            get => matchingFilter;
            set
            {
                if (matchingFilter == value)
                    return;

                matchingFilter = value;
                updateFilter();
            }
        }

        private void updateFilter() => this.FadeTo(MatchingFilter ? 1 : 0, 200);

        public bool FilteringActive { get; set; }
    }
}
