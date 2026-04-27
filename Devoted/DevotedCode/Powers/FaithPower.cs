using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Powers;


public class FaithPower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => true;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx,
        PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        var player = Owner.Player;
        if (power is not FaithPower || amount <= 0 || applier != null || player == null)
            return;

        var trigger = Amount / 10;
        if (amount <= 0)
            return;

        var VigorGain = Math.Min(0, (Decimal) 5 + player.Creature.GetPowerAmount<ZealPower>()) ;
        var baseDevotion = (Decimal) player.Creature.GetPowerAmount<DevotionPower>();

        VigorPower vigorPower = await PowerCmd.Apply<VigorPower>(ctx, Owner, VigorGain, applier, cardSource, true);

        var newFaith = baseDevotion-10;
        await PowerCmd.ModifyAmount(ctx,this, newFaith, Owner, cardSource);

    }
}