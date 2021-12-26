﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Skinning.Mappings
{
    public class StyleMappingBySingerIds : IStyleMappingRole
    {
        public int[] SingerIds { get; set; }

        [JsonProperty(IsReference = true)]
        public LyricStyle LyricStyle { get; set; }

        [JsonProperty(IsReference = true)]
        public LyricStyle NoteStyle { get; set; }
    }
}