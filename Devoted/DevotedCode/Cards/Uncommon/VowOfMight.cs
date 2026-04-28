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
public class VowOfMight() : DevotedCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VigorPower>(3M), new PowerVar<VowOfStrengthPower>(2M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>(), HoverTipFactory.FromPower<StrengthPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        VowOfMight cardSource = this;
        
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        VigorPower vigorPower = await PowerCmd.Apply<VigorPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["VigorPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
        VowOfStrengthPower vowOfStrengthPower = await PowerCmd.Apply<VowOfStrengthPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["VowOfStrengthPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["VigorPower"].UpgradeValueBy(2m);
    }
}