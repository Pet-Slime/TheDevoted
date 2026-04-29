using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Powers.FaithPowers;


public class FaithPower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()]; 
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx,
        PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        var player = Owner.Player;
        MainFile.Logger.Info($"Faith amount gained: {amount}");
        if (power is not FaithPower || amount <= 0 || applier != Owner || player == null)
            return;

        MainFile.Logger.Info($"Total player amount: {Amount}");

        var trigger = Math.Floor((decimal)(Amount / 10));
        if (trigger < 1)
            return;

        var zeal = player.Creature.GetPowerAmount<ZealPower>();
        var devotion = (decimal)player.Creature.GetPowerAmount<DevotionPower>();

        var vigorGainPerTrigger = Math.Max(0, 5 + zeal);
        var totalVigor = vigorGainPerTrigger * trigger;

        await PowerCmd.Apply<VigorPower>(ctx, Owner, totalVigor, applier, cardSource, true);

        
        var remainder = Amount - (10 * trigger);
        var finalFaith = remainder + devotion;

        var delta = finalFaith - Amount;

        await PowerCmd.ModifyAmount(ctx, this, delta, Owner, cardSource);
    }
}