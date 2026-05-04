using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Rare;


[Pool(typeof(DevotedCardPool))]
public class Overexert() : DevotedCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(5M), new PowerVar<VigorPower>(25M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<VigorPower>()];

    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Exhaust ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Overexert cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, cardSource.Owner.Creature, -cardSource.DynamicVars["StrengthPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
        VigorPower vigorPower = await PowerCmd.Apply<VigorPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["VigorPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }

    
    protected override void OnUpgrade()
    {
        DynamicVars["VigorPower"].UpgradeValueBy(5m);
    }
}