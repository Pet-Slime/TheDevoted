using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Devoted.DevotedCode.Cards.Uncommon;


[Pool(typeof(DevotedCardPool))]
public class BreathingExercise() : DevotedCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{

    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityTrigger];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}