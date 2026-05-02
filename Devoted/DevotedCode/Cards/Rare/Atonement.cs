using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Common;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Rare;

[Pool(typeof(DevotedCardPool))]
public class Atonement() : DevotedCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
 
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceHealPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceHealPower>(1M)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        Atonement cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        PenanceHealPower demonFormPower = await PowerCmd.Apply<PenanceHealPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["PenanceHealPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}