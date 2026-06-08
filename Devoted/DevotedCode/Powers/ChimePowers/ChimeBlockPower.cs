using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.ChimePowers;

public class ChimeBlockPower : ChimeBasePower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    protected override async Task ResolveChimeEffect(PlayerChoiceContext choiceContext)
    {
        Flash();

        await CreatureCmd.GainBlock(
            Owner,
            (decimal)Amount,
            ValueProp.Unpowered,
            null
        );
    }
}