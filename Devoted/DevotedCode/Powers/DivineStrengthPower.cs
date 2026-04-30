using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Powers;

  
public class DivineStrengthPower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()]; 
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx,
        PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        var player = Owner.Player;
        if (power is not VigorPower || amount <= 0 || applier != Owner || player == null)
            return;

        this.Flash();
        await PowerCmd.Apply<StrengthPower>(ctx, Owner, amount*this.Amount, applier, cardSource, true);

        var delta = amount * -1;

        await PowerCmd.ModifyAmount(ctx, power, delta, Owner, cardSource);
    }
}