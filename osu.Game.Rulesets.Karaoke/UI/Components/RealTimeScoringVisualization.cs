﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.UI.Position;

namespace osu.Game.Rulesets.Karaoke.UI.Components;

public partial class RealTimeScoringVisualization : VoiceVisualization<KeyValuePair<double, KaraokeScoringAction>>
{
    private readonly Cached addStateCache = new();

    protected override float PathRadius => 2.5f;

    protected override float Offset => DrawSize.X;

    [Resolved]
    private INotePositionInfo notePositionInfo { get; set; } = null!;

    public RealTimeScoringVisualization()
    {
        Masking = true;
    }

    protected override double GetTime(KeyValuePair<double, KaraokeScoringAction> frame) => frame.Key;

    protected override float GetPosition(KeyValuePair<double, KaraokeScoringAction> frame) => notePositionInfo.Calculator.YPositionAt(frame.Value);

    private bool createNew = true;

    private double minAvailableTime;

    public void AddAction(KaraokeScoringAction action)
    {
        if (Time.Current <= minAvailableTime)
            return;

        minAvailableTime = Time.Current;

        if (createNew)
        {
            createNew = false;

            CreateNew(new KeyValuePair<double, KaraokeScoringAction>(Time.Current, action));
        }
        else
        {
            Append(new KeyValuePair<double, KaraokeScoringAction>(Time.Current, action));
        }

        // Trigger update last frame
        addStateCache.Invalidate();
    }

    public void Release()
    {
        if (Time.Current < minAvailableTime)
            return;

        minAvailableTime = Time.Current;

        createNew = true;
    }

    protected override void Update()
    {
        // If addStateCache is invalid, means last path should be re-calculate
        if (!addStateCache.IsValid && Paths.Any())
        {
            var updatePath = Paths.Last();
            MarkAsInvalid(updatePath);
            addStateCache.Validate();
        }

        base.Update();
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours)
    {
        Colour = colours.Yellow;
    }
}
