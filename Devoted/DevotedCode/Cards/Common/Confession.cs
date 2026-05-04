using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;

  
[Pool(typeof(DevotedCardPool))]
public class Confession() : DevotedCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("FaithGain", 2m), new DamageVar(1M, ValueProp.Unpowered | ValueProp.Move)];

    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.PenanceTrigger];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Confession cardSource = this;
        
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars.Damage, (CardModel) cardSource);
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        FaithPower vigorPower = await PowerCmd.Apply<FaithPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["FaithGain"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["FaithGain"].UpgradeValueBy(2m);
    }
}