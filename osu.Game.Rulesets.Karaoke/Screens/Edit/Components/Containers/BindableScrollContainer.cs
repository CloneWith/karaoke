// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Containers
{
    public abstract partial class BindableScrollContainer : ZoomableScrollContainer
    {
        protected readonly BindableFloat BindableZoom = new();
        protected readonly BindableFloat BindableCurrent = new();

        protected BindableScrollContainer()
        {
            ZoomDuration = 200;
            ZoomEasing = Easing.OutQuint;
            ScrollbarVisible = false;

            BindableZoom.MaxValueChanged += assignZoomRange;
            BindableZoom.MinValueChanged += assignZoomRange;

            void assignZoomRange(float _)
            {
                // we should make sure that will not cause error while assigning the size.
                float initial = Math.Clamp(BindableZoom.Value, BindableZoom.MinValue, BindableZoom.MaxValue);
                float minimum = BindableZoom.MinValue;
                float maximum = BindableZoom.MaxValue;
                SetupZoom(initial, minimum, maximum);
            }

            BindableZoom.BindValueChanged(e =>
            {
                if (e.NewValue == Zoom)
                    return;

                Zoom = e.NewValue;
            }, true);

            BindableCurrent.BindValueChanged(e =>
            {
                ScrollTo(e.NewValue);
            }, true);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            bool zoneChanged = base.OnScroll(e);
            if (!zoneChanged)
                return false;

            if (e.AltPressed)
            {
                // todo : this event not working while zooming, because zooming will also call scroll to.
                // bindableCurrent.Value = getCurrentPosition();

                // Update zoom to target, ignore easing value.
                BindableZoom.Value = Zoom;
            }

            return true;

            /*
            float getCurrentPosition()
            {
                // params
                var zoomedContent = Content;
                var focusPoint = zoomedContent.ToLocalSpace(e.ScreenSpaceMousePosition).X;
                var contentSize = zoomedContent.DrawWidth;
                var scrollOffset = Current;

                // calculation
                float focusOffset = focusPoint - scrollOffset;
                float expectedWidth = DrawWidth * Zoom;
                float targetOffset = expectedWidth * (focusPoint / contentSize) - focusOffset;

                return targetOffset;
            }
            */
        }

        protected override void OnUserScroll(float value, bool animated = true, double? distanceDecay = null)
        {
            base.OnUserScroll(value, animated, distanceDecay);

            // update current value if user scroll to.
            BindableCurrent.Value = value;
        }
    }
}
