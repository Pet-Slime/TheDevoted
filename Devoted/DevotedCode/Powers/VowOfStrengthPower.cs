using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers;


public class VowOfStrengthPower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1M)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()]; 
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        VowOfStrengthPower vow = this;
        if (cardPlay.Card.Owner.Creature != vow.Owner || cardPlay.Card == null)
            return;
        vow.Flash();

        if (cardPlay.Card.Type == CardType.Attack)
        {
            StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, vow.Owner, (Decimal) vow.DynamicVars["StrengthPower"].IntValue, vow.Owner, (CardModel) null);
        }
        else
        {
            StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, vow.Owner, (Decimal) vow.DynamicVars["StrengthPower"].IntValue * -1, vow.Owner, (CardModel) null);
        }
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        VowOfStrengthPower power = this;
        if (side != power.Owner.Side)
            return;
        power.Flash();
        await PowerCmd.Decrement((PowerModel)power);
    }

}