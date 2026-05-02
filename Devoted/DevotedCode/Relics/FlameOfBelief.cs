using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace Devoted.DevotedCode.Relics;


  
  
[Pool(typeof(DevotedRelicPool))]
public class FlameOfBelief : DevotedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FaithPower>(3M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()];
    
    public override async Task AfterEnergyResetLate(Player player)
    {
        FlameOfBelief relic = this;
        if (player != relic.Owner)
            return;;
        relic.Flash();
        FaithPower vigorPower = await PowerCmd.Apply<FaithPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), relic.Owner.Creature, (Decimal) relic.DynamicVars["FaithPower"].IntValue, relic.Owner.Creature, (CardModel) null);
    }
    
}