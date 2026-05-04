using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;


[Pool(typeof(DevotedCardPool))]
public class Wicklash() : DevotedCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9M, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromKeyword(MyCustomEnums.Waxed)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Wicklash cardSource = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);

        var drawPile = PileType.Discard.GetPile(cardSource.Owner).Cards;

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
        )).FirstOrDefault()!;

        if (card == null)
            return;
        
        CardCmd.ApplyKeyword(card, MyCustomEnums.Waxed);
        CardCmd.Preview(card);
        CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    
    private static bool IsValidWaxTarget(CardModel c)
    {
        return !(c.Keywords.Contains(MyCustomEnums.Waxed)
                 || c.Keywords.Contains(CardKeyword.Exhaust));
    }
}