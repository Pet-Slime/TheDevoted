using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Common;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.VowPowers;
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
public class VowOfAsh() : DevotedCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.VowOfAsh];    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(27, ValueProp.Move), new PowerVar<VowOfAshPower>(2M)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        VowOfAsh cardSource = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        
        VowOfAshPower vowOfAshPower = await PowerCmd.Apply<VowOfAshPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["VowOfAshPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(15m);
    }
}
