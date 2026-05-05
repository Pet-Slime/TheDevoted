using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
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
public class ChainLighting() : DevotedCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12M, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MyCustomEnums.Waxed), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ChainLighting veilpiercer = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(veilpiercer.DynamicVars.Damage.BaseValue).FromCard((CardModel) veilpiercer).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        ChainLightingPower veilpiercerPower = await PowerCmd.Apply<ChainLightingPower>(choiceContext, veilpiercer.Owner.Creature, 1M, veilpiercer.Owner.Creature, (CardModel) veilpiercer);
    }
    
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(6M);
}
