using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Cards.Uncommon;


  
[Pool(typeof(DevotedCardPool))]
public class LatentSpark() : DevotedCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceEnergyPower>(2M), new PowerVar<PenanceDrawPower>(2M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceEnergyPower>(), HoverTipFactory.FromPower<PenanceDrawPower>()];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Exhaust ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        LatentSpark cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        PenanceEnergyPower energyPower = await PowerCmd.Apply<PenanceEnergyPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceEnergyPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
        PenanceDrawPower drawPower = await PowerCmd.Apply<PenanceDrawPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceDrawPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["PenanceEnergyPower"].UpgradeValueBy(1M);
        DynamicVars["PenanceDrawPower"].UpgradeValueBy(1M);
    }
}