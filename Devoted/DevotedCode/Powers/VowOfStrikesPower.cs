using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers;

  
public class VowOfStrikesPower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(5M)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        VowOfStrikesPower vow = this;
        if (cardPlay.Card.Owner.Creature != vow.Owner || cardPlay.Card == null)
            return;
        vow.Flash();

        if (cardPlay.Card.Tags.Contains(CardTag.Strike))
        {
            cardPlay.Card.BaseReplayCount += 1;
            CardCmd.Preview(cardPlay.Card);
        }
        else
        {
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, vow.Owner.Player.Creature, vow.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) null);
        }
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        VowOfStrikesPower power = this;
        if (side != power.Owner.Side)
            return;
        power.Flash();
        await PowerCmd.Decrement((PowerModel)power);
    }

}