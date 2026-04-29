using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.SerenityPowers;

  
public class SerenityZealotryPower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private const string PlayMaxKey = "PlayMax";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(PlayMaxKey, 4M)];
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        bool hasSerenityTag = cardPlay.Card.Tags.Contains(MyCustomEnums.SerenityTrigger);
        
        if (!(hasSerenityTag))
            return;
        
        await ResolveSerenityEffect(choiceContext);
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        SerenityZealotryPower power = this;
        if (side != CombatSide.Player || !power.CanTrigger)
            return;
        
        await ResolveSerenityEffect(choiceContext);
    }
    
    private async Task ResolveSerenityEffect(PlayerChoiceContext choiceContext)
    {
        SerenityZealotryPower power = this;
        Decimal num = await CreatureCmd.GainBlock(power.Owner, (Decimal) power.Amount, ValueProp.Unpowered, (CardPlay) null);

    }
    
    private bool CanTrigger
    {
        get
        {
            return CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.CardPlay.Card.Owner.Creature == this.Owner)) < this.DynamicVars[PlayMaxKey].IntValue;
        }
    }
}
