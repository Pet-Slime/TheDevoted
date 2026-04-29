using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Uncommon;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;


  
[Pool(typeof(DevotedCardPool))]
public class CandleLight() : DevotedCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceWaxedPower>(1M), new BlockVar(3, ValueProp.Move)];


    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceWaxedPower>(), HoverTipFactory.FromPower<WeakPower>(),];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CandleLight cardSource = this;
        
        await CommonActions.CardBlock(this, cardPlay);
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        PenanceWaxedPower vigorPower = await PowerCmd.Apply<PenanceWaxedPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceWaxedPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["PenanceWaxedPower"].UpgradeValueBy(1m);
        DynamicVars["Block"].UpgradeValueBy(2m);
    }
}