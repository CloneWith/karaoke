// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;

public interface IStagesChangeHandler : IAutoGenerateChangeHandler<StageInfo>
{
    LocalisableString? GetGeneratorNotSupportedMessage<TStageInfo>() where TStageInfo : StageInfo;

    void Remove<TStageInfo>() where TStageInfo : StageInfo;
}
