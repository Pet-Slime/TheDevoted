using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Common;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;

public class UncommonDefend1() : DevotedCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DevotionGain", 1m), new BlockVar(5, ValueProp.Move)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        UncommonDefend1 cardSource = this;
        
        await CommonActions.CardBlock(this, cardPlay);
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        DevotionPower devotionPower = await PowerCmd.Apply<DevotionPower>(cardSource.Owner.Creature, cardSource.DynamicVars["DevotionGain"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["DevotionGain"].UpgradeValueBy(1m);
        DynamicVars["Block"].UpgradeValueBy(3m);
    }
}
