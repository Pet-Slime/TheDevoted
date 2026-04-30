using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Common;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
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

namespace Devoted.DevotedCode.Cards.Uncommon;



[Pool(typeof(DevotedCardPool))]
public class FlamesOfPurification() : DevotedCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.PenanceTrigger];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceRetributionPower>(12M), new HpLossVar(5M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceRetributionPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        FlamesOfPurification cardSource = this;
        PenanceRetributionPower vigorPower = await PowerCmd.Apply<PenanceRetributionPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceRetributionPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars.HpLoss.BaseValue,  ValueProp.Unpowered | ValueProp.Move, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["PenanceRetributionPower"].UpgradeValueBy(1m);
    }
}
