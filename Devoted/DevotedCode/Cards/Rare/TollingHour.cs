using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.ChimePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Rare;


[Pool(typeof(DevotedCardPool))]
public class TollingHour() : DevotedCard(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
{

    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.TollTriggers];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ MyCustomEnums.Toll ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("TollTriggers", 2m)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["TollTriggers"].UpgradeValueBy(1m);
    }
}