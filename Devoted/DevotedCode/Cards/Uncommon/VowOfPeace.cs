using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
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
public class VowOfPeace() : DevotedCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1M), new PowerVar<VowOfPeacePower>(2M)];


    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<ZealPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        VowOfPeace cardSource = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, (Decimal) cardSource.DynamicVars["WeakPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
        VowOfPeacePower vigorPower = await PowerCmd.Apply<VowOfPeacePower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["VowOfPeacePower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["WeakPower"].UpgradeValueBy(1m);
        this.EnergyCost.UpgradeBy(-1);
    }
}