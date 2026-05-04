using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;


[Pool(typeof(DevotedCardPool))]
public class DivineHymn() : DevotedCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [ 
        new CalculationBaseVar(5M), 
        new CalculationExtraVar(1M), 
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) =>
        {
            ICombatState combatState = card.CombatState;
            return (Decimal) (combatState != null ? combatState.PlayerCreatures.Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).Sum<Creature>((Func<Creature, int>) (c => c.GetPowerAmount<FaithPower>())) : 0);
        }))
    
    
    ];

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        DivineHymn mirage = this;
        Decimal num = await CreatureCmd.GainBlock(mirage.Owner.Creature, mirage.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), mirage.DynamicVars.CalculatedBlock.Props, cardPlay);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(1m);
    }
}
