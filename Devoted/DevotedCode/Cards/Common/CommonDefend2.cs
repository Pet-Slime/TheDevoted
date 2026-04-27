using BaseLib.Utils;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;

public class CommonDefend2() : DevotedCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(5M), new CalculationExtraVar(1M), 
    
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) =>
        {
            CombatState combatState = card.CombatState;
            return (Decimal) (combatState != null ? combatState.PlayerCreatures.Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).Sum<Creature>((Func<Creature, int>) (c => c.GetPowerAmount<FaithPower>())) : 0);
        }))
    
    
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CommonDefend2 mirage = this;
        Decimal num = await CreatureCmd.GainBlock(mirage.Owner.Creature, mirage.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), mirage.DynamicVars.CalculatedBlock.Props, cardPlay);
    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
