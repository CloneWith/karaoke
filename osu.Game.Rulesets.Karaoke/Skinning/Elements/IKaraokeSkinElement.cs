// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Elements;

public interface IKaraokeSkinElement
{
    int ID { get; set; }

    string Name { get; set; }

    void ApplyTo(Drawable d);
}
