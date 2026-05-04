using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;


[Pool(typeof(DevotedCardPool))]
public class RushLight() : DevotedCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15M, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MyCustomEnums.Waxed), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        RushLight card1 = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(card1.DynamicVars.Damage.BaseValue).FromCard((CardModel) card1).Targeting(cardPlay.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NFireBurstVfx.Create(t, 0.75f))).Execute(choiceContext);
        CardPile pile = PileType.Hand.GetPile(card1.Owner);
        var validCards = pile.Cards.Where(IsValidWaxTarget).ToList();
        if (validCards.Count == 0)
            return;
        CardModel card2 = card1.Owner.RunState.Rng.CombatCardSelection.NextItem(validCards);
        if (card2 == null)
            return;
        CardCmd.ApplyKeyword(card2, MyCustomEnums.Waxed);
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