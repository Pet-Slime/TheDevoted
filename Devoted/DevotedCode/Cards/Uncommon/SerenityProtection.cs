using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Rare;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.PenancePowers;
using Devoted.DevotedCode.Powers.SerenityPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Uncommon;



[Pool(typeof(DevotedCardPool))]
public class SerenityProtection() : DevotedCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityCard];    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SerenityBlockPower>(1M)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)]; 

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        SerenityProtection cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        SerenityBlockPower demonFormPower = await PowerCmd.Apply<SerenityBlockPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["SerenityBlockPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["SerenityBlockPower"].UpgradeValueBy(1m);
    }
}