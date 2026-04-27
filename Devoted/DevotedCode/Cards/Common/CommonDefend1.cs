using BaseLib.Utils;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;

public class CommonDefend1() : DevotedCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("FaithGain", 3m), new BlockVar(5, ValueProp.Move)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CommonDefend1 cardSource = this;
        
        await CommonActions.CardBlock(this, cardPlay);
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        FaithPower faithPower = await PowerCmd.Apply<FaithPower>(cardSource.Owner.Creature, cardSource.DynamicVars["FaithGain"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["FaithGain"].UpgradeValueBy(3m);
        DynamicVars["Block"].UpgradeValueBy(3m);
    }
}
