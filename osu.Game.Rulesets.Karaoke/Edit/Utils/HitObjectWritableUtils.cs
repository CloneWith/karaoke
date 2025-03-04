﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Utils;

public static class HitObjectWritableUtils
{
    #region Remove lyrics.

    public static bool IsRemoveLyricLocked(Lyric lyric)
        => GetRemoveLyricLockedBy(lyric) != null;

    public static LockLyricPropertyBy? GetRemoveLyricLockedBy(Lyric lyric)
    {
        bool lockedByState = isRemoveLyricLockedByState(lyric.Lock);
        if (lockedByState)
            return LockLyricPropertyBy.LockState;

        return null;
    }

    private static bool isRemoveLyricLockedByState(LockState lockState)
        => lockState != LockState.None;

    #endregion

    #region Lyric property

    public static bool IsWriteLyricPropertyLocked(Lyric lyric, params string[] propertyNames)
        => GetLyricPropertyLockedBy(lyric, propertyNames) != null;

    public static bool IsWriteLyricPropertyLocked(Lyric lyric, string propertyName)
        => GetLyricPropertyLockedBy(lyric, propertyName) != null;

    public static LockLyricPropertyBy? GetLyricPropertyLockedBy(Lyric lyric, params string[] propertyNames)
    {
        var reasons = propertyNames.Select(x => GetLyricPropertyLockedBy(lyric, x))
                                   .Where(x => x != null)
                                   .OfType<LockLyricPropertyBy>()
                                   .ToArray();

        if (reasons.Contains(LockLyricPropertyBy.ReferenceLyricConfig))
            return LockLyricPropertyBy.ReferenceLyricConfig;

        if (reasons.Contains(LockLyricPropertyBy.LockState))
            return LockLyricPropertyBy.LockState;

        return null;
    }

    public static LockLyricPropertyBy? GetLyricPropertyLockedBy(Lyric lyric, string propertyName)
    {
        bool lockedByConfig = isWriteLyricPropertyLockedByConfig(lyric.ReferenceLyricConfig, propertyName);
        if (lockedByConfig)
            return LockLyricPropertyBy.ReferenceLyricConfig;

        bool lockedByState = isWriteLyricPropertyLockedByState(lyric.Lock, propertyName);
        if (lockedByState)
            return LockLyricPropertyBy.LockState;

        return null;
    }

    private static bool isWriteLyricPropertyLockedByState(LockState lockState, string propertyName)
    {
        // partial lock will only lock some property change like text because they are easy to be modified.
        // fully lock will basically lock all lyric properties.
        return propertyName switch
        {
            nameof(Lyric.ID) => false, // although the id is not changeable, but it's not locked by config.
            nameof(Lyric.Text) => lockState > LockState.None,
            nameof(Lyric.TimeTags) => lockState > LockState.None,
            nameof(Lyric.RubyTags) => lockState > LockState.None,
            nameof(Lyric.StartTime) => lockState > LockState.Partial,
            nameof(Lyric.Duration) => lockState > LockState.Partial,
            nameof(Lyric.TimeValid) => lockState > LockState.Partial,
            nameof(Lyric.SingerIds) => lockState > LockState.Partial,
            nameof(Lyric.Translations) => lockState > LockState.Partial,
            nameof(Lyric.Language) => lockState > LockState.Partial,
            nameof(Lyric.Order) => false, // order can always be changed.
            nameof(Lyric.Lock) => false, // order can always be changed.
            nameof(Lyric.ReferenceLyric) or nameof(Lyric.ReferenceLyricId) => lockState > LockState.Partial,
            nameof(Lyric.ReferenceLyricConfig) => lockState > LockState.Partial,
            // base class
            nameof(Lyric.Samples) => false,
            _ => throw new NotSupportedException(),
        };
    }

    private static bool isWriteLyricPropertyLockedByConfig(IReferenceLyricPropertyConfig? config, string propertyName)
    {
        return config switch
        {
            ReferenceLyricConfig => false,
            SyncLyricConfig syncLyricConfig => propertyName switch
            {
                nameof(Lyric.ID) => false, // although the id is not changeable, but it's not locked by config.
                nameof(Lyric.Text) => true,
                nameof(Lyric.TimeTags) => syncLyricConfig.SyncTimeTagProperty,
                nameof(Lyric.RubyTags) => true,
                nameof(Lyric.StartTime) => false,
                nameof(Lyric.Duration) => false,
                nameof(Lyric.TimeValid) => false,
                nameof(Lyric.SingerIds) => syncLyricConfig.SyncSingerProperty,
                nameof(Lyric.Translations) => true,
                nameof(Lyric.Language) => true,
                nameof(Lyric.Order) => true,
                nameof(Lyric.Lock) => true,
                nameof(Lyric.ReferenceLyric) or nameof(Lyric.ReferenceLyricId) => false,
                nameof(Lyric.ReferenceLyricConfig) => false,
                // base class
                nameof(Lyric.Samples) => false,
                _ => throw new NotSupportedException(),
            },
            null => false,
            _ => throw new NotSupportedException(),
        };
    }

    #endregion

    #region Create or remove notes.

    public static bool IsCreateOrRemoveNoteLocked(Lyric lyric)
        => GetCreateOrRemoveNoteLockedBy(lyric) != null;

    public static LockLyricPropertyBy? GetCreateOrRemoveNoteLockedBy(Lyric lyric)
    {
        bool lockedByConfig = isCreateOrRemoveNoteLocked(lyric.ReferenceLyricConfig);
        if (lockedByConfig)
            return LockLyricPropertyBy.ReferenceLyricConfig;

        return null;
    }

    private static bool isCreateOrRemoveNoteLocked(IReferenceLyricPropertyConfig? config)
    {
        // todo: implementation.
        return config switch
        {
            ReferenceLyricConfig => false,
            SyncLyricConfig => true,
            null => false,
            _ => throw new NotSupportedException(),
        };
    }

    #endregion

    #region Note property

    public static bool IsWriteNotePropertyLocked(Note note, params string[] propertyNames)
        => GetNotePropertyLockedBy(note, propertyNames) != null;

    public static bool IsWriteNotePropertyLocked(Note note, string propertyName)
        => GetNotePropertyLockedBy(note, propertyName) != null;

    public static LockNotePropertyBy? GetNotePropertyLockedBy(Note note, params string[] propertyNames)
    {
        var reasons = propertyNames.Select(x => GetNotePropertyLockedBy(note, x))
                                   .Where(x => x != null)
                                   .OfType<LockNotePropertyBy>()
                                   .ToArray();

        if (reasons.Contains(LockNotePropertyBy.ReferenceLyricConfig))
            return LockNotePropertyBy.ReferenceLyricConfig;

        return null;
    }

    public static LockNotePropertyBy? GetNotePropertyLockedBy(Note note, string propertyName)
    {
        var lyric = note.ReferenceLyric;

        bool lockByReferenceLyricConfig = lyric != null && isWriteNotePropertyLockedByReferenceLyric(lyric, propertyName);
        if (lockByReferenceLyricConfig)
            return LockNotePropertyBy.ReferenceLyricConfig;

        return null;
    }

    private static bool isWriteNotePropertyLockedByReferenceLyric(Lyric lyric, string propertyName)
    {
        // todo: implement.
        return false;
    }

    #endregion
}

public enum LockLyricPropertyBy
{
    ReferenceLyricConfig,

    LockState,
}

public enum LockNotePropertyBy
{
    ReferenceLyricConfig,
}
