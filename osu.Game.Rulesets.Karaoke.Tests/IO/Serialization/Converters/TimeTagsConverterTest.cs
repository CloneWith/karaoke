﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Serialization.Converters
{
    [TestFixture]
    public class TimeTagsConverterTest : BaseSingleConverterTest<TimeTagsConverter>
    {
        [Test]
        public void TestSerialize()
        {
            var rowTimeTag = new[]
            {
                TimeTagsUtils.Create(new TimeTagIndex(0), 1000d),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1100d),
                TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1200d),
            };

            var result = JsonConvert.SerializeObject(rowTimeTag, CreateSettings());

            Assert.AreEqual(result, "[\r\n  \"0,0,1000\",\r\n  \"0,1,1100\",\r\n  \"0,1,1200\"\r\n]");
        }

        [Test]
        public void TestDeserialize()
        {
            const string json_string = "[\r\n  \"0,0,1000\",\r\n  \"0,1,1100\",\r\n  \"0,1,1200\"\r\n]";
            var result = JsonConvert.DeserializeObject<Tuple<TimeTagIndex, double?>[]>(json_string, CreateSettings());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0].Item1.Index, 0);
            Assert.AreEqual(result[0].Item1.State, TimeTagIndex.IndexState.Start);
            Assert.AreEqual(result[0].Item2, 1000);
        }
    }
}