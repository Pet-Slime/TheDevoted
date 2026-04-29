using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Common;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
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
public class ZealousStrike() : DevotedCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("ZealGain", 1m), new DamageVar(11, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ZealPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ZealousStrike cardSource = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        
        ZealPower vigorPower = await PowerCmd.Apply<ZealPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["ZealGain"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["ZealGain"].UpgradeValueBy(1m);
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
