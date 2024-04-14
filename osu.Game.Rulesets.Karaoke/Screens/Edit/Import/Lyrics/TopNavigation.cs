// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public abstract partial class TopNavigation<T> : TopNavigation where T : LyricImporterStepScreenWithTopNavigation
{
    protected new T Screen => (T)base.Screen;

    protected TopNavigation(T screen)
        : base(screen)
    {
    }
}

public abstract partial class TopNavigation : CompositeDrawable
{
    [Resolved]
    private OsuColour colours { get; set; } = null!;

    [Resolved]
    private EditorBeatmap editorBeatmap { get; set; } = null!;

    protected LyricImporterStepScreen Screen { get; }

    private readonly Box background;
    private readonly NavigationTextContainer text;
    private readonly IconButton button;

    private NavigationState state;

    protected TopNavigation(LyricImporterStepScreen screen)
    {
        Screen = screen;

        RelativeSizeAxes = Axes.Both;
        InternalChildren = new Drawable[]
        {
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
            },
            text = CreateTextContainer().With(t =>
            {
                t.Anchor = Anchor.CentreLeft;
                t.Origin = Anchor.CentreLeft;
                t.RelativeSizeAxes = Axes.X;
                t.AutoSizeAxes = Axes.Y;
                t.Margin = new MarginPadding { Left = 15 };
            }),
            button = new IconButton
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Margin = new MarginPadding { Right = 5 },
                Action = () =>
                {
                    if (AbleToNextStep(state))
                    {
                        CompleteClicked();
                    }
                },
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        // use transaction ended for some reason.
        // 1. seems customized beatmap cannot get hit object updated event(not really sure why).
        // 2. object updated event will trigger hit object updated event lots of time.
        editorBeatmap.TransactionEnded += TriggerStateChange;

        TriggerStateChange();
    }

    protected void TriggerStateChange()
    {
        // wait for a bit until lyric editor's all property loaded.
        ScheduleAfterChildren(() =>
        {
            state = GetState(editorBeatmap.HitObjects.OfType<Lyric>().ToArray());
            updateNavigationDisplayInfo(state);
        });
    }

    private void updateNavigationDisplayInfo(NavigationState value)
    {
        switch (value)
        {
            case NavigationState.Initial:
                background.Colour = colours.Gray2;
                text.Colour = colours.GrayF;
                button.Colour = colours.Gray6;
                button.Icon = FontAwesome.Regular.QuestionCircle;
                break;

            case NavigationState.Working:
                background.Colour = colours.Gray2;
                text.Colour = colours.GrayF;
                button.Colour = colours.Gray6;
                button.Icon = FontAwesome.Solid.InfoCircle;
                break;

            case NavigationState.Done:
                background.Colour = colours.Gray6;
                text.Colour = colours.GrayF;
                button.Colour = colours.Yellow;
                button.Icon = FontAwesome.Regular.ArrowAltCircleRight;
                break;

            case NavigationState.Error:
                background.Colour = colours.Gray2;
                text.Colour = colours.GrayF;
                button.Colour = colours.Yellow;
                button.Icon = FontAwesome.Solid.ExclamationTriangle;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        // Force change style if this step is able to go to next step.
        if (AbleToNextStep(value))
        {
            button.Icon = FontAwesome.Regular.ArrowAltCircleRight;
        }

        text.Text = GetNavigationText(value);
    }

    protected abstract NavigationTextContainer CreateTextContainer();

    protected abstract NavigationState GetState(Lyric[] lyrics);

    protected abstract LocalisableString GetNavigationText(NavigationState value);

    protected virtual bool AbleToNextStep(NavigationState value)
        => value == NavigationState.Done;

    protected virtual void CompleteClicked() => Screen.Complete();

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        editorBeatmap.TransactionEnded -= TriggerStateChange;
    }

    public partial class NavigationTextContainer : CustomizableTextContainer
    {
        protected void AddLinkFactory(string name, string text, Action action)
        {
            AddIconFactory(name, () => new ClickableSpriteText
            {
                Font = new FontUsage(size: 20),
                Text = text,
                Action = action,
            });
        }

        internal partial class ClickableSpriteText : OsuSpriteText
        {
            public Action? Action { get; set; }

            protected override bool OnClick(ClickEvent e)
            {
                Action?.Invoke();
                return base.OnClick(e);
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                Colour = colours.Yellow;
            }
        }
    }

    /// <summary>
    /// Get the dependency from the screen instead of <see cref="ImportLyricHeader"/>
    /// </summary>
    /// <typeparam name="TInject"></typeparam>
    /// <returns></returns>
    protected TInject GetDependency<TInject>() where TInject : class
        => Screen.Dependencies.Get<TInject>();
}

public enum NavigationState
{
    Initial,

    Working,

    Done,

    Error,
}
