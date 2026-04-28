using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Rare;



[Pool(typeof(DevotedCardPool))]
public class Judgement() : DevotedCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
 
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceTriggerPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceTriggerPower>(1M)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        Judgement cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        PenanceTriggerPower demonFormPower = await PowerCmd.Apply<PenanceTriggerPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["PenanceTriggerPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade() => this.DynamicVars["PenanceTriggerPower"].UpgradeValueBy(1M);
}