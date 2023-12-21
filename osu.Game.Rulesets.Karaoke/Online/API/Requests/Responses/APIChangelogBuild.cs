﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

public class APIChangelogBuild
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="organization">Account or organization name</param>
    /// <param name="project">Project name</param>
    /// <param name="branch">Branch name</param>
    public APIChangelogBuild(string organization, string project, string branch = "master")
    {
        OrganizationName = organization;
        ProjectName = project;
        Branch = branch;
        Versions = new VersionNavigation();
    }

    /// <summary>
    /// Organization name
    /// </summary>
    public string OrganizationName { get; }

    /// <summary>
    /// Project name
    /// </summary>
    public string ProjectName { get; }

    /// <summary>
    /// Branch name
    /// </summary>
    public string Branch { get; }

    /// <summary>
    /// The URL of the loaded document.
    /// </summary>
    public string DocumentUrl => $"https://raw.githubusercontent.com/{OrganizationName}/{ProjectName}/{Branch}/{Path}/";

    /// <summary>
    /// The base URL for all root-relative links.
    /// </summary>
    public string RootUrl { get; set; } = null!;

    /// <summary>
    /// Path of the project
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// Path to download readme url
    /// </summary>
    public string ReadmeDownloadUrl => $"{DocumentUrl}index.md";

    /// <summary>
    /// Version number
    /// </summary>
    /// <example>2023.0123</example>
    /// <example>2023.1111</example>
    public string Version { get; set; } = null!;

    /// <summary>
    /// Display version
    /// </summary>
    public string DisplayVersion => Version;

    /// <summary>
    /// Might be preview or detail markdown content.
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// Version
    /// </summary>
    public VersionNavigation Versions { get; }

    /// <summary>
    /// Created date.
    /// </summary>
    public DateTimeOffset PublishedAt { get; set; }

    public class VersionNavigation
    {
        /// <summary>
        /// Next version
        /// </summary>
        public APIChangelogBuild? Next { get; set; }

        /// <summary>
        /// Previous version
        /// </summary>
        public APIChangelogBuild? Previous { get; set; }
    }

    public override string ToString() => $"Karaoke! {DisplayVersion}";
}
