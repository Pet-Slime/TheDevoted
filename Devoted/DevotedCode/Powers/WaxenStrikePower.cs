using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.VowPowers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Powers;

  
public class WaxenStrikePower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        WaxenStrikePower vow = this;
        CardModel card = cardPlay.Card;

        if (card.Owner.Creature != vow.Owner
            || !card.Tags.Contains(CardTag.Strike)
            || !IsValidWaxTarget(card))
            return;
        
        
        vow.Flash();
        CardModel clone = card.CreateClone();
        clone.EnergyCost.SetThisCombat(0);
        CardCmd.ApplyKeyword(clone, MyCustomEnums.Waxed);
        await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, card.Owner);
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        WaxenStrikePower power = this;
        if (side != power.Owner.Side)
            return;
        power.Flash();
        await PowerCmd.Decrement((PowerModel)power);
    }
    
    private static bool IsValidWaxTarget(CardModel c)
    {
        return !(c.Keywords.Contains(MyCustomEnums.Waxed)
                 || c.Keywords.Contains(CardKeyword.Exhaust));
    }

}