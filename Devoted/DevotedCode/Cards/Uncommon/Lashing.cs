using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Rare;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;


[Pool(typeof(DevotedCardPool))]
public class Lashing() : DevotedCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<PenanceRagePower>(2M), new DamageVar(3, ValueProp.Move)];


    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceRagePower>()];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        Lashing card = this;
        ArgumentNullException.ThrowIfNull((object)cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).WithHitCount(3)
            .FromCard((CardModel)card).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        PenanceRagePower vigorPower = await PowerCmd.Apply<PenanceRagePower>(choiceContext, card.Owner.Creature,
            (Decimal)card.DynamicVars["PenanceRagePower"].IntValue, card.Owner.Creature, (CardModel)card);
    }

    protected override void OnUpgrade() => this.DynamicVars["PenanceRagePower"].UpgradeValueBy(2M);
}