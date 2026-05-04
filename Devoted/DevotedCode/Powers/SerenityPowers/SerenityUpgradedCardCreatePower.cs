using Devoted.DevotedCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Powers.SerenityPowers;




public class SerenityUpgradedCardCreatePower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        bool hasSerenityTag = cardPlay.Card.Tags.Contains(MyCustomEnums.SerenityTrigger);
        
        if (!(hasSerenityTag))
            return;
        
        SerenityUpgradedCardCreatePower creativeAiCreatePower = this;
        var player = Owner.Player;
        for (int i = 0; i < creativeAiCreatePower.Amount; ++i)
        {
            CardModel card = CardFactory.GetDistinctForCombat(player, player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains(MyCustomEnums.SerenityCard))), 1, player.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
            if (card != null)
            {
                CardCmd.Upgrade(card);
                CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, creativeAiCreatePower.Owner.Player);
            }
        }
    }
    
    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        SerenityUpgradedCardCreatePower creativeAiCreatePower = this;
        if (player != creativeAiCreatePower.Owner.Player)
            return;
        for (int i = 0; i < creativeAiCreatePower.Amount; ++i)
        {
            CardModel card = CardFactory.GetDistinctForCombat(player, player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains(MyCustomEnums.SerenityCard))), 1, player.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
            if (card != null)
            {
                CardCmd.Upgrade(card);
                CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, creativeAiCreatePower.Owner.Player);
            }
        }
    }
}