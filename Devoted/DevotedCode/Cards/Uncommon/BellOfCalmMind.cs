using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Rare;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.ChimePowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Uncommon;





[Pool(typeof(DevotedCardPool))]
public class BellOfCalmMind() : DevotedCard(3, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityCard];    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ MyCustomEnums.Toll ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ChimeCardCreatePower>(1M)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        BellOfCalmMind cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        if (cardSource.IsUpgraded)
        {
            ChimeUpgradedCardCreatePower demonFormCreatePower = (await PowerCmd.Apply<ChimeUpgradedCardCreatePower>(choiceContext,
                cardSource.Owner.Creature, cardSource.DynamicVars["SerenityCardCreatePower"].BaseValue,
                cardSource.Owner.Creature, (CardModel)cardSource))!;
        }
        else
        {
            ChimeCardCreatePower demonFormCreatePower = (await PowerCmd.Apply<ChimeCardCreatePower>(choiceContext,
                cardSource.Owner.Creature, cardSource.DynamicVars["SerenityCardCreatePower"].BaseValue,
                cardSource.Owner.Creature, (CardModel)cardSource))!;
        }
    }



    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}