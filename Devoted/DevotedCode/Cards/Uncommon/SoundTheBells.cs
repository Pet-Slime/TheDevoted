using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.ChimePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;


[Pool(typeof(DevotedCardPool))]
public class SoundTheBells() : DevotedCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{

    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityTrigger];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ MyCustomEnums.Toll ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(4, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        SoundTheBells card = this;
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Attack", card.Owner.Character.AttackAnimDelay);
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard((CardModel) card).TargetingAllOpponents(card.CombatState).WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}