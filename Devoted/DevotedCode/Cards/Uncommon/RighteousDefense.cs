using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Common;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
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
public class RighteousDefense() : DevotedCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
      
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceRetributionPower>(3M), new BlockVar(8M, ValueProp.Move)];

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceRetributionPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        RighteousDefense gatherLight = this;
        Decimal num = await CreatureCmd.GainBlock(gatherLight.Owner.Creature, gatherLight.DynamicVars.Block, cardPlay);
        PenanceRetributionPower vigorPower = await PowerCmd.Apply<PenanceRetributionPower>(choiceContext, gatherLight.Owner.Creature, (Decimal) gatherLight.DynamicVars["PenanceRetributionPower"].IntValue, gatherLight.Owner.Creature, (CardModel) gatherLight);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["PenanceRetributionPower"].UpgradeValueBy(1M);
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}