using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Cards.Rare;

  
[Pool(typeof(DevotedCardPool))]
public class DivineStrength() : DevotedCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
 
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DivineStrengthPower>(1M)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        DivineStrength cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        DivineStrengthPower litanyPower = await PowerCmd.Apply<DivineStrengthPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["DivineStrengthPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DivineStrengthPower"].UpgradeValueBy(1M);
    }
}