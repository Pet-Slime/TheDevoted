using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Rare;


[Pool(typeof(DevotedCardPool))]
public class WaxOff() : DevotedCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [ 
        
        
        (DynamicVar) new HealVar(1M),
        (DynamicVar) new CalculationBaseVar(0M),
        (DynamicVar) new CalculationExtraVar(1M),
        (DynamicVar) new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) WaxOff.GetStatuses(card.Owner).Count<CardModel>()))
        
    
    ];
    

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MyCustomEnums.Waxed)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Exhaust ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        WaxOff cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        List<CardModel> list = WaxOff.GetStatuses(cardSource.Owner).ToList<CardModel>();
        int statusCount = (int) ((CalculatedVar) cardSource.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target);
        foreach (CardModel card2 in list)
            await CardCmd.Exhaust(choiceContext, card2);
        
        await CreatureCmd.Heal(cardSource.Owner.Creature, cardSource.DynamicVars.Heal.IntValue * statusCount);
    }
    
    private static IEnumerable<CardModel> GetStatuses(Player owner)
    {
        return owner.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Keywords.Contains(MyCustomEnums.Waxed) && c.Pile.Type != PileType.Exhaust));
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(1m);
    }
}