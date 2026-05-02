using BaseLib.Extensions;
using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Uncommon;


  
[Pool(typeof(DevotedCardPool))]
public class WellOfFaith() : DevotedCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Triggers", 1m), new PowerVar<FaithPower>(2M)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Exhaust ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        WellOfFaith card = this;
        
        var faith = cardPlay.Card.Owner.Creature.GetPowerAmount<FaithPower>();
        if (cardPlay.Card.Owner.HasPower<FaithPower>())
        {
            await PowerCmd.Remove<FaithPower>(cardPlay.Card.Owner.Creature);
        }

        var triggers = Math.Max(0, faith) + card.DynamicVars["Triggers"].IntValue;

        for (var i = 0; i < triggers; i++)
        {
            FaithPower vigorPower = await PowerCmd.Apply<FaithPower>(choiceContext, card.Owner.Creature, (Decimal) card.DynamicVars["FaithPower"].IntValue, card.Owner.Creature, (CardModel) card);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars["FaithPower"].UpgradeValueBy(1m);
        this.EnergyCost.UpgradeBy(-1);
    }
}