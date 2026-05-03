using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Rare;

[Pool(typeof(DevotedCardPool))]
public class WaxCopy() : DevotedCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(1),new CardsVar(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MyCustomEnums.Waxed)];

    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Exhaust ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        WaxCopy source = this;
        
        var cards = await CardSelectCmd.FromHand(choiceContext, 
            source.Owner, 
            new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, source.DynamicVars.Repeat.IntValue), 
            (Func<CardModel, bool>) IsValidWaxTarget, 
            (AbstractModel) source);
        
        
        foreach (CardModel card in cards)
        {
            await CardCmd.Exhaust(choiceContext, card);
            for (int i = 0; i < source.DynamicVars.Cards.IntValue; i++)
            {
                var clone = card.CreateClone();
                CardCmd.ApplyKeyword(clone, MyCustomEnums.Waxed);
                await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, card.Owner);
            }

        }

    }
    
    private static bool IsValidWaxTarget(CardModel c)
    {
        return !(c.Keywords.Contains(MyCustomEnums.Waxed)
                 || c.Keywords.Contains(CardKeyword.Exhaust));
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Cards"].UpgradeValueBy(1m);
    }
}