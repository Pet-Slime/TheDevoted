using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Cards.Basic;


public class TestOfFaith() : DevotedCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("FaithGain", 2m)];

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceFaithPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        TestOfFaith cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        PenanceFaithPower vigorPower = await PowerCmd.Apply<PenanceFaithPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["FaithGain"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade() => this.DynamicVars["FaithGain"].UpgradeValueBy(1M);
}