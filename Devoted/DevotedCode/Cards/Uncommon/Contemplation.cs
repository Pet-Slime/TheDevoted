using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;

  
[Pool(typeof(DevotedCardPool))]
public class Contemplation() : DevotedCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(0), new PowerVar<DevotionPower>(1M), new PowerVar<ZealPower>(1M)];


    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>(), HoverTipFactory.FromPower<DevotionPower>(), HoverTipFactory.FromPower<ZealPower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Contemplation cardSource = this;
        
        await CommonActions.CardBlock(this, cardPlay);
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        
        var faith = cardPlay.Card.Owner.Creature.GetPowerAmount<FaithPower>();
        IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, cardSource.DynamicVars.Cards.BaseValue+faith, cardSource.Owner);
        DevotionPower devotionPower = await PowerCmd.Apply<DevotionPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["DevotionPower"].IntValue * -1, cardSource.Owner.Creature, (CardModel) cardSource);
        ZealPower zealPower = await PowerCmd.Apply<ZealPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["ZealPower"].IntValue *-1, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}