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
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;

  
[Pool(typeof(DevotedCardPool))]
public class CrushingUnder() : DevotedCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceWaxedPower>(2M), new DamageVar(15, ValueProp.Move), new HpLossVar(1M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceWaxedPower>()];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Exhaust ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CrushingUnder cardSource = this;
        
        VfxCmd.PlayOnCreatureCenter(cardSource.Owner.Creature, "vfx/vfx_bloody_impact");
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars.HpLoss.BaseValue,  ValueProp.Unpowered | ValueProp.Move, (CardModel) cardSource);
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).TargetingAllOpponents(cardSource.CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
        PenanceWaxedPower vigorPower = await PowerCmd.Apply<PenanceWaxedPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceWaxedPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["PenanceWaxedPower"].UpgradeValueBy(1m);
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}