using BaseLib.Utils;
using Devoted.DevotedCode.Cards.Rare;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.ChimePowers;
using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Devoted.DevotedCode.Cards.Uncommon;




[Pool(typeof(DevotedCardPool))]
public class Choir() : DevotedCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.SerenityCard];    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ MyCustomEnums.Toll ];
    
    

    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay play)
    {
        Choir cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        foreach (Creature creature in cardSource.CombatState.GetTeammatesOf(cardSource.Owner.Creature).Where<Creature>((Func<Creature, bool>) (c => c != null && c.IsAlive && c.IsPlayer)))
        {
            if (creature.Player == null)
                return;
            CardPile pile = PileType.Hand.GetPile(creature.Player);
            CardModel card2 = creature.Player.RunState.Rng.CombatCardSelection.NextItem<CardModel>((IEnumerable<CardModel>) pile.Cards);
            if (card2 == null)
                return;
            CardCmd.ApplyKeyword(card2, MyCustomEnums.Toll);
            ChoirPower demonFormPower = await PowerCmd.Apply<ChoirPower>(choiceContext, cardSource.Owner.Creature, 1, cardSource.Owner.Creature, (CardModel) cardSource);

        }
        
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}