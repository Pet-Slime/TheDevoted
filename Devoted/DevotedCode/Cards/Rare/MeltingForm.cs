using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Cards.Rare;



[Pool(typeof(DevotedCardPool))]
public class MeltingForm() : DevotedCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{

  //  public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SlipperyPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SlipperyPower>(3M)];

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        MeltingForm cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        SlipperyPower demonFormPower = await PowerCmd.Apply<SlipperyPower>(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars["SlipperyPower"].BaseValue, cardSource.Owner.Creature, (CardModel) cardSource);
    }
    
    protected override void OnUpgrade() => this.DynamicVars["SlipperyPower"].UpgradeValueBy(1M);
}