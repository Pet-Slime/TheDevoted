using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;


  
[Pool(typeof(DevotedCardPool))]
public class WaxOn() : DevotedCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FaithPower>(1M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>(), HoverTipFactory.FromKeyword(MyCustomEnums.Waxed)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        WaxOn cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        List<CardModel> list = cardSource.Owner.PlayerCombatState.Hand.Cards.ToList<CardModel>();
        int exhaustedCount = 0;
        foreach (CardModel card in list)
        {
            bool alreadyWaxed = IsValidWaxTarget(card);
            if (alreadyWaxed)
                return;
            
            CardCmd.ApplyKeyword(card, MyCustomEnums.Waxed);
            ++exhaustedCount;
        }
        
        FaithPower faithPower = (await PowerCmd.Apply<FaithPower>(choiceContext, 
            cardSource.Owner.Creature, 
            (Decimal) cardSource.DynamicVars["FaithPower"].IntValue * exhaustedCount, 
            cardSource.Owner.Creature, (CardModel) cardSource))!;
    }
    
    private static bool IsValidWaxTarget(CardModel c)
    {
        return !(c.Keywords.Contains(MyCustomEnums.Waxed)
                 || c.Keywords.Contains(CardKeyword.Exhaust));
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["FaithPower"].UpgradeValueBy(2m);
    }
}