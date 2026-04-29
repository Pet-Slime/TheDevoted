using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.PenancePowers;



public class CrusadePower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("ExtraDamage", 1m)];

    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {

        CrusadePower power = this;
        
        if (dealer == null || dealer != power.Owner)
            return 0M;
        
        bool notPowered = !props.IsPoweredAttack();
        bool noCard = cardSource == null;
        bool notStrike = cardSource != null && !cardSource.Tags.Contains<CardTag>(CardTag.Strike);

        if (notPowered || noCard || notStrike)
            return 0M;

        return this.DynamicVars["ExtraDamage"].BaseValue * power.Amount;
    }

}