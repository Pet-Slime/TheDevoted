using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.SerenityPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Rare;



[Pool(typeof(DevotedCardPool))]
public class SerenityForesight() : DevotedCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityCard];    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SerenityDrawPower>(1M)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        SerenityForesight cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        SerenityDrawPower demonFormPower = await PowerCmd.Apply<SerenityDrawPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["SerenityDrawPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["SerenityDrawPower"].UpgradeValueBy(1m);
    }
}