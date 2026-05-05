using Devoted.DevotedCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Powers;

  
public class ChainLightingPower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MyCustomEnums.Waxed)];
    
    public override bool TryModifyEnergyCostInCombatLate(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != this.Owner || !card.Keywords.Contains(MyCustomEnums.Waxed))
            return false;
        PileType? type = card.Pile?.Type;
        bool flag;
        if (type.HasValue)
        {
            switch (type.GetValueOrDefault())
            {
                case PileType.Hand:
                case PileType.Play:
                    flag = true;
                    goto label_6;
            }
        }
        flag = false;
        label_6:
        if (!flag)
            return false;
        modifiedCost = 0M;
        return true;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        ChainLightingPower power = this;
        if (cardPlay.Card.Owner.Creature != power.Owner || !cardPlay.Card.Keywords.Contains(MyCustomEnums.Waxed))
            return;
        PileType? type = cardPlay.Card.Pile?.Type;
        bool flag;
        if (type.HasValue)
        {
            switch (type.GetValueOrDefault())
            {
                case PileType.Hand:
                case PileType.Play:
                    flag = true;
                    goto label_6;
            }
        }
        flag = false;
        label_6:
        if (!flag)
            return;
        await PowerCmd.Decrement((PowerModel) power);
    }
}
