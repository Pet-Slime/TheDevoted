using Devoted.DevotedCode.Keywords;
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

namespace Devoted.DevotedCode.Powers.VowPowers;

  
public class VowOfAshPower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4M, ValueProp.Unpowered | ValueProp.Move)];
    
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        VowOfAshPower vow = this;
        if (cardPlay.Card.Owner.Creature != vow.Owner || cardPlay.Card == null || cardPlay.Card.Tags.Contains(MyCustomEnums.VowOfAsh))
            return;
        vow.Flash();

        if (cardPlay.Card.Type == CardType.Attack)
        {
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(),
                vow.Owner, 
                vow.DynamicVars.Damage.BaseValue, 
                ValueProp.Unpowered, 
                (Creature) null, 
                (CardModel) null);
        }
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        VowOfAshPower power = this;
        if (side != power.Owner.Side)
            return;
        power.Flash();
        await PowerCmd.Decrement((PowerModel)power);
    }

}