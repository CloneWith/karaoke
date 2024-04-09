// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Export;

public partial class ExportLyricManager : Component
{
    [Resolved]
    private Storage storage { get; set; } = null!;

    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    public void ExportToLrc()
    {
        var exportStorage = storage.GetStorageForDirectory("lrc");
        string filename = $"{beatmap.Name}.lrc";

        using (var outputStream = exportStorage.GetStream(filename, FileAccess.Write, FileMode.Create))
        using (var sw = new StreamWriter(outputStream))
        {
            var encoder = new LrcEncoder();
            sw.WriteLine(encoder.Encode(new Beatmap
            {
                HitObjects = beatmap.HitObjects.ToList(),
            }));
        }

        exportStorage.PresentFileExternally(filename);
    }

    public void ExportToText()
    {
        var exportStorage = storage.GetStorageForDirectory("text");
        string filename = $"{beatmap.Name}.txt";

        using (var outputStream = exportStorage.GetStream(filename, FileAccess.Write, FileMode.Create))
        using (var sw = new StreamWriter(outputStream))
        {
            var encoder = new LyricTextEncoder();
            sw.WriteLine(encoder.Encode(new Beatmap
            {
                HitObjects = beatmap.HitObjects.ToList(),
            }));
        }

        exportStorage.PresentFileExternally(filename);
    }
}
