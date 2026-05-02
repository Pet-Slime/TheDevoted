using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;


[Pool(typeof(DevotedCardPool))]
public class Retaliation() : DevotedCard(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceRetributionPower>(1), new DamageVar(8, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceRetributionPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Retaliation cardSource = this;
        VfxCmd.PlayOnCreatureCenter(cardSource.Owner.Creature, "vfx/vfx_bloody_impact");
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).TargetingAllOpponents(cardSource.CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
        PenanceRetributionPower vigorPower = await PowerCmd.Apply<PenanceRetributionPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceRetributionPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["PenanceRetributionPower"].UpgradeValueBy(1m);
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}