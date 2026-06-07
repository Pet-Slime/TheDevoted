using Devoted.DevotedCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.ChimePowers;


public class ChimeBlockPower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)]; 
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        bool hasTollTag = cardPlay.Card.Keywords.Contains(MyCustomEnums.Toll);
        
        if (!(hasTollTag))
            return;
        
        await ResolveSerenityEffect(choiceContext);
    }
    
    private async Task ResolveSerenityEffect(PlayerChoiceContext choiceContext)
    {
        ChimeBlockPower power = this;
        power.Flash();
        Decimal num = await CreatureCmd.GainBlock(power.Owner, (Decimal) power.Amount, ValueProp.Unpowered, (CardPlay) null);

    }
    
    
}
