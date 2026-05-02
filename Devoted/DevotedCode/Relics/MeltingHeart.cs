using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Devoted.DevotedCode.Relics;

  
public class MeltingHeart: RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SlipperyPower>(2M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SlipperyPower>()];
    
    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        MeltingHeart akabeko = this;
        if (side != akabeko.Owner.Creature.Side || combatState.RoundNumber > 1)
            return;
        akabeko.Flash();
        SlipperyPower vigorPower = await PowerCmd.Apply<SlipperyPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), akabeko.Owner.Creature, (Decimal) akabeko.DynamicVars["SlipperyPower"].IntValue, akabeko.Owner.Creature, (CardModel) null);
    }
}