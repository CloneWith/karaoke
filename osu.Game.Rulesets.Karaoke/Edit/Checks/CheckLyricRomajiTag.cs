﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckLyricRomajiTag : CheckLyricTextTag<RomajiTag>
{
    protected override string Description => "Lyric with invalid romaji tag.";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateLyricRomajiOutOfRange(this),
        new IssueTemplateLyricRomajiOverlapping(this),
        new IssueTemplateLyricRomajiEmptyText(this),
    };

    protected override IList<RomajiTag> GetTextTag(Lyric lyric)
        => lyric.RomajiTags;

    protected override TextTagsUtils.Sorting Sorting => TextTagsUtils.Sorting.Asc;

    protected override Issue GetOutOfRangeIssue(Lyric lyric, RomajiTag textTag)
        => new IssueTemplateLyricRomajiOutOfRange(this).Create(lyric, textTag);

    protected override Issue GetOverlappingIssue(Lyric lyric, RomajiTag textTag)
        => new IssueTemplateLyricRomajiOverlapping(this).Create(lyric, textTag);

    protected override Issue GetEmptyTextIssue(Lyric lyric, RomajiTag textTag)
        => new IssueTemplateLyricRomajiEmptyText(this).Create(lyric, textTag);

    public abstract class IssueTemplateLyricRomaji : IssueTemplateLyricTextTag
    {
        protected IssueTemplateLyricRomaji(ICheck check, IssueType type, string unformattedMessage)
            : base(check, type, unformattedMessage)
        {
        }

        public Issue Create(Lyric lyric, RomajiTag textTag) => new LyricRomajiTagIssue(lyric, this, textTag);
    }

    public class IssueTemplateLyricRomajiOutOfRange : IssueTemplateLyricRomaji
    {
        public IssueTemplateLyricRomajiOutOfRange(ICheck check)
            : base(check, IssueType.Error, "Romaji tag index is out of range.")
        {
        }
    }

    public class IssueTemplateLyricRomajiOverlapping : IssueTemplateLyricRomaji
    {
        public IssueTemplateLyricRomajiOverlapping(ICheck check)
            : base(check, IssueType.Problem, "Romaji tag index is overlapping to another romaji tag.")
        {
        }
    }

    public class IssueTemplateLyricRomajiEmptyText : IssueTemplateLyricRomaji
    {
        public IssueTemplateLyricRomajiEmptyText(ICheck check)
            : base(check, IssueType.Problem, "Romaji tag's text should not be empty or white-space only.")
        {
        }
    }
}
