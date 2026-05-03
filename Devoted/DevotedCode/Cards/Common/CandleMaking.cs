using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;

 
[Pool(typeof(DevotedCardPool))]
public class CandleMaking() : DevotedCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7, ValueProp.Move)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CandleMaking cardSource = this;

        await CreatureCmd.TriggerAnim(
            cardSource.Owner.Creature,
            "Cast",
            cardSource.Owner.Character.CastAnimDelay);

        var drawPile = PileType.Draw.GetPile(cardSource.Owner).Cards;

        if (drawPile.Count == 0)
            return;

        List<CardModel> validWaxTargets = new();

        foreach (CardModel c in drawPile)
        {
            if (c.Keywords.Contains(CardKeyword.Unplayable))
                continue;

            if (c.Type is CardType.Curse or CardType.Quest)
                continue;

            if (!IsValidWaxTarget(c))
                continue;

            validWaxTargets.Add(c);
        }

        if (validWaxTargets.Count == 0)
            return;

        List<CardModel> prioritized = new();

        foreach (CardModel c in validWaxTargets)
        {
            if (c.Type is CardType.Attack or CardType.Skill or CardType.Power)
                prioritized.Add(c);
        }

        IReadOnlyList<CardModel> items =
            prioritized.Count > 0 ? prioritized : validWaxTargets;

        CardModel card = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            items,
            cardSource.Owner,
            new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1)
        )).FirstOrDefault();

        if (card == null)
            return;

        CardCmd.ApplyKeyword(card, MyCustomEnums.Waxed);
        CardCmd.Preview(card);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
    
        
    private static bool IsValidWaxTarget(CardModel c)
    {
        return !(c.Keywords.Contains(MyCustomEnums.Waxed)
                 || c.Keywords.Contains(CardKeyword.Exhaust));
    }
    
}