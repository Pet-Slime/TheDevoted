using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devoted.DevotedCode.Powers.ChimePowers;

  
public class ChimeZealotryPower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private const string PlayMaxKey = "PlayMax";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(PlayMaxKey, 4M)];
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ZealPower>(), HoverTipFactory.FromPower<FaithPower>()]; 
    
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
        ChimeZealotryPower power = this;
        power.Flash();
        await PowerCmd.Apply<ZealPower>(choiceContext, power.Owner, power.Amount, power.Owner, null);
        await PowerCmd.Apply<FaithPower>(choiceContext, power.Owner, power.Amount * -1, power.Owner, null);

    }
}
