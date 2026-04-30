using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Rare;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using Devoted.DevotedCode.Powers.SerenityPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Uncommon;



[Pool(typeof(DevotedCardPool))]
public class SerenityFaith() : DevotedCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityCard];    
    
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()]; 
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SerenityFaithPower>(1M)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        SerenityFaith cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        SerenityFaithPower demonFormPower = await PowerCmd.Apply<SerenityFaithPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["SerenityFaithPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["SerenityFaithPower"].UpgradeValueBy(1m);
    }
}