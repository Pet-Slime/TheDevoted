using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Rare;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.ChimePowers;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Uncommon;



[Pool(typeof(DevotedCardPool))]
public class BellOfInnerWrath() : DevotedCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityCard];   
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ MyCustomEnums.Toll ]; 
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ChimeZealotryPower>(1M)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ZealPower>(), HoverTipFactory.FromPower<DevotionPower>()]; 

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        BellOfInnerWrath cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        ChimeZealotryPower demonFormPower = await PowerCmd.Apply<ChimeZealotryPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["SerenityZealotryPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["SerenityZealotryPower"].UpgradeValueBy(1m);
    }
}