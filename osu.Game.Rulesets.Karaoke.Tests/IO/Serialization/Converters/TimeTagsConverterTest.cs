// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters;

[TestFixture]
public class TimeTagsConverterTest : BaseSingleConverterTest<TimeTagsConverter>
{
    protected override IEnumerable<JsonConverter> CreateExtraConverts()
    {
        yield return new TimeTagConverter();
    }

    [Test]
    public void TestSerialize()
    {
        var timeTags = new[]
        {
            new TimeTag(new TextIndex(0, TextIndex.IndexState.End), 1000),
            new TimeTag(new TextIndex(0), 0),
        };

        const string expected = "[\"[0,start]:0\",\"[0,end]:1000\"]";
        string actual = JsonConvert.SerializeObject(timeTags, CreateSettings());
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestDeserialize()
    {
        const string json = "[\"[0,end]:1000\",\"[0,start]:0\"]";

        var expected = new[]
        {
            new TimeTag(new TextIndex(0), 0),
            new TimeTag(new TextIndex(0, TextIndex.IndexState.End), 1000),
        };
        var actual = JsonConvert.DeserializeObject<TimeTag[]>(json, CreateSettings()) ?? throw new InvalidCastException();
        TimeTagAssert.ArePropertyEqual(expected, actual);
    }
}
