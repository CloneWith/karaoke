// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricTextChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricTextChangeHandler>
{
    [Test]
    public void TestInsertText()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラ",
        });

        TriggerHandlerChanged(c => c.InsertText(2, "オケ"));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Text, Is.EqualTo("カラオケ"));
        });
    }

    [Test]
    public void TestDeleteLyricText()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.DeleteLyricText(4));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Text, Is.EqualTo("カラオ"));
        });
    }

    [Test]
    public void TestDeleteAllLyricText()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カ",
        });

        TriggerHandlerChanged(c => c.DeleteLyricText(1));

        AssertHitObjects(x => Assert.That(x, Is.Empty));
    }

    [Test]
    public void TestWithReferenceLyric()
    {
        PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "カラ",
        });

        TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.InsertText(2, "オケ"));
    }
}
