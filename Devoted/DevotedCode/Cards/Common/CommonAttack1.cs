using BaseLib.Utils;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;

public class CommonAttack1() : DevotedCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("FaithGain", 3m), new DamageVar(9, ValueProp.Move)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CommonAttack1 cardSource = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        FaithPower faithPower = await PowerCmd.Apply<FaithPower>(cardSource.Owner.Creature, cardSource.DynamicVars["FaithGain"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["FaithGain"].UpgradeValueBy(2m);
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
