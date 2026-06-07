using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.ChimePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Rare;



[Pool(typeof(DevotedCardPool))]
public class BellOfForesight() : DevotedCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityCard];    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ MyCustomEnums.Toll ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ChimeDrawPower>(1M)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        BellOfForesight cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        ChimeDrawPower demonFormPower = await PowerCmd.Apply<ChimeDrawPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["ChimeDrawPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["ChimeDrawPower"].UpgradeValueBy(1m);
    }
}