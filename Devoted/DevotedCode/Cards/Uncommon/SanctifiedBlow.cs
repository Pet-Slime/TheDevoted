using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;


  
[Pool(typeof(DevotedCardPool))]
public class SanctifiedBlow() : DevotedCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new PowerVar<FaithPower>(2M), new DamageVar(10, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        SanctifiedBlow cardSource = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        FaithPower vigorPower = await PowerCmd.Apply<FaithPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["FaithPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
        IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, cardSource.DynamicVars.Cards.BaseValue, cardSource.Owner);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["FaithPower"].UpgradeValueBy(2m);
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
