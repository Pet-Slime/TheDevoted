using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.PenancePowers;
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
public class GuidingLight() : DevotedCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
      
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceDrawPower>(2M), new DamageVar(12, ValueProp.Move)];

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceDrawPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        GuidingLight cardSource = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        PenanceDrawPower vigorPower = await PowerCmd.Apply<PenanceDrawPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceDrawPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["PenanceDrawPower"].UpgradeValueBy(1M);
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}
