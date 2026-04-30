using BaseLib.Extensions;
using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;


  
[Pool(typeof(DevotedCardPool))]
public class Sanctification() : DevotedCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
      
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Triggers", 2m), new PowerVar<FaithPower>(1M), new BlockVar(1M, ValueProp.Move)];

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>(), HoverTipFactory.FromPower<VigorPower>(), HoverTipFactory.FromPower<ZealPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Sanctification gatherLight = this;
        
        var zeal = cardPlay.Card.Owner.Creature.GetPowerAmount<ZealPower>();
        var vigor = cardPlay.Card.Owner.Creature.GetPowerAmount<VigorPower>();
        if (cardPlay.Card.Owner.HasPower<ZealPower>())
        {
            await PowerCmd.Remove<ZealPower>(cardPlay.Card.Owner.Creature);
        }

        if (cardPlay.Card.Owner.HasPower<VigorPower>())
        {
            await PowerCmd.Remove<VigorPower>(cardPlay.Card.Owner.Creature);
        }

        var triggers = zeal + vigor + gatherLight.DynamicVars["Triggers"].IntValue;

        for (var i = 0; i < triggers; i++)
        {
            Decimal num = await CreatureCmd.GainBlock(gatherLight.Owner.Creature, gatherLight.DynamicVars.Block, cardPlay);
            FaithPower vigorPower = await PowerCmd.Apply<FaithPower>(choiceContext, gatherLight.Owner.Creature, (Decimal) gatherLight.DynamicVars["FaithPower"].IntValue, gatherLight.Owner.Creature, (CardModel) gatherLight);
        }
    }


    protected override void OnUpgrade()
    {
        DynamicVars["FaithPower"].UpgradeValueBy(1M);
        DynamicVars["Triggers"].UpgradeValueBy(1M);
        DynamicVars.Block.UpgradeValueBy(1m);
    }
}