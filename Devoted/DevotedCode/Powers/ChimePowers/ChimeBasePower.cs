using BaseLib.Extensions;
using Devoted.DevotedCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.ChimePowers;

public abstract class ChimeBasePower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Only trigger from this power's owner, or from a card owner with Choir.
        if (!CardTriggers(cardPlay, Owner))
            return;

        // Only Toll cards trigger Chime effects.
        if (!cardPlay.Card.Keywords.Contains(MyCustomEnums.Toll))
            return;

        int triggers = GetTollTriggerCount(cardPlay.Card);

        for (int i = 0; i < triggers; i++)
        {
            await ResolveChimeEffect(choiceContext);
        }
    }

    private static bool CardTriggers(CardPlay cardPlay, Creature chimeOwner)
    {
        // Your own Toll cards always trigger your own Chime effects.
        if (cardPlay.Card.Owner == chimeOwner.Player)
            return true;

        // If the card's owner has Choir, their Toll cards can trigger allied Chime effects.
        if (cardPlay.Card.Owner.HasPower<ChoirPower>())
            return true;

        return false;
    }

    private static int GetTollTriggerCount(CardModel card)
    {
        if (!card.Tags.Contains(MyCustomEnums.TollTriggers))
            return 1;

        return card.IsUpgraded ? 4 : 3;
    }

    protected abstract Task ResolveChimeEffect(PlayerChoiceContext choiceContext);
}