using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers;

public class PenanceRagePower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()]; 
    
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? _,
        CardModel? cardSource)
    {
        PenanceRagePower power = this;
        if (target != power.Owner || !props.IsPoweredAttack() || result.UnblockedDamage <= 0)
            return;
        var player = Owner.Player;
        var triggers = player.Creature.GetPowerAmount<PenanceTriggerPower>() + 1;
        var healTrigger = player.Creature.GetPowerAmount<PenanceHealPower>();
        for (int i = 0; i < triggers; i++)
        {
            power.Flash();
            VigorPower vigorPower = await PowerCmd.Apply<VigorPower>(choiceContext, power.Owner, power.Amount,
                power.Owner, (CardModel)null);

            if (healTrigger > 0)
            {               
                var healPower = player.Creature.GetPower<PenanceHealPower>();
                if (healPower != null)
                {
                    
                    await CreatureCmd.Heal(power.Owner, healTrigger);
                    await PowerCmd.ModifyAmount(choiceContext, healPower, -1, healPower.Owner, (CardModel)null);
                    healTrigger = player.Creature.GetPowerAmount<PenanceHealPower>();
                }
            }

            if (power.Amount - 1 <= 0)
            {
                await PowerCmd.Remove((PowerModel)power);
                break;
            }
            else
            {
                await PowerCmd.ModifyAmount(choiceContext, this, -1, Owner, (CardModel)null);
            }
        }
    }
}