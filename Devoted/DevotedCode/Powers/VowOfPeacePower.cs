using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers;


public class VowOfPeacePower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => false;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(5M, ValueProp.Move), new PowerVar<ZealPower>(1M)];

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block),HoverTipFactory.FromPower<ZealPower>()]; 

    
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        VowOfPeacePower vow = this;
        if (cardPlay.Card.Owner.Creature != vow.Owner || cardPlay.Card == null)
            return;
        vow.Flash();

        if (cardPlay.Card.Type == CardType.Skill)
        {
            Decimal num = await CreatureCmd.GainBlock(vow.Owner.Player.Creature, vow.DynamicVars.Block, cardPlay);
        }
        else
        {
            ZealPower strengthPower = await PowerCmd.Apply<ZealPower>(choiceContext, vow.Owner,
                (Decimal)vow.DynamicVars["ZealPower"].IntValue * -1, vow.Owner, (CardModel)null);
        }
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        VowOfPeacePower power = this;
        if (side != power.Owner.Side)
            return;
        power.Flash();
        await PowerCmd.Decrement((PowerModel)power);
    }
}