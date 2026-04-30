using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Common;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
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

namespace Devoted.DevotedCode.Cards.Uncommon;


[Pool(typeof(DevotedCardPool))]
public class TearsOfTheFlame() : DevotedCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
      
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceEnergyPower>(2M), new HpLossVar(5M)];

    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.PenanceTrigger];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceEnergyPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        TearsOfTheFlame cardSource = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        PenanceEnergyPower vigorPower = await PowerCmd.Apply<PenanceEnergyPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceEnergyPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars.HpLoss.BaseValue,  ValueProp.Unpowered | ValueProp.Move, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["PenanceEnergyPower"].UpgradeValueBy(1M);
    }
}
