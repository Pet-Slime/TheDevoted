using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using Devoted.DevotedCode.Powers.SerenityPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Rare;



  
[Pool(typeof(DevotedCardPool))]
public class ZealousFury() : DevotedCard(0, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("ZealFuryRepeat", 1m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceStrengthPower>(), HoverTipFactory.FromPower<ZealPower>()];
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        ZealousFury cardSource = this;
        
        int num1 = cardSource.ResolveEnergyXValue();
        var amount = num1 + cardSource.DynamicVars["ZealFuryRepeat"].BaseValue;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        PenanceStrengthPower strengthPower = await PowerCmd.Apply<PenanceStrengthPower>(choiceContext, cardSource.Owner.Creature, amount, cardSource.Owner.Creature, (CardModel) cardSource);
        ZealPower zealPower = await PowerCmd.Apply<ZealPower>(choiceContext, cardSource.Owner.Creature, amount*-1, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["ZealFuryRepeat"].UpgradeValueBy(1m);
    }
}