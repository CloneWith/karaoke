// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics;

public partial class TestSceneRightTriangle : OsuTestScene
{
    private RightTriangle rightTriangle = null!;

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        Child = new Container
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            AutoSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colours.Gray4,
                },
                rightTriangle = new RightTriangle
                {
                    Size = new Vector2(100),
                    Colour = colours.Yellow,
                },
            },
        };
    }

    [Test]
    public void TestRightAngleDirections()
    {
        foreach (var direction in Enum.GetValues<TriangleRightAngleDirection>())
        {
            AddStep($"Test direction {direction}", () =>
            {
                rightTriangle.RightAngleDirection = direction;
            });
        }
    }
}
