using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.PenancePowers;


public class PenanceRetributionPower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (cardPlay.Card.Owner != Owner.Player)
            return;

        bool hasPenanceTag = cardPlay.Card.Tags.Contains(MyCustomEnums.PenanceTrigger);
        bool isValidStrike = cardPlay.Card.Tags.Contains(CardTag.Strike) && Owner.GetPowerAmount<CrusadePower>() > 0;

        if (!(hasPenanceTag || isValidStrike))
            return;

        PenanceRetributionPower power = this;
        var target = cardPlay.Target;
        if (target == null || target == power.Owner)
            return;

        await ResolvePenanceRetributionTriggers(choiceContext, target, cardPlay.Card);
    }
    
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner || !props.IsPoweredAttack() || result.UnblockedDamage <= 0 || dealer == null)
            return;

        await ResolvePenanceRetributionTriggers(choiceContext, dealer, cardSource);
    }
    
    private async Task ResolvePenanceRetributionTriggers(PlayerChoiceContext choiceContext, Creature dealer, CardModel? cardSource)
    {
        PenanceRetributionPower power = this;
        var player = Owner.Player;

        if (player == null)
            return;

        var triggers = player.Creature.GetPowerAmount<PenanceTriggerPower>() + 1;
        var healTrigger = player.Creature.GetPowerAmount<PenanceHealPower>();

        for (int i = 0; i < triggers; i++)
        {
            power.Flash();

            await CreatureCmd.Damage(choiceContext, dealer, (Decimal)power.Amount,
                ValueProp.Unpowered | ValueProp.SkipHurtAnim, power.Owner, cardSource);

            if (healTrigger > 0)
            {
                var healPower = player.Creature.GetPower<PenanceHealPower>();
                if (healPower != null)
                {
                    await CreatureCmd.Heal(power.Owner, healTrigger);
                    await PowerCmd.ModifyAmount(choiceContext, healPower, -1, healPower.Owner, cardSource);

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