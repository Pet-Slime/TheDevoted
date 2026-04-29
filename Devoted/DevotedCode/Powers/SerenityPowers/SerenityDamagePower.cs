using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.SerenityPowers;

  
public class SerenityDamagePower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private const string PlayMaxKey = "PlayMax";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(PlayMaxKey, 4M), new DamageVar(1, ValueProp.Move)];
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        bool hasSerenityTag = cardPlay.Card.Tags.Contains(MyCustomEnums.SerenityTrigger);
        
        if (!(hasSerenityTag))
            return;
        
        await ResolveSerenityEffect();
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        SerenityDamagePower power = this;
        if (side != CombatSide.Player || !power.CanTrigger)
            return;
        
        await ResolveSerenityEffect();
    }
    
    private async Task ResolveSerenityEffect()
    {
        SerenityDamagePower power = this;
        Creature target = power.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) power.Owner.CombatState.HittableEnemies);
        if (target == null)
            return;
        VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_attack_blunt");
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), 
            target, 
            (Decimal) power.Amount, ValueProp.Unpowered, 
            power.Owner, 
            (CardModel) null);
    }
    
    private bool CanTrigger
    {
        get
        {
            return CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.CardPlay.Card.Owner.Creature == this.Owner)) < this.DynamicVars[PlayMaxKey].IntValue;
        }
    }
}
