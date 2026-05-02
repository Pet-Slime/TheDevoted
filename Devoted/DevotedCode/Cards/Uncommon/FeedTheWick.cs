using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;


  
[Pool(typeof(DevotedCardPool))]
public class FeedTheWick() : DevotedCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceWaxedPower>(2M), new BlockVar(5, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        FeedTheWick cardSource = this;
        
        await CommonActions.CardBlock(this, cardPlay);
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        PenanceWaxedPower energyPower = await PowerCmd.Apply<PenanceWaxedPower>(choiceContext, cardSource.Owner.Creature, (Decimal) cardSource.DynamicVars["PenanceWaxedPower"].IntValue, cardSource.Owner.Creature, (CardModel) cardSource);
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        CardModel card = (await CardSelectCmd.FromHand(choiceContext, cardSource.Owner, prefs, (Func<CardModel, bool>) null, (AbstractModel) cardSource)).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        await CardCmd.Exhaust(choiceContext, card);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(3m);
    }
}