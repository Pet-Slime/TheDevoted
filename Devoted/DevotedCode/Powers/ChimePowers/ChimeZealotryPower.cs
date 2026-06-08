using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Powers.ChimePowers;

  
public class ChimeZealotryPower: ChimeBasePower
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ZealPower>(), HoverTipFactory.FromPower<FaithPower>()]; 
    

    protected override async  Task ResolveChimeEffect(PlayerChoiceContext choiceContext)
    {
        ChimeZealotryPower power = this;
        power.Flash();
        await PowerCmd.Apply<ZealPower>(choiceContext, power.Owner, power.Amount, power.Owner, null);
        await PowerCmd.Apply<FaithPower>(choiceContext, power.Owner, power.Amount * -1, power.Owner, null);

    }
}
