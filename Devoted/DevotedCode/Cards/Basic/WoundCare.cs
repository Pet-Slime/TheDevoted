using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Basic;



public class WoundCare() : DevotedCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(5)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        WoundCare notYet = this;
        await CreatureCmd.TriggerAnim(notYet.Owner.Creature, "Cast", notYet.Owner.Character.CastAnimDelay);
        await CreatureCmd.Heal(notYet.Owner.Creature, notYet.DynamicVars.Heal.BaseValue);
    }


    protected override void OnUpgrade() => this.DynamicVars.Heal.UpgradeValueBy(3M);
}