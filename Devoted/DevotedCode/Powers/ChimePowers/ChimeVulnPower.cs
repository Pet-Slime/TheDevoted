using Devoted.DevotedCode.Keywords;
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
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devoted.DevotedCode.Powers.ChimePowers;

  
public class ChimeVulnPower: ChimeBasePower
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()]; 
    
    
    protected override async  Task ResolveChimeEffect(PlayerChoiceContext choiceContext)
    {
        ChimeVulnPower power = this;
        Creature target = power.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) power.Owner.CombatState.HittableEnemies);
        if (target == null)
            return;
        power.Flash();
        VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_attack_blunt");
        await PowerCmd.Apply<VulnerablePower>(choiceContext, target, power.Amount, power.Owner, null);
    }
    
}
