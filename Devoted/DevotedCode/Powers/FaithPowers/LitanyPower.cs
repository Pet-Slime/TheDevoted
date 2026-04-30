using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Powers.FaithPowers;


public class LitanyPower: DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterEnergyReset(Player player)
    {
        LitanyPower power = this;
        if (player != power.Owner.Player)
            return;
        power.Flash();
        FaithPower litanyPower = await PowerCmd.Apply<FaithPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), power.Owner, (Decimal) power.Amount, power.Owner, (CardModel) null);
    }
}