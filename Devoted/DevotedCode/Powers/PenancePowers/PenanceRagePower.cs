using Devoted.DevotedCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.PenancePowers;

public class PenanceRagePower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()]; 
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        bool hasPenanceTag = cardPlay.Card.Tags.Contains(MyCustomEnums.PenanceTrigger);
        bool isValidStrike = cardPlay.Card.Tags.Contains(CardTag.Strike) && Owner.GetPowerAmount<CrusadePower>() > 0;

        if (!(hasPenanceTag || isValidStrike))
            return;

        await ResolvePenanceRageTriggers(choiceContext, cardPlay.Card);
    }
    
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? _,
        CardModel? cardSource)
    {
        if (target != Owner || !props.IsPoweredAttack() || result.UnblockedDamage <= 0)
            return;

        await ResolvePenanceRageTriggers(choiceContext, cardSource);
    }
    
    private async Task ResolvePenanceRageTriggers(PlayerChoiceContext choiceContext, CardModel? cardSource)
    {
        PenanceRagePower power = this;
        var player = Owner.Player;

        if (player == null)
            return;

        var triggers = player.Creature.GetPowerAmount<PenanceTriggerPower>() + 1;
        var healTrigger = player.Creature.GetPowerAmount<PenanceHealPower>();

        for (int i = 0; i < triggers; i++)
        {
            power.Flash();

            await PowerCmd.Apply<VigorPower>(choiceContext, power.Owner, power.Amount, power.Owner, cardSource);

            if (healTrigger > 0)
            {
                var healPower = player.Creature.GetPower<PenanceHealPower>();
                if (healPower != null)
                {
                    await CreatureCmd.Heal(power.Owner, healTrigger);
       //             await PowerCmd.ModifyAmount(choiceContext, healPower, -1, healPower.Owner, cardSource);

                    // refresh after modification
                    healTrigger = player.Creature.GetPowerAmount<PenanceHealPower>();
                }
            }

            if (power.Amount - 1 <= 0)
            {
                await PowerCmd.Remove(power);
                break;
            }
            else
            {
                await PowerCmd.ModifyAmount(choiceContext, this, -1, Owner, cardSource);
            }
        }
    }
}