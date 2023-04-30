﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public class CultureInfoConverter : JsonConverter<CultureInfo>
{
    public override CultureInfo? ReadJson(JsonReader reader, Type objectType, CultureInfo? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var obj = JToken.Load(reader);
        int? value = obj.Value<int?>();

        if (value == null)
            return null;

        return CultureInfoUtils.CreateLoadCultureInfoById(value.Value);
    }

    public override void WriteJson(JsonWriter writer, CultureInfo? value, JsonSerializer serializer)
    {
        int? id = value != null ? CultureInfoUtils.GetSaveCultureInfoId(value) : null;

        writer.WriteValue(id);
    }
}
