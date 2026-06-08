using Devoted.DevotedCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Powers.ChimePowers;

  
public class ChimeDamagePower: ChimeBasePower
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(1, ValueProp.Move)];
    

    protected override async Task ResolveChimeEffect(PlayerChoiceContext choiceContext)
    {
        ChimeDamagePower power = this;
        Creature target = power.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) power.Owner.CombatState.HittableEnemies);
        if (target == null)
            return;
        power.Flash();
        VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_attack_blunt");
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), 
            target, 
            (Decimal) power.Amount, ValueProp.Unpowered, 
            power.Owner, 
            (CardModel) null);
    }
}
