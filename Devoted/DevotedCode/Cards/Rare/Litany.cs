using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Rare;


[Pool(typeof(DevotedCardPool))]
public class Litany() : DevotedCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
 
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<LitanyPower>(2M)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        Litany cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        LitanyPower litanyPower = await PowerCmd.Apply<LitanyPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["LitanyPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}